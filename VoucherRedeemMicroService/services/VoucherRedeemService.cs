using System;
using System.Text.Json;
using System.Threading.Tasks;
using Beis.Htg.VendorSme.Database.Models;
using Microsoft.Extensions.Logging;
using smevoucherencryption;
using VoucherCheckService.services.interfaces;
using VoucherRedeemService.interfaces;
using VoucherUpdateService.domain.entities;

namespace VoucherRedeemMicroService.services
{
    public class VoucherRedeemService : IVoucherRedeemService
    {
        private readonly IEncryptionService _encryptionService;
        private readonly ITokenRepository _tokenRepository;
        private readonly IVendorCompanyRepository _vendorCompanyRepository;
        private readonly ILogger<VoucherRedeemService> _logger;
        private IVendorAPICallStatusServices _vendorApiCallStatusService;

        public VoucherRedeemService(
            ILogger<VoucherRedeemService> logger,
            IEncryptionService encryptionService,
            ITokenRepository tokenRepository,
            IVendorCompanyRepository vendorCompanyRepository, IVendorAPICallStatusServices vendorApiCallStatusService)
        {
            _encryptionService = encryptionService;
            _tokenRepository = tokenRepository;
            _vendorCompanyRepository = vendorCompanyRepository;
            _logger = logger;
            _vendorApiCallStatusService = vendorApiCallStatusService;
        }

        private async Task logAPiCallStatus(vendor_api_call_status vendor_api_call_status, VoucherUpdateResponse voucherResponse)
        {
            vendor_api_call_status.result = JsonSerializer.Serialize(voucherResponse);
            _vendorApiCallStatusService.LogRequestDetails(vendor_api_call_status);
        }
        private async Task<VoucherUpdateResponse> getVoucherErrorResponse(VoucherUpdateRequest voucherRequest, int errorCode, string message)
        {
            _logger.LogError("There was an error checking the voucher ({errorCode}) : {Message}", errorCode, message);
            var vendorApiCallStatus = _vendorApiCallStatusService.CreateLogRequestDetails(voucherRequest);

            var voucherResponse = new VoucherUpdateResponse
            {
                status = "ERROR",
                errorCode = errorCode,
                message = message,
                voucherCode = voucherRequest.voucherCode
            };


            vendorApiCallStatus.error_code = "400";
            await logAPiCallStatus(vendorApiCallStatus, voucherResponse);
            return voucherResponse;
        }

        public async Task<VoucherUpdateResponse> GetVoucherResponse(VoucherUpdateRequest voucherRequest)
        {
            _logger.LogInformation("VoucherRedeemServiceRequest: {@VoucherUpdateRequest}", JsonSerializer.Serialize(voucherRequest));
            
            var voucherResponse = new VoucherUpdateResponse();
            
            try
            {
                var vendorCompanySingle = _vendorCompanyRepository.GetVendorCompanyByRegistration(voucherRequest.registration);
                var decryptedVoucherCode = DecryptVoucher(voucherRequest, vendorCompanySingle);

                var token = _tokenRepository.GetToken(decryptedVoucherCode);

                if (token == null)
                {
                    return await getVoucherErrorResponse(voucherRequest, 10, "Unknown TOKEN");                   
                }
                // reject tokens with any cancellation status
                if (token.cancellation_status_id.HasValue)
                {
                    return await getVoucherErrorResponse(voucherRequest, 40, "Cancelled token");
                }

                if (vendorCompanySingle == null || !IsValidVendor(voucherRequest, vendorCompanySingle))
                {
                    return await getVoucherErrorResponse(voucherRequest, 10, "Unknown token or company");
                }

                return await ProcessVoucherResponse(token, voucherRequest);
            }
            catch
            {
                return await getVoucherErrorResponse(voucherRequest, 10, "Unknown token or company");
            }
            
            _logger.LogInformation("VoucherRedeemServiceResponse: {@VoucherResponse}", voucherResponse);
            
            return voucherResponse;
        }

        private async Task< VoucherUpdateResponse> ProcessVoucherResponse(token token, VoucherUpdateRequest voucherRequest)
        {
            var voucherResponse = new VoucherUpdateResponse();
            
            if (token.token_expiry.CompareTo(DateTime.Now) < 0)
            {
                return await getVoucherErrorResponse(voucherRequest, 20, "Expired Token");
            }

            if (token.authorisation_code != voucherRequest.authorisationCode)
            {
                return await getVoucherErrorResponse(voucherRequest, 30, "Unknown Authorisation code");
            }
            
            voucherResponse.status = "OK"; 
            voucherResponse.errorCode = 0;
            voucherResponse.message = "Successful check - proceed";
            voucherResponse.voucherCode = voucherRequest.voucherCode;

            if (token.reconciliation_status_id is
                (long)ReconciliationStatus.PendingReconciliation or 
                (long)ReconciliationStatus.Reconciled)
            {
                return await getVoucherErrorResponse(voucherRequest, 10, "Already reconciled or pending reconciliation");
            }

            if (token.redemption_status_id is
                (long)RedemptionStatus.PendingRedemption or
                (long)RedemptionStatus.Redeemed)
            {
                return await getVoucherErrorResponse(voucherRequest, 10, "Already reconciled or pending reconciliation");
            }

            token.reconciliation_status_id = (long)ReconciliationStatus.PendingReconciliation;
            token.redemption_status_id = (long)RedemptionStatus.PendingRedemption;
            token.redemption_date = DateTime.Now;
            _tokenRepository.UpdateToken(token);

            var vendorApiCallStatus = _vendorApiCallStatusService.CreateLogRequestDetails(voucherRequest);
            vendorApiCallStatus.error_code = "200";
            await logAPiCallStatus(vendorApiCallStatus, voucherResponse); 

            return voucherResponse;
        }

        private string DecryptVoucher(VoucherUpdateRequest voucherRequest, vendor_company vendorCompany) =>
            _encryptionService.Decrypt(
                $"{voucherRequest.voucherCode}==", 
                vendorCompany.registration_id + vendorCompany.vendorid);

        public bool IsValidVendor(VoucherUpdateRequest voucherRequest, vendor_company vendorCompany) =>
            voucherRequest.registration == vendorCompany.registration_id &&
            voucherRequest.accessCode == vendorCompany.access_secret;
    }
}