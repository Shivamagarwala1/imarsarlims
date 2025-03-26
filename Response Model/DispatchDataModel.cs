namespace iMARSARLIMS.Response_Model
{
    public class DispatchDataModel
    {
        public string bookingDate { get; set; }
        public string? workOrderId { get; set; }
        public string? SampleRecievedDate { get; set; }
        public string? ApproveDate { get; set; }
        public string PatientName { get; set; }
        public string Age { get; set; }
        public string barcodeNo { get; set; }
        public string mobileNo { get; set; }
        public string investigationName { get; set; }
        public string Remark { get; set; }
        public string centrecode { get; set; }
        public string centreName { get; set; }
        public string departmentName { get; set; }
        public string isSampleCollected { get; set; }
        public int transactionId { get; set; }
        public DateTime? sampleCollectionDate { get; set; }
        public int reportType { get; set; }
        public string Comment { get; set; }
        public byte? resultdone { get; set; }
        public byte? Approved { get; set; }
        public DateTime? approvedDateFilter { get; set; }
        public DateTime? sampleReceiveDateFilter { get; set; }
        public int centreId { get; set; }
        public int itemId { get; set; }
        public int deptId { get; set; }
        public string ReferDoctor { get; set; }
        public string CreatedBy { get; set; }
        public int testId { get; set; }
        public byte Email { get; set; }
        public int? Whatsapp { get; set; }
        public int isremark { get; set; }
        public DateTime? BokkingDateFilter { get; set; }
        public string status { get; set; }
    }
}
