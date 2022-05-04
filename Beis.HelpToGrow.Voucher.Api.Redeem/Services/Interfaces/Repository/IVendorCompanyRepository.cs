using System.Threading.Tasks;
using Beis.Htg.VendorSme.Database.Models;

namespace Beis.HelpToGrow.Voucher.Api.Redeem.Services.Interfaces
{
    public interface IVendorCompanyRepository
    {
        vendor_company GetVendorCompanyByRegistration(string registrationId);
    }
}