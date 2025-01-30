using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(item_OutHouseMaster))]
    public class item_OutHouseMaster :Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int bookingCentreId { get; set; }
        public int ProcessingLabId { get; set; }
        public int itemId { get; set; }
        public int departmentId { get; set; }
    }
}
