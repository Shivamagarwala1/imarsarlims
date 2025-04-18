﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{

    [Table(nameof(labDepartment))]
    public class labDepartment :Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required, MaxLength(20)]
        public string deptCode { get; set; }
        [Required, MaxLength(100)]
        public string deptName { get; set; }
        [Required, MaxLength(100)]
        public string subDeptName { get; set; }
        [MaxLength(5)]
        public string? abbreviation { get; set; }
        public int printSequence { get; set; }
    }
}
