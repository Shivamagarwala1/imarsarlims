using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Transaction
{
    [Table(nameof(tnx_outhousedeatils))]
    public class tnx_outhousedeatils : Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int? itemId { get; set; }
        public string? itemName { get; set; }
        public int? centreId { get; set; }
        public int? transactionId { get; set; }
        public int? Companyid { get; set; }
        public string? workOrderId { get; set; }
        public double? bookingRate { get; set; }
        public double? outHoseRate { get; set; }
        public int? outHosueLabID { get; set; }
        public string? outhouseLabName { get; set; }
        public DateTime? OutsouceDate { get; set; }
        public int? sentByid { get; set; }
        public string? SentName { get; set; }
        public string? remarks { get; set; }
    }
}
