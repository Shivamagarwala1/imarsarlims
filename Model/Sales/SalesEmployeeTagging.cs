using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Sales
{
    [Table(nameof(SalesEmployeeTagging))]
    public class SalesEmployeeTagging
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int SalesEmployeeId { get; set; }
        public int TaggedToId { get; set; }
        public int TaggedById { get; set; }
        public DateTime TaggedDate { get; set; }

    }
}
