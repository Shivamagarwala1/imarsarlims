using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;

namespace iMARSARLIMS.Interface
{
    public interface IroleMenuAccessServices
    {
        Task<ServiceStatusResponseModel> SaveRoleMenuAccess(RoleMenuAccessRequestModel roleMenu);
        Task<ServiceStatusResponseModel> GetMenuList(menuAccess MenuAccess);
    }
}
