using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface IdegreeMasterServices
    {
        Task<ServiceStatusResponseModel> SaveUpdateDegree(degreeMaster Degree);
        Task<ServiceStatusResponseModel> UpdateDegreeStatus(int id, byte status, int userId);

    }
}
