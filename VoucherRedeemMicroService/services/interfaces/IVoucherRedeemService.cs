using System.Threading.Tasks;
using Beis.HelpToGrow.Voucher.Api.Redeem.Domain.Entities;

namespace Beis.HelpToGrow.Voucher.Api.Redeem.Interfaces
{
    public interface IVoucherRedeemService
    {
        public Task<VoucherUpdateResponse> GetVoucherResponse(VoucherUpdateRequest voucherRequest);
    }
}