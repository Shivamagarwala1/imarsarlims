﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(itemRateMaster))]
    public class itemRateMaster:Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int id { get; set; }
        public int itemId { get; set; }
        public int? rateTypeId { get; set; }
        [MaxLength(20)]
        public string? itemCode { get; set; }
    }
}
