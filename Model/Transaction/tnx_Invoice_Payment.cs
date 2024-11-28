using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Transaction
{
    [Table(nameof(tnx_Invoice_Payment))]
    public class tnx_Invoice_Payment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string? invoiceNo { get; set; }
        public int? centreId { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public int? createdBy { get; set; }
        public DateTime? createdOn { get; set; }
        public DateTime? createdDate { get; set; }
        public string? cancelReason { get; set; }
        public int? cancelById { get; set; }
        public string? cancelByName { get; set; }
        public byte? isCancel { get; set; }
    }
}
