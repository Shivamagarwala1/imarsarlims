using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface ILocationsServices
    {
        Task<ServiceStatusResponseModel> SaveCity(cityMaster City);
        Task<ServiceStatusResponseModel> SaveDistrict(districtMaster District);
        Task<ServiceStatusResponseModel> SaveState(stateMaster State);
    }
}
