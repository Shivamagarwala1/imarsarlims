namespace iMARSARLIMS.Request_Model
{
    public class WorkSheetRequestModel
    {
      public int CentreId {  get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int DeptId { get; set; }
        public int ItemId { get; set; }
        public string? BarcodeNo { get; set; }
        public string? Status { get; set; }
        public int empid {  get; set; }
    }
}
