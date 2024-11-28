using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface IcentreMasterServices
    {
        Task<ServiceStatusResponseModel> SaveCentreDetail(centreMaster centremaster);
    }
}
