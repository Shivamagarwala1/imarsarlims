using iMARSARLIMS.Model.Transaction;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Store
{
    [Table(nameof(Indent))]
    public class Indent:Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int indentId { get; set; }
        public string indentBy { get; set; }
        public int indentById {  get; set; }
        public int indentStatus { get; set; }
        
        public int isrejected { get; set; }
        public int rejectedBy { get; set; }
        public DateTime RejectDatetime { get; set; }
        [ForeignKey(nameof(indentId))]
        public List<indentDetail>? addIndentDetail { get; set; }

    }
}
