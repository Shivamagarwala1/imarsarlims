using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface IinvestigationUDServices
    {
        Task<ServiceStatusResponseModel> UpdateInvestigationFormat(InvestigationMasterUD FormatData);
        Task<ServiceStatusResponseModel> RemoveInvestigationFormat(int Id);
        Task<ServiceStatusResponseModel> UploadDocument(IFormFile Document);
    }
}
