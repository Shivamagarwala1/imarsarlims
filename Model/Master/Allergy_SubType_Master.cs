using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(Allergy_SubType_Master))]
    public class Allergy_SubType_Master : Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int? typeId { get; set; }
        public string? sub_TypeName { get; set; }
        public string? firstRange { get; set; }
        public string? secondRange { get; set; }
        public string? thirdRange { get; set; }
        public string? imagePath { get; set; }
        public string? descrition { get; set; }
        public string? defultReading { get; set; }
        public string? unit { get; set; }
    }
}
