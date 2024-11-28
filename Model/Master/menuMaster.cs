using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(menuMaster))]
    public class menuMaster: Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string? menuName { get; set; }
        public string? dispalyName { get; set; }
        public string? navigationUrl { get; set; }
        public int? displaySequence { get; set; }
        public int? parentId { get; set; }
        public long iconId { get; set; }
    }
}
