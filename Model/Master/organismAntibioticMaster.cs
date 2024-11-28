using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(organismAntibioticMaster))]
    public class organismAntibioticMaster: Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short id { get; set; }
        public string organismAntibiotic { get; set; }
        public string? machineCode { get; set; }
        public byte? microType { get; set; }
    }
}
