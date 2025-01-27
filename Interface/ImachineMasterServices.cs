using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface ImachineMasterServices
    {
        Task<ServiceStatusResponseModel> SaveMachineMaster(machineMaster Machine);
        Task<ServiceStatusResponseModel> SaveUpdateMapping(List<machineObservationMapping> MachineMapping);
    }
}
