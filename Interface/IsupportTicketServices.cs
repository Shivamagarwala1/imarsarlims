using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface IsupportTicketServices
    {
        Task<ServiceStatusResponseModel> saveUpdateSupportTicket(supportTicket ticket);
    }
}
