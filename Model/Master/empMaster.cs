﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(empMaster))]
    public class empMaster : Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int empId { get; set; }
        [Required, MaxLength(20)]
        public string empCode { get; set; }
        [Required, MaxLength(10)]
        public string title { get; set; }
        [Required, MaxLength(50)]
        public string fName { get; set; }
        [Required, MaxLength(50)]
        public string lName { get; set; }
        [MaxLength(100)]
        public string address { get; set; }
        public int? pinCode { get; set; }
        [Required, MaxLength(50)]
        public string email { get; set; }
        [Required, MaxLength(10)]
        public string mobileNo { get; set; }
        [Required]
        public DateTime dob { get; set; }
        [MaxLength(50)]
        public string? qualification { get; set; }
        [MaxLength(5)]
        public string? bloodGroup { get; set; }
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
        [Required]
        public int defaultcentre { get; set; }
        public int? pro { get; set; }
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
        [MaxLength(20)]
        public string? fromIP { get; set; }
        [MaxLength(20)]
        public string? toIP { get; set; }
        public byte isdeviceAuthentication { get; set; }

        [MaxLength(20)]
        public string? adminPassword { get; set; }
        [MaxLength(20)]
        public string? tempPassword { get; set; }
        public int? district { get; set; }
        public byte? indentIssue { get; set; }
        public byte? IndentApprove { get; set; }
        public byte? allowTicket { get; set; }
        public int allowTicketRole { get; set; }
        [MaxLength(30)]
        public string? employeeCentretype { get; set; } = "";
        
        [ForeignKey(nameof(empId))]
        public List<empCenterAccess>? addEmpCentreAccess { get; set; }
        [ForeignKey(nameof(empId))]
        public List<empRoleAccess>? addEmpRoleAccess { get; set; }
        [ForeignKey(nameof(empId))]
        public List<empDepartmentAccess>? addEmpDepartmentAccess { get; set; }

        [ForeignKey(nameof(empId))]
        public List<chatGroupMasterEmployee>? addChatGroupMasterEmployee { get; set; }
        [ForeignKey(nameof(empId))]
        public List<chatMessage>? addChatchatMessage { get; set; }

    }

}
