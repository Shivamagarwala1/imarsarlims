using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(chatMessage))]
    public class chatMessage : Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int messageId { get; set; }
        [Required, MaxLength(2000)]
        public string content { get; set; }
        public bool isSeen { get; set; } 
        [ForeignKey("empMaster")]
        public int? empId { get; set; }
        [ForeignKey("chatGroupMaster")]
        public int? groupMasterId { get; set; }
        public chatGroupMaster? chatGroupMaster { get; set; }
        public empMaster? empMaster { get; set; }
        [MaxLength(200)]
        public string? fileName { get; set; } // File name if a file is attached
        [MaxLength(500)]
        public string? fileUrl { get; set; }  // File path or URL

    }
}
