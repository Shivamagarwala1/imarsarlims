using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface IrateTypeMasterServices
    {
        Task<ServiceStatusResponseModel> SaveUpdateRateType(int rateTypeId, string rateTypeName, string CentreId, int userId);
        Task<ServiceStatusResponseModel> GetRatetypeTagging();

    }
}
