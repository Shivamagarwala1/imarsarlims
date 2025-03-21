using iMARSARLIMS.Model.SupportTicket;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface.SupportTicket
{
    public interface IsupportTicketServices
    {
        Task<ServiceStatusResponseModel> saveUpdateSupportTicket(supportTicket ticket);
        Task<ServiceStatusResponseModel> AssignTicket(int ticketId, int AssigneTo, DateTime DeliveryDate, int UserId);
        Task<ServiceStatusResponseModel> closeTicket(int ticketId, string actionTaken, int UserId);
        Task<ServiceStatusResponseModel> ReOpenTicket(int ticketId, string reOpenReason, int UserId);
        Task<ServiceStatusResponseModel> GetTicketDetails();
    }
}
