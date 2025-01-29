using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(organismAntibioticMaster))]
    public class organismAntibioticMaster: Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short id { get; set; }
        [MaxLength(50)]
        public string organismAntibiotic { get; set; }
        [MaxLength(50)]
        public string? machineCode { get; set; }
        public byte? microType { get; set; }
    }
}
