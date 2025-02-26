namespace iMARSARLIMS.Request_Model
{
    public class patientDataRequestModel
    {
        public List<int> CentreIds { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string? SearchValue { get; set; }
        public int? UserId { get; set; }
        public string? status {  get; set; }
    }
}
