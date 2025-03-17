using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(ReportEmail))]
    public class ReportEmail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required, MaxLength(10)]
        public string? workOrderId { get; set; }
        [Required, MaxLength(100)]
        public string? name { get; set; }
        [Required, MaxLength(50)]
        public string? emailId { get; set; }
        public byte? isSend { get; set; }
        public byte? isAutoSend { get; set; }
        public int? sentBy { get; set; }
        public DateTime? sendDate { get; set; }
        [MaxLength(100)]
        public string? remarks { get; set; }
        [MaxLength(20)]
        public string? type { get; set; }
        public int Header { get; set; }
        public DateTime? createdDate { get; set; }
    }
}
