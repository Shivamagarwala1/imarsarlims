using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(helpMenuMapping))]
    public class helpMenuMapping 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? id { get; set; }
        public int? helpId { get; set; }
        public int? ObservationId { get; set; }
        public int? itemId { get; set; }
        public int mappedById{ get; set; }
        public DateTime mappedDate { get; set; }
        public int isActive { get; set; }
        public int removedById { get; set; }
        public DateTime removedDate { get; set; }
}
}
