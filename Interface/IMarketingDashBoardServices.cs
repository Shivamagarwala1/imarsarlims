using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;

namespace iMARSARLIMS.Interface
{
    public interface IMarketingDashBoardServices
    {
        Task<ServiceStatusResponseModel> SaveUpdateDashboard(MarketingDashBoard DashboardData);
        Task<ServiceStatusResponseModel> DeactiveDashboardImage(int id, int userid, byte status);
        Task<ServiceStatusResponseModel> UploadDocument(IFormFile Document);
        Task<ServiceStatusResponseModel> GetDashBoardData(string type);
        Task<ServiceStatusResponseModel> ViewMarketingDashboard();
    }
}
