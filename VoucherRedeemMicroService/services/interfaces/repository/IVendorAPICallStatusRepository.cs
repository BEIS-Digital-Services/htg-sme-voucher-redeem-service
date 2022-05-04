using System.Threading.Tasks;
using Beis.Htg.VendorSme.Database.Models;

namespace Beis.HelpToGrow.Voucher.Api.Redeem.Services.Interfaces
{
    public interface IVendorAPICallStatusRepository
    {
        Task  LogRequestDetails(vendor_api_call_status vendorApiCallStatuses);
    }
}