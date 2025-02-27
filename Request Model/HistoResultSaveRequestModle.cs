namespace iMARSARLIMS.Request_Model
{
    public class HistoResultSaveRequestModle
    {
        public int histoObservationId { get; set; }
        public int transactionId { get; set; }
        public int patientId { get; set; }
        public string? barcodeNo { get; set; }
        public int? testId { get; set; }
        public string? clinicalHistory { get; set; }
        public string? specimen { get; set; }
        public string? gross { get; set; }
        public string? typesFixativeUsed { get; set; }
        public string? blockKeys { get; set; }
        public string? stainsPerformed { get; set; }
        public string? biospyNumber { get; set; }
        public string? microscopy { get; set; }
        public string? finalImpression { get; set; }
        public string? comment { get; set; }
        public DateTime dtEntry { get; set; }
        public byte isResultDone { get; set; }
        public byte isApproved { get; set; }
        public string createdBy { get; set; }
        public int createdById { get; set; }
        public DateTime createdDate { get; set; }
        public int? approvaldoctorId { get; set; }

    }
}
