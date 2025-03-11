using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(InvestigationMasterUD))]
    public class InvestigationMasterUD :Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int investigationId { get; set; }
        [MaxLength(100)]
        public string investigationName { get; set; }
        [MaxLength(300)]
        public string formatUrl { get; set; }
    }
}
