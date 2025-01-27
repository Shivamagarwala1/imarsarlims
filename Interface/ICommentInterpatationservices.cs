using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface ICommentInterpatationservices
    {
        Task<ServiceStatusResponseModel> SaveInterpatation(itemInterpretation Interpretation);
        Task<ServiceStatusResponseModel> updateInterpatationStatus(int InterpatationId, byte Status, int UserId);
        Task<ServiceStatusResponseModel> SaveCommentMaster(itemCommentMaster Comment);
        Task<ServiceStatusResponseModel> updateCommentStatus(int CommentId, byte Status, int UserId);
        Task<ServiceStatusResponseModel> GetCommentData(int CentreID, string type, int testid);

    }
}
