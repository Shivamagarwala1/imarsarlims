using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface ItatMasterServices
    {
        Task<ServiceStatusResponseModel> SaveUpdateTatMaster(List<tat_master> Tatdata);
        Task<ServiceStatusResponseModel> GetTatMaster(int centreId, int departmentId, List<int> ItemIds);
    }
}
