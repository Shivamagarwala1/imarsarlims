using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(centreWelcomeEmail))]
    public class centreWelcomeEmail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public int? centreId { get; set; }
        [Required, MaxLength(20)]
        public string? centreCode { get; set; }
        [Required, MaxLength(50)]
        public string? emailId { get; set; }
        [Required]
        public string? emailBody { get; set; }
        [Required, MaxLength(50)]
        public string? subject { get; set; }
        public byte? isSent { get; set; }
        public DateTime? sendDate { get; set; }
        [Required]
        public int? createdByID { get; set; }
        [MaxLength(500)]
        public string? errorMsg { get; set; }
    }
}
