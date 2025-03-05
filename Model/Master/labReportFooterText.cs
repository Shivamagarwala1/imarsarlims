using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(labReportFooterText))]
    public class labReportFooterText: Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int centreId { get; set; }
        [MaxLength(300)]
        public string footerText { get; set; }
    }
}
