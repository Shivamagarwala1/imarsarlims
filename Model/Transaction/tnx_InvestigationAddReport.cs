using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Transaction
{
    [Table(nameof(tnx_InvestigationAddReport))]
    public class tnx_InvestigationAddReport :Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int testId { get; set; }
        [MaxLength(300)]
        public string Attachment { get; set; }
        

    }
}
