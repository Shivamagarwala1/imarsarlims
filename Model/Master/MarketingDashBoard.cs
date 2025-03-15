using Org.BouncyCastle.Bcpg;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(MarketingDashBoard))]
    public class MarketingDashBoard :Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [MaxLength(50)]
        public string type { get; set; }
        [MaxLength(50)]
        public string Subject { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        public string Image { get; set; }
        public string Pdf { get; set; }

    }
}
