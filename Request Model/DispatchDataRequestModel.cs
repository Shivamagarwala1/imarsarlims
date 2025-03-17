namespace iMARSARLIMS.Request_Model
{
    public class DispatchDataRequestModel
    {
        public int centreId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Datetype { get; set; }
        public string searchvalue { get; set; }
        public List<int> ItemIds { get; set; }
        public List<int> departmentIds { get; set; }
        public int empid { get; set; }
        public string? status { get; set; }
    }
}
