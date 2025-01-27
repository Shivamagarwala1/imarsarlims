using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(itemTemplate))]
    public class itemTemplate : Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int itemId { get; set; }
        public string Template { get; set; }
        public int CentreId { get; set; }
        [MaxLength(20)]
        public string gender { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
