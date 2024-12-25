using System.ComponentModel.DataAnnotations.Schema;
namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(Testing))]
    public class Testing
    {
        public int id { get; set; }
        public int titleId { get; set; }
        public string Name { get; set; }
        public DateTime DOB { get; set; }
        public string Department { get; set; }
        public int DesignationId { get; set; }
        public bool isactive { get; set; }
        public int EmpTypeId { get; set; }
        public string Image { get; set; }

    }
}
