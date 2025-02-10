using Google.Protobuf.WellKnownTypes;
using Org.BouncyCastle.Ocsp;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(chatGroupMaster))]
    public class chatGroupMaster :Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int groupMasterId { get; set; }
        [Required, MaxLength(100)]
        public string groupMasterName { get; set; }

        [ForeignKey(nameof(groupMasterId))]
        public List<chatGroupMasterEmployee>? addChatGroupMasterEmployee { get; set; }

        [ForeignKey(nameof(groupMasterId))]
        public List<chatMessage>? addChatMessage { get; set; }
    }
}
