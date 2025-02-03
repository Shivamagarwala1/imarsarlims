using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface ItitleMasterServices
    {
        Task<ServiceStatusResponseModel> SaveUpdateTitle(titleMaster Title);
        Task<ServiceStatusResponseModel> UpdateTitleStatus(int id, byte status, int userId);
    }
}
