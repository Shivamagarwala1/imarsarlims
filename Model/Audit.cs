using System.ComponentModel.DataAnnotations;

namespace iMARSARLIMS.Model
{
    public class Audit
    {
        public bool? isActive { get; set; }
        public int? createdById { get; set; }
        public DateTime createdDateTime { get; set; } 
        public int? updateById { get; set; } 
        public DateTime? updateDateTime { get; set; }   
    }
}
