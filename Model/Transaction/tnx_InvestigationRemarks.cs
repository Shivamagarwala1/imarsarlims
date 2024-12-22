using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Transaction
{
    [Table(nameof(tnx_InvestigationRemarks))]
    public class tnx_InvestigationRemarks : Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string? invRemarks { get; set; }
        public int? transactionId { get; set; }
        public int? customerID { get; set; }
        public int? itemId { get; set; }
        public string? itemName { get; set; }
        public byte? isActive { get; set; }
        public byte? isInternal { get; set; }
    }
}
