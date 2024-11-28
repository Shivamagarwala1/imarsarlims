using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(centreCheckList))]
    public class centreCheckList: Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public int id { get; set; }
        [Required]
        public int? companyID { get; set; }
        [Required]
        public int? checklistID { get; set; }
        [Required,MaxLength(100)]
        public string? name { get; set; }
    }
}
