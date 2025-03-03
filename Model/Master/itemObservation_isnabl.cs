using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(itemObservation_isnabl))]
    public class itemObservation_isnabl:Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int observationId { get; set; }
        public int itemid { get; set; }
        public int centreId { get; set; }
        public string? NablLogo { get; set; }
        public int IsDefaultLogo { get; set; }
    }
}
