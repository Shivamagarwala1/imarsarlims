using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface IitemObservation_isnablService
    { 
        Task<ServiceStatusResponseModel> SaveUpdateNabl(NablRequestModel Nabldata);
        Task<ServiceStatusResponseModel> UploadNablLogo(IFormFile NablLogo, int centreId);
        Task<ServiceStatusResponseModel> RemoveNabl(int id);
        Task<ServiceStatusResponseModel> GetNablData(int CentreId, int itemId);
    }
}
