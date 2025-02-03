using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface IdocumentTypeMasterServices
    {
        Task<ServiceStatusResponseModel> SaveUpdateDocumentType(documentTypeMaster DocumnetType);
        Task<ServiceStatusResponseModel> UpdateDocumnetTypeStatus(int id, byte status, int userId);
    }
}
