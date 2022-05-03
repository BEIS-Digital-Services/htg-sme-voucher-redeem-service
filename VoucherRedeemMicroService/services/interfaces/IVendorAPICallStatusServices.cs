using Beis.Htg.VendorSme.Database.Models;
using System.Threading.Tasks;
using VoucherUpdateService.domain.entities;

namespace VoucherRedeemService.interfaces
{
    public interface IVendorAPICallStatusServices
    {
        vendor_api_call_status  CreateLogRequestDetails(VoucherUpdateRequest voucherUpdateRequest);
        Task LogRequestDetails(vendor_api_call_status vendorApiCallStatuses);
    }
}