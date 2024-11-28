using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(districtMaster))]
    public class districtMaster : Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required,MaxLength(30)] 
        public string district { get; set; }
        [Required]
        public int stateId { get; set; }
    }
}
