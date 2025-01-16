using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(itemMaster))]
    public class itemMaster :Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int itemId { get; set; }
        [Required, MaxLength(100)]
        public string? itemName { get; set; }
        [Required, MaxLength(100)]
        public string? dispalyName { get; set; }
        [Required, MaxLength(100)]
        public string? testMethod { get; set; }
        public int? deptId { get; set; }
        [Required, MaxLength(20)]
        public string? code { get; set; }
        [Required, MaxLength(20)]
        public string? sortName { get; set; }
        public byte allowDiscont { get; set; }
        public byte? allowShare { get; set; }
        public byte? allowReporting { get; set; }
        public int itemType { get; set; }
        public byte? isOutsource { get; set; }
        public byte? lmpRequire { get; set; }
        public int? reportType { get; set; }
        public string? gender { get; set; }
        [Required, MaxLength(20)]
        public string? sampleVolume { get; set; }
        [Required, MaxLength(20)]
        public string? containerColor { get; set; }
        [MaxLength(50)]
        public string? testRemarks { get; set; }
        [Required]
        public int? defaultsampletype { get; set; }
        public int? agegroup { get; set; }
        public string? samplelogisticstemp { get; set; }
        public byte? printsamplename { get; set; }
        public byte? showinpatientreport { get; set; }
        public byte? showinonlinereport { get; set; }
        public byte? autosaveautoapprove { get; set; }
        public byte? printseperate { get; set; }
        public byte? isorganism { get; set; }
        public byte? culturereport { get; set; }
        public byte? ismic { get; set; }
        public byte? showOnWebsite { get; set; }
        public byte? isSpecialItem { get; set; }
        public byte? isAllergyTest { get; set; }
        public int? displaySequence { get; set; }
        [MaxLength(150)]
        public string? consentForm { get; set; }
        [ForeignKey(nameof(itemId))]
        public List<itemSampleTypeMapping>? AddSampletype { get; set; }
    }
}
