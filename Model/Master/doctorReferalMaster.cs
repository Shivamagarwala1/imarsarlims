using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(doctorReferalMaster))]
    public class doctorReferalMaster :Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int doctorId { get; set; }
        [MaxLength(20)]
        public string? doctorCode { get; set; }
        [Required, MaxLength(10)]
        public string? title { get; set; }
        [Required, MaxLength(100)]
        public string? doctorName { get; set; }
        [MaxLength(20)]
        public string? imaRegistartionNo { get; set; }
        [MaxLength(50)]
        public string? email { get; set; }
        [MaxLength(50)]
        public string? reportEmail { get; set; }
        [MaxLength(10)]
        public string? mobileNo { get; set; }
        [MaxLength(10)]
        public string? mobileno2 { get; set; }
        [MaxLength(100)]
        public string? address1 { get; set; }
        [MaxLength(100)]
        public string? address2 { get; set; }
        public int? pinCode { get; set; }
        public int? degreeId { get; set; }
        [MaxLength(50)]
        public string? degreeName { get; set; }
        public int? specializationID { get; set; }
        [MaxLength(50)]
        public string? specialization { get; set; }
        public DateTime? dob { get; set; }
        public DateTime? anniversary { get; set; }
        public byte? allowsharing { get; set; }
        public byte? referMasterShare { get; set; }
        public int? proId { get; set; }
        public int? areaId { get; set; }
        public int? city { get; set; }
        public int? state { get; set; }
        public int type { get; set; }
        public int centreID { get; set; }
        [MaxLength(15)]
        public string UserId { get; set; }
        [MaxLength(15)]
        public string password { get; set; }
        [MaxLength(50)]
        public string EmailReport { get; set; }
        public byte OPDFee { get; set; } = 0;
        public byte Discount { get; set; } = 0;
        public byte OnlineLogin { get; set; } = 0;
        public byte AllowOPD { get; set; } = 0;
    }
}
