using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Transaction
{
    [Table(nameof(tnx_InvestigationAddReport))]
    public class tnx_InvestigationAddReport
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int testId { get; set; }
        [MaxLength(300)]
        public string Attachment { get; set; }
        public int isactive { get; set; }
        public int createdById { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
