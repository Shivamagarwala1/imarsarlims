using System.ComponentModel.DataAnnotations;

namespace iMARSARLIMS.Request_Model
{
    public class LoginRequestModel
    {
        [Required]
        public string userName { get; set; }
        [Required]
        public string password { get; set; }
    }
}
