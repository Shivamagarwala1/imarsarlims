using iMARSARLIMS.Model.Master;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Store
{
    [Table(nameof(indentDetail))]
    public class indentDetail : Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int id { get; set; }
        [ForeignKey("Indent")]
        public int indentId { get; set; }
        public int itemId { get; set; }
        public string itemName { get; set; }
        public int Quantity { get; set; }
        public int IssuedQuantity { get; set; }
        public int? ApprovedQuantity { get; set; }

    }
}
