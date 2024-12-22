using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(area_master))]
    public class area_master : Audit

    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int id { get; set; }
        [Required, MaxLength(50)]
        public string? areaName { get; set; }
        [Required]
        public int cityId { get; set; }

    }
}
