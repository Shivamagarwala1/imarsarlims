using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(RolePageAccess))]
    public class RolePageAccess :Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int roleid { get; set; }
        public int parentmenuid { get; set; }
        public int submenuId { get; set; }


    }
}
