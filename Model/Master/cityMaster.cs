using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(cityMaster))]
    public class cityMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required,MaxLength(50)]
        public string cityName { get; set; }
        [Required]
        public int stateID { get; set; }
    }
}
