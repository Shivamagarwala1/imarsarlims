namespace iMARSARLIMS.Request_Model
{
    public class SampltypeChangeRequestModel
    {
        public int testid { get; set; }
        public int sampletypeId { get; set; }
        public string Sampletypename { get; set; }
        public int reporttype { get; set; }
        public int empid { get; set; }
        public DateTime ChangeDate { get; set; }
    }
}
