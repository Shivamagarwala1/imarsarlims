using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface IObservationMasterServices
    {
        Task<ServiceStatusResponseModel> SaveUpdateObservationMater(itemObservationMaster Observation);
    }
}
