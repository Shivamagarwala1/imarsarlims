using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(helpMenuMapping))]
    public class helpMenuMapping 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? id { get; set; }
        public int? helpId { get; set; }
        public int? labTestId { get; set; }
        public int? itemId { get; set; }
        public string? mappedName { get; set; }
        public string? helpName { get; set; }
}
}
