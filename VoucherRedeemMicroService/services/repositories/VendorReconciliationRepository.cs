using Beis.Htg.VendorSme.Database;
using Beis.Htg.VendorSme.Database.Models;
using VoucherCheckService.services.interfaces;

namespace VoucherCheckService.services.repositories
{
    public class VendorReconciliationRepository: IVendorReconciliationRepository
    {
        private readonly HtgVendorSmeDbContext _context;

        public VendorReconciliationRepository(HtgVendorSmeDbContext context)
        {
            _context = context;
        }

        public void AddVendorReconciliation(vendor_reconciliation vendorReconciliation)
        {
            _context.vendor_reconciliations.Add(vendorReconciliation);
            _context.SaveChanges();
        }
    }
}