using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(patientDemographicLog))]
    public class patientDemographicLog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public int patientId { get; set; }
        [Required, MaxLength(10)]
        public string? title { get; set; }
        [Required, MaxLength(100)]
        public string? patientName { get; set; }
        [Required, MaxLength(10)]
        public string gender { get; set; }
        [Required]
        public int ageTotal { get; set; }
        [Required]
        public int ageDays { get; set; }
        [Required]
        public int ageMonth { get; set; }
        [Required]
        public int ageYear { get; set; }
        [Required]
        public DateTime dob { get; set; }
        [Required]
        public string mobileNo { get; set; }
        [Required, MaxLength(100)]
        public string address { get; set; }
        [Required]
        public int transactionId { get; set; }
        [Required]
        public int refID1 { get; set; }
        [Required, MaxLength(100)]
        public string RefDoctor1 { get; set; }
        public int refID2 { get; set; }
        public string refDoctor2 { get; set; }
        [Required]
        public int? centreId { get; set; }
        public DateTime? updateDate { get; set; }
        public int? updateById { get; set; }
    }
}
