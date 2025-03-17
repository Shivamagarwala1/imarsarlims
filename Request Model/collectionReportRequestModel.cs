namespace iMARSARLIMS.Request_Model
{
    public class collectionReportRequestModel
    {
        public List<int> empIds {  get; set; }
        public List<int> centreIds { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

    }
}
