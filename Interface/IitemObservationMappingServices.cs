using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;

namespace iMARSARLIMS.Interface
{
    public interface IitemObservationMappingServices
    {
        Task<ServiceStatusResponseModel> SaveObservationMapping(List<ItemObservationMapping> itemObservationMapping);
    }
}
