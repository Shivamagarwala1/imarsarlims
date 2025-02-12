using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface IcentreMasterServices
    {
        Task<ServiceStatusResponseModel> SaveCentreDetail(centreMaster centremaster);
        Task<ServiceStatusResponseModel> UpdateCentreStatus(int CentreId, byte status, int UserId);
        Task<ServiceStatusResponseModel> GetParentCentre();
        Task<ServiceStatusResponseModel> GetProcesiongLab();
        Task<ServiceStatusResponseModel> GetMRPRateType();
        Task<ServiceStatusResponseModel> GetRateType(int CentreType, int ParentCentre);
        Task<ServiceStatusResponseModel> GetCentreData(int centreId);
        Task<ServiceStatusResponseModel> SaveLetterHead(ReportLetterHead LetterHead);
        Task<ServiceStatusResponseModel> GetRatetypeCentreWise(int CentreId);
    }
}
