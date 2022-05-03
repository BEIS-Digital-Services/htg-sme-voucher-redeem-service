using System.Threading.Tasks;
using Beis.Htg.VendorSme.Database.Models;

namespace VoucherCheckService.services.interfaces
{
    public interface IEnterpriseRepository
    {
        Task<enterprise> GetEnterprise(long enterpriseId);
    }
}