using iMARSARLIMS.Model.Store;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface.Store
{
    public interface IItemMasterStoreServices
    {
        Task<ServiceStatusResponseModel> UpdateItemStatus(int id, byte status, int userId);
        Task<ServiceStatusResponseModel> SaveUpdateItemStore(ItemMasterStore item);
    }
}
