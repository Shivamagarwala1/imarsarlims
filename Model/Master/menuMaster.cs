using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(menuMaster))]
    public class menuMaster: Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [MaxLength(150)]
        public string? menuName { get; set; }
       
        [MaxLength(200)]
        public string? navigationUrl { get; set; }
        public int? displaySequence { get; set; }
        public int? parentId { get; set; }
        public bool isHide { get; set; }
        public int MenuType { get; set; }
    }
}
