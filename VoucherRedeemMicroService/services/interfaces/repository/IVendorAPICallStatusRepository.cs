using System.Threading.Tasks;
using Beis.Htg.VendorSme.Database.Models;

namespace VoucherCheckService.services.interfaces
{
    public interface IVendorAPICallStatusRepository
    {
        Task  LogRequestDetails(vendor_api_call_status vendorApiCallStatuses);
    }
}