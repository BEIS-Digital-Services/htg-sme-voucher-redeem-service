using System.Threading.Tasks;
using Beis.Htg.VendorSme.Database.Models;

namespace Beis.HelpToGrow.Voucher.Api.Redeem.Services.Interfaces
{
    public interface IVendorReconciliationSalesRepository
    {
        Task AddVendorReconciliationSales(vendor_reconciliation_sale vendorReconciliationSales, token token);
        Task<vendor_reconciliation_sale> GetVendorReconciliationSalesByVoucherCode(string voucherCode);
    }
}