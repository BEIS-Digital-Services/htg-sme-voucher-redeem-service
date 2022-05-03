using System.Linq;
using System.Threading.Tasks;
using Beis.Htg.VendorSme.Database;
using Beis.Htg.VendorSme.Database.Models;
using VoucherCheckService.services.interfaces;

namespace VoucherCheckService.services.repositories
{
    public class TokenRepository: ITokenRepository
    {
        private readonly HtgVendorSmeDbContext _context;

        public TokenRepository(HtgVendorSmeDbContext context)
        {
            _context = context;
        }
        
        public async Task AddToken(token token)
        {
            await _context.tokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }

        public token GetToken(string tokenCode)
        {
            var token = _context.tokens.SingleOrDefault(t => t.token_code == tokenCode);        
            return token;
        }
        
        public void UpdateToken(token token)
        {
            _context.tokens.Update(token);
            _context.SaveChanges();
        }
    }
}