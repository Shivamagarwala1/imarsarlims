using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace iMARSARLIMS.Interface
{
    public interface IroleMenuAccessServices
    {
        Task<ServiceStatusResponseModel> SaveRoleMenuAccess(RoleMenuAccessRequestModel roleMenu);
        Task<ServiceStatusResponseModel> GetAllRoleMenuAcess(ODataQueryOptions<roleMenuAccess> queryOptions);
        Task<ServiceStatusResponseModel> EmpPageAccessRemove(int Id);
    }
}
