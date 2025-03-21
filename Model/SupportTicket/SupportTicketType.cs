using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.SupportTicket
{
    [Table(nameof(SupportTicketType))]
    public class SupportTicketType :Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [MaxLength(50)]
        public string ticketType { get; set; }
    }
}
