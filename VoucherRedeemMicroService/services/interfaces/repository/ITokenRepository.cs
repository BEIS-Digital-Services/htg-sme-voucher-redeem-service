using System.Threading.Tasks;
using Beis.Htg.VendorSme.Database.Models;

namespace VoucherCheckService.services.interfaces
{
    public interface ITokenRepository
    {
        Task AddToken(token token);
        token GetToken(string tokenCode);
        void UpdateToken(token token);
        
    }
}