using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Store
{
    [Table(nameof(ItemMasterStore))]
    public class ItemMasterStore :Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int itemId { get; set; }
        public string itemName { get; set; }
        public int Category {  get; set; }
        public int Quantity { get; set; }

    }
}
