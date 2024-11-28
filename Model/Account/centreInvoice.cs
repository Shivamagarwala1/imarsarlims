using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Account
{
    [Table(nameof(centreInvoice))]
    public class centreInvoice
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [MaxLength(20)]
        public string? invoiceNo { get; set; }
        public int centreid { get; set; }
        public double rate { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public int createdBy { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime? createDate { get; set; }
        [MaxLength(150)]
        public string? cancelReason { get; set; }
        public int? cancelByID { get; set; }
        public byte? isCancel { get; set; }
        public DateTime? cancelDate { get; set; }
    }
}
