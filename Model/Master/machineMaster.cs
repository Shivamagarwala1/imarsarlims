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
        public int centreId { get; set; }
        public int referRange {  get; set; }
        public string comPort { get; set; }
        public int boundRate { get; set; }
        public int dataBit {  get; set; }
        public int stopBit { get; set; }
        public string parity { get; set; }
        public int machinePortNo { get; set; }
        public string machineIP {  get; set; }
    }
}
