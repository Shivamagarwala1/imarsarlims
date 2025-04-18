﻿using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(rateTypeWiseRateList))]
    public class rateTypeWiseRateList :Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int deptId { get; set; }
        public int rateTypeId { get; set; }
        public double mrp { get; set; }
        public double discount { get; set; }
        public double rate { get; set; }
        public int itemid { get; set; }
        public string? itemCode { get; set; }
        public string? transferRemarks { get; set; }
        public DateTime? transferDate { get; set; }
    }
}
