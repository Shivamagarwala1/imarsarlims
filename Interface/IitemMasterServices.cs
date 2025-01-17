using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface IitemMasterServices
    {
        Task<ServiceStatusResponseModel> SaveItemMaster(itemMaster itemmaster);
        Task<ServiceStatusResponseModel> updateItemStatus(int ItemId, byte Status, int UserId);
    }
}
