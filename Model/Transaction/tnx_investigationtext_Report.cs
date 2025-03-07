using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Transaction
{
    [Table(nameof(tnx_investigationtext_Report))]
    public class tnx_investigationtext_Report
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        
        public int testId { get; set; }
        public string value { get; set; }
        public int createdbyId { get; set; }
        public DateTime createdate { get; set; }
    }
}
