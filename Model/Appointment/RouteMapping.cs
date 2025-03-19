using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Appointment
{
    [Table(nameof(RouteMapping))]
    public class RouteMapping:Audit
    { 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int pheleboId { get; set; }
        public int routeId { get; set; }
    }
}
