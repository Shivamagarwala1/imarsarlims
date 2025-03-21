﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.SupportTicket
{
    [Table(nameof(supportTicket))]
    public class supportTicket
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int clientId { get; set; }
        [MaxLength(50)]
        public string clientName { get; set; }
        public int ticketTypeId { get; set; }
        public int Priority { get; set; }
        [MaxLength(500)]
        public string task { get; set; }
        public string Document { get; set; }
        public DateTime CreateDate { get; set; }
        public int assignedTo { get; set; }
        public int AssignedBy { get; set; }
        public DateTime? AssignedDate { get; set; }
        public byte isAssigned {  get; set; }
        public byte isCompleted { get; set; }

        public int completedBy { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime? Deliverydate { get; set; }
        [MaxLength(100)]
        public string ActionTaken { get; set; }
        public byte isReopen {  get; set; }
        public int ReopenBy { get; set; }
        public DateTime? ReopenDate { get; set; }
        [MaxLength(100)]
        public string? ReopenReason { get; set; }

    }
}
