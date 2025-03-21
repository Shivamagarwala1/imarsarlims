using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(doctorShareMaster))]
    public class doctorShareMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int Doctorid { get; set; }
        public int deptid { get; set; }
        public int ItemID { get; set; }
        public int Centreid { get; set; }
        public double percentage { get; set; }
        public double Amount { get; set; }
        public byte type { get; set; }
        public int CreatedBYID { get; set; }
        public string createdbyName { get; set; }
        public DateTime createdDate { get; set; }
    }
}
