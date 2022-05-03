using System.Threading.Tasks;
using Beis.Htg.VendorSme.Database.Models;

namespace VoucherCheckService.services.interfaces
{
    public interface IVendorCompanyRepository
    {
        vendor_company GetVendorCompanyByRegistration(string registrationId);
    }
}