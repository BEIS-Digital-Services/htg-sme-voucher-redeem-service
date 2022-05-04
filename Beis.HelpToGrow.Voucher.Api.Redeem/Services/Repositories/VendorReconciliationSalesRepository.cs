using System.Threading.Tasks;
using Beis.Htg.VendorSme.Database;
using Beis.Htg.VendorSme.Database.Models;
using Microsoft.EntityFrameworkCore;
using Beis.HelpToGrow.Voucher.Api.Redeem.Services.Interfaces;

namespace Beis.HelpToGrow.Voucher.Api.Redeem.Services.Repositories
{
    public class VendorReconciliationSalesRepository: IVendorReconciliationSalesRepository
    {
        private readonly HtgVendorSmeDbContext _context;
        public VendorReconciliationSalesRepository(HtgVendorSmeDbContext context)
        {
            _context = context;
        }

        public async Task AddVendorReconciliationSales(vendor_reconciliation_sale vendorReconciliationSales, token token)
        {
             await _context.vendor_reconciliation_sales.AddAsync(vendorReconciliationSales);
              _context.tokens.Update(token);
             await _context.SaveChangesAsync();
        }

        public async Task<vendor_reconciliation_sale> GetVendorReconciliationSalesByVoucherCode(string voucherCode)
        {
            return await _context.vendor_reconciliation_sales.FirstOrDefaultAsync(t => t.token_code == voucherCode);
        }
    }
}