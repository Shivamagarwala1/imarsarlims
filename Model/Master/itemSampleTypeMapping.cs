using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(itemSampleTypeMapping))]
    public class itemSampleTypeMapping :Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int sampleTypeId { get; set; }
        [Required, MaxLength(100)]
        public string? sampleTypeName { get; set; }
        public byte? isDefault { get; set; }
    }
}
