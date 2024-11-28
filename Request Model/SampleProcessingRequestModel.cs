namespace iMARSARLIMS.Request_Model
{
    public class SampleProcessingRequestModel
    {
        public int patientId { get; set; }
        public int transactionId { get; set; }
        public string? name { get; set; }
        public string? SearchDateType { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public int department {  get; set; }
        public int centreId { get; set; }
        public string  barcodeNo { get; set; }
        public int investigationId { get; set; }

    }
}
