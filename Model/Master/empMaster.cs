﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(empMaster))]
    public class empMaster : Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required, MaxLength(20)]
        public string empCode { get; set; }
        [Required, MaxLength(10)]
        public string title { get; set; }
        [Required, MaxLength(50)]
        public string fName { get; set; }
        [Required, MaxLength(50)]
        public string lName { get; set; }
        [ MaxLength(100)]
        public string address { get; set; }
        public int pinCode { get; set; }
        [Required, MaxLength(50)]
        public string email { get; set; }
        [Required,MaxLength(10)]
        public string mobileNo { get; set; }
        [Required, MaxLength(15)]
        public string landline { get; set; }
        [Required, MaxLength(30)]
        public string deptAccess { get; set; }
        public DateTime dob { get; set; }
        [MaxLength(50)]
        public string? qualification { get; set; }
        [ MaxLength(5)]
        public string bloodGroup { get; set; }
        [Required]
        public int designationId { get; set; }
        [Required, MaxLength(20)]
        public string userName { get; set; }
        [Required, MaxLength(20)]
        public string password { get; set; }
        public int zone { get; set; }
        public int state { get; set; }
        public int city { get; set; }
        public int area { get; set; }
        public int defaultcentre { get; set; }
        public int pro { get; set; }
        [Required]
        public int defaultrole { get; set; }
        public byte rate { get; set; }
        public string fileName { get; set; }
        public byte autoCreated { get; set; }
        public int centreId { get; set; }
        public byte allowDueReport { get; set; }
        public int employeeType { get; set; }
        public byte isSalesTeamMember { get; set; }
        public byte isDiscountAppRights { get; set; }
        public byte isPwdchange { get; set; }
        public byte isemailotp { get; set; }
        public string fromIP { get; set; }
        public string toIP { get; set; }
        public byte isdeviceAuthentication { get; set; }

        [MaxLength(20)]
        public string? adminPassword { get; set; }

        public List<empCenterAccess>? addEmpCentreAccess { get; set; }
        public List<empRoleAccess>? addEmpRoleAccess { get; set; }

    }
}
