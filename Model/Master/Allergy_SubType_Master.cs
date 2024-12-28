using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(Allergy_SubType_Master))]
    public class Allergy_SubType_Master : Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int? typeId { get; set; }
        [MaxLength(100)]
        public string? sub_TypeName { get; set; }
        [MaxLength(100)]
        public string? firstRange { get; set; }
        [MaxLength(100)]
        public string? secondRange { get; set; }
        [MaxLength(100)]
        public string? thirdRange { get; set; }
        [MaxLength(200)]
        public string? imagePath { get; set; }
        public string? descrition { get; set; }
        [MaxLength(100)]
        public string? defultReading { get; set; }
        [MaxLength(50)]
        public string? unit { get; set; }
    }
}
