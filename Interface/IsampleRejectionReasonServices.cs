using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface IsampleRejectionReasonServices
    {
        Task<ServiceStatusResponseModel> SaveUpdateRejectionReason(sampleRejectionReason RejectionReason);
        Task<ServiceStatusResponseModel> UpdateRejectionReasonStatus(int id, byte status, int Userid);
    }
}
