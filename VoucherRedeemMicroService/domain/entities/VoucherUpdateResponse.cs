namespace VoucherUpdateService.domain.entities
{
    public class VoucherUpdateResponse
    {
    public string status { get; set; }
    public int errorCode { get; set; }
    public string message { get; set; }
    public string voucherCode { get; set; }
    }
}