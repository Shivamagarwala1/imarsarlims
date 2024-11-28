using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(sms))]
    public class sms
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public int transactionId { get; set; }
        [Required, MaxLength(10)]
        public string workOrderId { get; set; }
        [Required, MaxLength(250)]
        public string smsText { get; set; }
        [Required, MaxLength(100)]
        public string name { get; set; }
        public DateTime? deliveryDate { get; set; }
        [Required, MaxLength(10)]
        public string mobileNo { get; set; }
        public byte? isSend { get; set; }
        public byte? isAutoSend { get; set; }
        public int? sentBy { get; set; }
        public DateTime? sendDate { get; set; }
        [MaxLength(100)]
        public string? remarks { get; set; }
        [MaxLength(20)]
        public string? type { get; set; }
        public DateTime? createdDate { get; set; }
        [MaxLength(20)]
        public string? templateId { get; set; }
    }
}
