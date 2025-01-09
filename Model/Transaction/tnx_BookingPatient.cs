using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Transaction
{
    [Table(nameof(tnx_BookingPatient))]
    public class tnx_BookingPatient :Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int patientId { get; set; }
        public int title_id { get; set; }
        [Required, MaxLength(100)]
        public string name { get; set; }
        [Required, MaxLength(6)]
        public string? gender { get; set; }
        [Required]
        public int ageTotal { get; set; }
        [Required]
        public int ageDays { get; set; }
        [Required]
        public int? ageMonth { get; set; }
        [Required]
        public short ageYear { get; set; }
        public DateTime? dob { get; set; }
        public byte? isActualDOB { get; set; }
        [MaxLength(50)]
        public string? emailId { get; set; }
        [MaxLength(10)]
        public string? mobileNo { get; set; }
        [MaxLength(100)]
        public string address { get; set; }
        public int pinCode { get; set; }
        public int cityId { get; set; } 
        public int centreId { get; set; }
        public int areaId { get; set; }
        public int stateId { get; set; }
        public int districtId { get; set; }
        public int countryId { get; set; } 
        public int visitCount { get; set; } = 0;
        [MaxLength(100)]
        public string? remarks { get; set; } = "";
        public int? documentId { get; set; } = 0;
        [MaxLength(20)]
        public string? documnetnumber { get; set; }
        [MaxLength(15)]
        public string? password { get; set; }
        [ForeignKey(nameof(patientId))]
        public List<tnx_Booking>? addBooking { get; set; }
        


    }
}
