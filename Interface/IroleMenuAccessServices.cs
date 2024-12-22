using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface IroleMenuAccessServices
    {
        Task<ServiceStatusResponseModel> SaveRoleMenuAccess(RoleMenuAccessRequestModel roleMenu);
        Task<ServiceStatusResponseModel> GetMenuList(menuAccess MenuAccess);
    }
}
