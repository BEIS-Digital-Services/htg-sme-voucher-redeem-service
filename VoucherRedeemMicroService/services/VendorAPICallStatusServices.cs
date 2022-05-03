using System;
using Beis.Htg.VendorSme.Database.Models;
using VoucherCheckService.services.interfaces;
using VoucherRedeemService.interfaces;
using System.Text.Json;
using VoucherUpdateService.domain.entities;
using System.Threading.Tasks;

namespace VoucherRedeemMicroService.services
{
    public class VendorAPICallStatusServices: IVendorAPICallStatusServices
    {
        private IVendorAPICallStatusRepository _vendorApiCallStatusRepository;
        
        public VendorAPICallStatusServices(IVendorAPICallStatusRepository vendorApiCallStatusRepository)
        {
            _vendorApiCallStatusRepository = vendorApiCallStatusRepository;
        }
        
        public vendor_api_call_status CreateLogRequestDetails(VoucherUpdateRequest voucherUpdateRequest)
        {
            var apiCallStatus = new vendor_api_call_status
            {
                // call_id = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                vendor_id = new[] {Convert.ToInt64(voucherUpdateRequest.registration.Substring(1, voucherUpdateRequest.registration.Length -1))},
                api_called = "voucherRedeem",
                call_datetime = DateTime.Now,
                request = JsonSerializer.Serialize(voucherUpdateRequest)
            };
            
            return apiCallStatus;
        }

        public async Task LogRequestDetails(vendor_api_call_status vendorApiCallStatuses)
        {
            await _vendorApiCallStatusRepository.LogRequestDetails(vendorApiCallStatuses);
        }
    }
}