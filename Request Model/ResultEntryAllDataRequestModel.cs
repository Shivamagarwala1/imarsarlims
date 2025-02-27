using System.ComponentModel.DataAnnotations;

namespace iMARSARLIMS.Request_Model
{
    public class ResultEntryAllDataRequestModel
    {
       public List<int> centreIds { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Datetype { get; set; }
        public string status { get; set; }
        public string orderby { get; set; }
        public string searchvalue { get; set; }
        public List<int> ItemIds { get; set; }
        public List<int> departmentIds { get; set; }
        public int empid { get; set; }
        public int reporttype { get; set; }
    }
}
