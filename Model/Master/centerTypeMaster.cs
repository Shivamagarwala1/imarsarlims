using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(centerTypeMaster))]
    public class centerTypeMaster : Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int id { get; set; }
        [Required, MaxLength(50)]
        public string centerTypeName { get; set; }
    }
}
