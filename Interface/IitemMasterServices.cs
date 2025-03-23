using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface IitemMasterServices
    {
        Task<ServiceStatusResponseModel> SaveItemMaster(itemMaster itemmaster);
        Task<ServiceStatusResponseModel> updateItemStatus(int ItemId, byte Status, int UserId);
        Task<ServiceStatusResponseModel> GetItemMasterAll();
        Task<ServiceStatusResponseModel> GetItemForTemplate();
        Task<ServiceStatusResponseModel> GetItemObservation(int itemtype);
        Task<ServiceStatusResponseModel> GetMappedItem(int itemtype, int itemid);
        Task<ServiceStatusResponseModel> RemoveMapping(int Id);
        Task<ServiceStatusResponseModel> EvaluateTest(int itemid1, int itemid2, int itemid3);
        byte[] DownloadDOS();
        Task<ServiceStatusResponseModel> GetDOS();
    }
}
