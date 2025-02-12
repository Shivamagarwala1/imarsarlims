namespace iMARSARLIMS.Request_Model
{
    public class ChangePaymentMode
    {
        public int id { get; set; }
        public int transactionId { get; set; }
        public string transactionType { get; set; }
        public string workOrderId { get; set; }
        public int receiptNo { get; set; }
        public double receivedAmt { get; set; }
        public double cashAmt { get; set; }
        public double creditCardAmt { get; set; }
        public string creditCardNo { get; set; }
        public double chequeAmt { get; set; }
        public string chequeNo { get; set; }
        public double onlinewalletAmt { get; set; }
        public string walletno { get; set; }
        public double NEFTamt { get; set; }
        public string BankName { get; set; }
        public short paymentModeId { get; set; }
        public byte? isCancel { get; set; }
        public DateTime? cancelDate { get; set; }
        public string? canceledBy { get; set; }
        public string? cancelReason { get; set; }
        public int bookingCentreId { get; set; }
        public int settlementCentreID { get; set; }
        public string receivedBy { get; set; }
        public int receivedID { get; set; }
    }
}
