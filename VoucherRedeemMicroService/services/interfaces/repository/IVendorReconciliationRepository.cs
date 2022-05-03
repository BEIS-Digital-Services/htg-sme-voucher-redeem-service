using Beis.Htg.VendorSme.Database.Models;

namespace VoucherCheckService.services.interfaces
{
    public interface IVendorReconciliationRepository
    {
        void AddVendorReconciliation(vendor_reconciliation vendorReconciliation);
    }
}