namespace iMARSARLIMS.Request_Model
{
    public class NablRequestModel
    {
        public List<int> observationId { get; set; }
        public int itemid { get; set; }
        public int centreId { get; set; }
        public string? NablLogo { get; set; }
        public int IsDefaultLogo { get; set; }
        public byte isactive { get; set; }
        public int createdById { get; set; }

    }
}
