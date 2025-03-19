using iMARSARLIMS.Model.Appointment;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface.appointment
{
    public interface IrouteMasterServices
    {
        Task<ServiceStatusResponseModel> SaveUpdateRoute(routeMaster Route);
        Task<ServiceStatusResponseModel> UpdateRouteStatus(int id, byte status, int Userid);
        Task<ServiceStatusResponseModel> SaveUpdateRouteMapping(List<RouteMapping> Route);
        Task<ServiceStatusResponseModel> UpdateRouteMappingStatus(int id, byte status, int Userid);
        Task<ServiceStatusResponseModel> GetRouteMapping(int PheleboId);
    }
}
