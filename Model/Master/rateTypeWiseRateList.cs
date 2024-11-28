using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(rateTypeWiseRateList))]
    public class rateTypeWiseRateList
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int deptId { get; set; }
        public int panelId { get; set; }
        public int rateTypeId { get; set; }
        public double mrp { get; set; }
        public double discount { get; set; }
        public double rate { get; set; }
        public int itemid { get; set; }
        public string? itemCode { get; set; }
        public string? panelItemName { get; set; }
        public string? createdBy { get; set; }
        public int createdById { get; set; }
        public DateTime createdOn { get; set; }
        public string? transferRemarks { get; set; }
        public DateTime? transferDate { get; set; }
    }
}
