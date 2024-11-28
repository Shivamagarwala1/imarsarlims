using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(machineMaster))]
    public class machineMaster :Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public short id { get; set; }
        [Required,MaxLength(30)]
        public string machineName { get; set; }
        [MaxLength(30)]
        public string? machineType { get; set; }
    }
}
