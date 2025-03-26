using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.SupportTicket
{
    [Table(nameof(SupportTicketRemarks))]
    public class SupportTicketRemarks
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int ticketId { get; set; }
        [MaxLength(500)]
        public string Remarks { get; set; }
        public string status { get; set; }
        public int addedBy { get; set; }
        public DateTime AddedDate { get; set; }


    }
}
