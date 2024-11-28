using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(empDepartmentAccess))]
    public class empDepartmentAccess : Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public int empId { get; set; }
        [Required]
        public int departmentId { get; set; }
    }
}
