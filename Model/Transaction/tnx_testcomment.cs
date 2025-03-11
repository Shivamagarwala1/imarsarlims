using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Transaction
{
    [Table(nameof(tnx_testcomment))]
    public class tnx_testcomment :Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int testid { get; set; }
        public string Comment { get; set; }
       
    }
}
