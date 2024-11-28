namespace iMARSARLIMS.Request_Model
{
    public class BatchStatusRecieveRequestModel
    {
        public int testId { get; set; }
        public int transactionId { get; set; }
        public int investigationId { get; set; }
        public int fromCentreId { get; set; }
        public string barcodeNo { get; set; }
        public int toCentreId { get; set; }
        public string status { get; set; }
        public string batchNumber { get; set; }
        public string? remarks { get; set; }
        public int? receivedBYId { get; set; }
        public DateTime? receivedOn { get; set; }
    }
}
