﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(ItemObservationMapping))]
    public class ItemObservationMapping :Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int? itemId { get; set; }
        public int? observationID { get; set; }
        public byte? isTest { get; set; }
        public byte? isProfile { get; set; }
        public byte? isPackage { get; set; }
        public short itemType { get; set; }
        [MaxLength(200)]
        public string? formula { get; set; }
        public byte? dlcCheck { get; set; }
        public byte? showInReport { get; set; }
        public byte? isHeader { get; set; }
        public byte? isBold { get; set; }
        public byte? isCritical { get; set; }
        public byte? printSeparate { get; set; }
        public int? printOrder { get; set; }
    }
}
