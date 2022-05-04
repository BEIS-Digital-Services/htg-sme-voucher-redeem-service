using System.Threading.Tasks;
using Beis.Htg.VendorSme.Database.Models;

namespace Beis.HelpToGrow.Voucher.Api.Redeem.Services.Interfaces
{
    public interface ITokenRepository
    {
        Task AddToken(token token);
        token GetToken(string tokenCode);
        void UpdateToken(token token);
        
    }
}