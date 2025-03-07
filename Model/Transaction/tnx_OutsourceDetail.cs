using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Transaction
{
    [Table(nameof(tnx_OutsourceDetail))]
    public class tnx_OutsourceDetail:Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int testId { get; set; }
        public int? itemId { get; set; }
        public string? itemName { get; set; }
        public int? centreId { get; set; }
        public int? transactionId { get; set; }
        public string? workOrderId { get; set; }
        public double? bookingRate { get; set; }
        public double? outSourceRate { get; set; }
        public int? outSourceLabID { get; set; }
        public string? outSourceLabName { get; set; }
        public string? remarks { get; set; }

    }
}
