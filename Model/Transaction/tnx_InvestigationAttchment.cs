using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Transaction
{
    [Table(nameof(tnx_InvestigationAttchment))]
    public class tnx_InvestigationAttchment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int testId { get; set; }
        public string Attachment {  get; set; }
        public int isactive { get; set; }
        public int createdById { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
