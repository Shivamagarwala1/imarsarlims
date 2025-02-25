using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface ItnxInvestigationRemarksService
    {
        Task<ServiceStatusResponseModel> AddSampleRemark(tnx_InvestigationRemarks remark);
        Task<ServiceStatusResponseModel> GetSampleremark(int transacctionId, string WorkOrderId, int itemId);
    }
}
