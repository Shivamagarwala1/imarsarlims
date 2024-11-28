using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(documentMaster))]
    public class documentMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public int transactionId { get; set; }
        [Required,MaxLength(100)]
        public string fileName { get; set; }
        [Required]
        public int documentTypeId { get; set; }
    }
}
