using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(patientReportEmail))]
    public class patientReportEmail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public int? transactionId { get; set; }
        [Required,MaxLength(10)]
        public string? workOrderId { get; set; }
        [Required, MaxLength(100)]
        public string? name { get; set; }
        [Required]
        public DateTime? deliveryDate { get; set; }
        [Required, MaxLength(50)]
        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public string? emailSentTo { get; set; }
        [Required, MaxLength(50)]
        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public string? emailCC { get; set; }
        public byte? isSend { get; set; }
        public byte? isAutoSend { get; set; }
        public int? sentBy { get; set; }
        public DateTime? sendDate { get; set; }
        [MaxLength(50)]
        public string? remarks { get; set; }
        [MaxLength(50)]
        public string? type { get; set; }
        [MaxLength(100)]
        public string? testId { get; set; }
    }
}
