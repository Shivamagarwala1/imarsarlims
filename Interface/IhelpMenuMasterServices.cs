using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface IhelpMenuMasterServices
    {
        Task<ServiceStatusResponseModel> SaveUpdateHelpMenu(helpMenuMaster HelpMenu);
        Task<ServiceStatusResponseModel> SaveHelpMenuMapping(helpMenuMapping HelpMenu);
        Task<ServiceStatusResponseModel> RemoveHelpMenuMapping(int helpId, int itemId, int observationId, int userId);
    }
}
