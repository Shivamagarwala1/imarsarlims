using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(labReportHeader))]
    public class labReportHeader:Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Label { get; set; }
        [MaxLength(50)]
        public string Fname { get; set; }
        [MaxLength(50)]
        public int Fsize { get; set; }
        [MaxLength(5)]
        public int Bold { get; set; }
        [MaxLength(5)]
        public int Italic { get; set; }
        [MaxLength(5)]
        public int Under { get; set; }
        [MaxLength(8)]
        public string Alignment { get; set; }
        public int Print { get; set; }
        [MaxLength(20)]
        public string P_forecolor { get; set; }
     
        public int Width { get; set; }
        public int Height { get; set; }
        public int printOrder { get; set; }

    }
}
