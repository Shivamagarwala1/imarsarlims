using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface IitemTemplateServices
    {
        Task<ServiceStatusResponseModel> SaveUpdateTemplate(itemTemplate Template);
        Task<ServiceStatusResponseModel> UpdateTemplateStatus(int id, byte status, int Userid);
        Task<ServiceStatusResponseModel> GetTemplateData(int CentreID, int testid);
    }
}
