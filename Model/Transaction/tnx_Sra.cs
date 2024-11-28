using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace iMARSARLIMS.Model.Transaction
{
    [Table(nameof(tnx_Sra))]
    public class tnx_Sra 
    {
        public int id { get; set; }
        public int testId { get; set; }
        public int transactionId { get; set; }
        public int investigationId { get; set; }
        public int fromCentreId { get; set; }
        public int toCentreId { get; set; }
        public string status { get; set; }
        public int entryById { get; set; }
        public DateTime entryDatetime { get; set; }
        public string batchNumber { get; set; }
        public DateTime? transferredOn { get; set; }
        public string barcodeNo { get; set; }
        public int transferByID { get; set; }
        public string? remarks { get; set; }
        public int? receivedBYId { get; set; }
        public DateTime? receivedOn { get; set; }
        public double? outsourceRate { get; set; }
        public string? invoiceNo { get; set; }
        public DateTime? invoiceDate { get; set; }
        public int? invoicecreatedID { get; set; }
        public double? invoiceAmount { get; set; }
    }
}
