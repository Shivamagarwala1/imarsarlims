using System.ComponentModel.DataAnnotations;

namespace iMARSARLIMS.Request_Model
{
    public class ResultEntryRequestModle
    {
        public int testId { get; set; }
        [Required, MaxLength(6)]
        public string gender { get; set; }
        public int fromAge { get; set; }
        public int toAge { get; set; }
        public int centreId { get; set; }
    }
}
