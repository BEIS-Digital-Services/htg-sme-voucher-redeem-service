using System.Threading.Tasks;
using Beis.Htg.VendorSme.Database.Models;

namespace VoucherCheckService.services.interfaces
{
    public interface IVendorReconciliationSalesRepository
    {
        Task AddVendorReconciliationSales(vendor_reconciliation_sale vendorReconciliationSales, token token);
        Task<vendor_reconciliation_sale> GetVendorReconciliationSalesByVoucherCode(string voucherCode);
    }
}