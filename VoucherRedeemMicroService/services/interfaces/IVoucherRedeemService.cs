using System.Threading.Tasks;
using VoucherUpdateService.domain.entities;

namespace VoucherRedeemService.interfaces
{
    public interface IVoucherRedeemService
    {
        public Task<VoucherUpdateResponse> GetVoucherResponse(VoucherUpdateRequest voucherRequest);
    }
}