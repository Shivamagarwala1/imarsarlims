using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(changeCentreLog))]
    public class changeCentreLog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public string? workOrderId { get; set; }
        [Required]
        public int? oldBookingCentre { get; set; }
        [Required]
        public int? newBookingCentre { get; set; }
        [Required]
        public int? oldCompanyId { get; set; }
        [Required]
        public int? newCompanyId { get; set; }
        [Required]
        public int? oldRateTypeId { get; set; }
        [Required]
        public int? newRateTypeId { get; set; }
        [Required]
        public DateTime dtEntry { get; set; }
        public int? enteredBy { get; set; }
    }
}
