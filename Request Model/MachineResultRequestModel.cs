namespace iMARSARLIMS.Request_Model
{
    public class MachineResultRequestModel
    {
        public int centreId {  get; set; }
        public int MachineId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string? BarcodeNo { get; set; }
        public int empId { get; set; }
    }
}
