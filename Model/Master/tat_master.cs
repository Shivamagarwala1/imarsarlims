using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(tat_master))]
    public class tat_master
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int centreid { get; set; }
        public int Deptid { get; set; }
        public int itemid { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int? Mins { get; set; }
        public int? Days { get; set; }
        public int? Sun { get; set; }
        public int? Mon { get; set; }
        public int? Tue { get; set; }
        public int? Wed { get; set; }
        public int? Thu { get; set; }
        public int? Fri { get; set; }
        public int? Sat { get; set; }
        public int? Regcoll { get; set; }
        public int? collrecv { get; set; }
        public DateTime CreatedOn { get; set; }
        public int Createdby { get; set; }
        public string? CreatedByName { get; set; }
        public string? TATType { get; set; }
    }
}
