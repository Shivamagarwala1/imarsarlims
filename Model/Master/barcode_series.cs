using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(barcode_series))]
    public class barcode_series
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int id { get; set; }
        public int? barcodeNo { get; set; }
        public string? suffix { get; set; }
    }
}
