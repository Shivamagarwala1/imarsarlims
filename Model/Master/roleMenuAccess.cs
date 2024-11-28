using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(roleMenuAccess))]
    public class roleMenuAccess
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public int? roleId { get; set; }
        public int? menuId { get; set; }
        public int? subMenuId { get; set; }
        public int? employeeId { get; set; }
        public bool isActive { get; set; } = true;
    }
}
