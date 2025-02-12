using Google.Protobuf.WellKnownTypes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(chatGroupMasterEmployee))]
    public class chatGroupMasterEmployee :Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int groupMasterEmployeeId { get; set; }
        [ForeignKey("chatGroupMaster")]
        public int groupMasterId { get; set; }
        [ForeignKey("empMaster")]
        public int empId { get; set; }
    }
}
