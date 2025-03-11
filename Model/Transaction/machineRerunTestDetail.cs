using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Transaction
{
    [Table(nameof(machineRerunTestDetail))]
    public class machineRerunTestDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int observationId { get; set; }
        public int testID { get; set; }
        [MaxLength(15)]
        public string workorderid { get; set; }
        [MaxLength(15)]
        public string MacReading { get; set; }
        public byte MacID { get; set; }
        [MaxLength(100)]
        public string LabObservationName { get; set; }
        [MaxLength(100)]
        public string InvestigationName { get; set; }
        [MaxLength(100)]
        public string RerunReason { get; set; }
        public int Rerunbyid { get; set; }
        public DateTime RerunDate { get; set; }
    }
}
