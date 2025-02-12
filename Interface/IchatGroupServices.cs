using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;

namespace iMARSARLIMS.Interface
{
    public interface IchatGroupServices
    {
        Task<chatGroupMaster> CreateChatGroupAsync(chatGroupMaster chatGroup);

        Task<chatGroupMaster> CheckGroupMessegesAsync(int groupMasterId);
    }

}
