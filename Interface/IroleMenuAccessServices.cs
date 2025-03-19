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

        Task<ServiceStatusResponseModel> SaveUpdateRole(roleMaster Role);
        Task<ServiceStatusResponseModel> UpdateRoleStatus(int id, byte status, int userId);
        Task<ServiceStatusResponseModel> SaveRolePageAccess(List<RolePageAccess> Rolepage);
        Task<ServiceStatusResponseModel> RolePageAccessRemove(int Id);
        Task<ServiceStatusResponseModel> GetEmployeePageAccess(int empid, int roleid);
        Task<ServiceStatusResponseModel> RolePagebindData(int roleid);
    }
}
