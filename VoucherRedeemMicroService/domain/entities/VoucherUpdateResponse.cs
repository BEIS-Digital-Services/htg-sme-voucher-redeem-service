namespace Beis.HelpToGrow.Voucher.Api.Redeem.Domain.Entities
{
    public class VoucherUpdateResponse
    {
    public string status { get; set; }
    public int errorCode { get; set; }
    public string message { get; set; }
    public string voucherCode { get; set; }
    }
}