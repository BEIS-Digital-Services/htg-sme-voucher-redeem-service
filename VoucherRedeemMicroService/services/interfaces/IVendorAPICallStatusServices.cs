using Beis.Htg.VendorSme.Database.Models;
using System.Threading.Tasks;
using Beis.HelpToGrow.Voucher.Api.Redeem.Domain.Entities;

namespace Beis.HelpToGrow.Voucher.Api.Redeem.Interfaces
{
    public interface IVendorAPICallStatusServices
    {
        vendor_api_call_status  CreateLogRequestDetails(VoucherUpdateRequest voucherUpdateRequest);
        Task LogRequestDetails(vendor_api_call_status vendorApiCallStatuses);
    }
}