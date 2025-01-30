using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface IitemOutHouseServices
    {
        Task<ServiceStatusResponseModel> SaveOutHouseMapping(List<item_OutHouseMaster> OutHouseMapping);
        Task<ServiceStatusResponseModel> GetOutHouseMapping(int BookingCentre, int ProcessingCentre, int DeptId);
        Task<ServiceStatusResponseModel> RemoveOutHouseMapping(int id);
    }
}
