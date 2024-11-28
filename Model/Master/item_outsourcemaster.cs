using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(item_outsourcemaster))]
    public class item_outsourcemaster :Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int? bookingCentreId { get; set; }
        public int? processingCentreId { get; set; }
        public int? itemId { get; set; }
        public int? departmentId { get; set; }
    }
}
