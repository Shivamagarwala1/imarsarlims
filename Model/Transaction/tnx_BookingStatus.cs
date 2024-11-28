using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Transaction
{
    [Table(nameof(tnx_BookingStatus))]
    public class tnx_BookingStatus : Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [ForeignKey("tnx_Booking")]
        public int transactionId { get; set; }
        public int patientId { get; set; }
        [MaxLength(12)]
        public string? barcodeNo { get; set; }
        [MaxLength(50)]
        public string? status { get; set; }
        public int centreId { get; set; }
        public int roleId { get; set; }
        [MaxLength(100)]
        public string? remarks { get; set; }
        public int testId { get; set; }

    }
}
