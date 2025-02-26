namespace iMARSARLIMS.Request_Model
{
    public class SampleProcessingRequestModel
    {
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public int? centreId { get; set; }
        public string?  barcodeNo { get; set; }
        public string? Status { get; set; }
        public int? empId { get; set; }

    }
}
