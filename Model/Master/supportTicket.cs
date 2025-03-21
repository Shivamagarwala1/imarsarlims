using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(supportTicket))]
    public class supportTicket
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int clientId { get; set; }
        [MaxLength(50)]
        public string clientName { get; set; }
        [MaxLength(40)]
        public string taskTypeName { get; set; }
        public int taskTypeId   { get; set; }
        [MaxLength(500)]
        public string task { get; set; }
        public int assignedTo { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime CompletedDate { get; set; }
        public DateTime Deliverydate { get; set; }
        [MaxLength(20)]
        public string ActionTaken { get; set; }

    }
}
