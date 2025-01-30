using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface IitemOutSourceServices
    {
        Task<ServiceStatusResponseModel> SaveOutSourceMapping(List<item_outsourcemaster> OutHouseMapping);
        Task<ServiceStatusResponseModel> GetOutSourceMapping(int BookingCentre, int OutSourceLab, int DeptId);
        Task<ServiceStatusResponseModel> RemoveOutSourceMapping(int id);
    }
}
