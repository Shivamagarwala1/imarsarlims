using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface ItnxInvestigationAttachmentService
    {
        Task<ServiceStatusResponseModel> AddAttchment(tnx_InvestigationAttchment attchment);
        Task<ServiceStatusResponseModel> AddReport(tnx_InvestigationAddReport Report);
    }
}
