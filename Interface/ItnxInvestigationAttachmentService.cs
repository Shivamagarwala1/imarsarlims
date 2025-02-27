using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface ItnxInvestigationAttachmentService
    {
        Task<ServiceStatusResponseModel> AddReport(tnx_InvestigationAttchment attchment);
    }
}
