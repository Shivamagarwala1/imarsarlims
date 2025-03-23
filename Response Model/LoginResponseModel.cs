namespace iMARSARLIMS.Response_Model
{
    public class LoginResponseModel
    {
        public string employeeId { get; set; }
        public string Name { get; set; }
        public string DefaultRole { get; set; }
        public string DefaultCenter { get; set; }
        public string tempPassword { get; set; }
        public string image { get; set; }
        public byte? allowTicket { get; set; }
        public int? allowTicketRole { get; set; }
    }
}
