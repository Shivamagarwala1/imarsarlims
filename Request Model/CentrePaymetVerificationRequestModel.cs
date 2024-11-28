namespace iMARSARLIMS.Request_Model
{
    public class CentrePaymetVerificationRequestModel
    {
        public int id { get; set; }
        public string? remarks { get; set; }
        public string? rejectRemarks { get; set; }
        public short approved { get; set; }
        public DateTime? updateDate { get; set; }
        public int? updateByID { get; set; }
        public int? apprvoedByID { get; set; }
    }
}
