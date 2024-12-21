using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(empLoginDetails))]
    public class empLoginDetails
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public int empId { get; set; }
        [Required]
        public int centreId { get; set; }
        [Required, MaxLength(20)]
        public string? ipAddress { get; set; }
        [Required, MaxLength(20)]
        public string? macAddress { get; set; }
        [Required, MaxLength(30)]
        public string? browserName { get; set; }
        [Required, MaxLength(20)]
        public string? hostName { get; set; }
        [Required,MaxLength(20)]
        public string userName { get; set; }
        [Required, MaxLength(50)]
        public string empName { get; set; }
        [Required]
        public int? roleId { get; set; }
        [Required]
        public DateTime logindatetime { get; set; }
        [Required]
        public DateTime? logoutdatetime { get; set; }
        public DateTime? creadteddate { get; set; }
        
    }
}
