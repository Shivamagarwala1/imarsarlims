using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(CentreCertificate))]
    public class CentreCertificate:Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int centreId { get; set; }
        [MaxLength(200)]
        public string centreName { get; set; }
        public DateTime certificateDate { get; set; }


    }
}
