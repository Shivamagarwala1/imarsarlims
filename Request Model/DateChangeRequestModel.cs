namespace iMARSARLIMS.Request_Model
{
    public class DateChangeRequestModel
    {
        public int testId { get; set; }
        public DateTime SamplecollectedDate { get; set; }
        public DateTime SamplerecievedDate { get; set; }
        public DateTime Resultdate {get; set;}
        public DateTime ApproveDate { get; set; }
        public int userId { get; set; }

    }
}
