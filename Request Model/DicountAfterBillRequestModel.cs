namespace iMARSARLIMS.Request_Model
{
    public class DicountAfterBillRequestModel
    {
        public string workOrderId { get; set; }
        public int transactionId { get; set; }
        public int discounttype { get; set; }
        public double discountAmt { get; set; }
        public int discountReason { get; set; }
        public int discountApprovedBy { get; set; }
        public int userId { get; set; }
        public DateTime DicountDateTime { get; set; }

    }
}
