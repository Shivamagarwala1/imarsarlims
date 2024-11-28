namespace iMARSARLIMS.Response_Model
{
    public class LoginResponseModel
    {
        public int? employeeId { get; set; }
        public string? Name { get; set; }
        public int? DefaultRole { get; set; }
        public int? DefaultCenter { get; set; }
        public int? Centres { get; set; }
        public int? Roles { get; set; }
    }
}
