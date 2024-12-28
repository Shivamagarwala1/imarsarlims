using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(barcode_series))]
    public class barcode_series
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int id { get; set; }
        public int? barcodeNo { get; set; }
        [MaxLength(2)]
        public string? suffix { get; set; }
    }
}
