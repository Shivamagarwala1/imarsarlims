using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Store
{
    [Table(nameof(indentIssueDetail))]
    public class indentIssueDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int indentId { get; set; }
        public int itemId { get; set; }
        public int requestedQuantity { get; set; }
        public int IssuedQuantity { get; set; }
        public int IssueById { get; set; }
        public DateTime IssueDateTime { get; set; }

    }
}
