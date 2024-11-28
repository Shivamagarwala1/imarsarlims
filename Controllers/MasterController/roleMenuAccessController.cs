using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class roleMenuAccessController : BaseController<roleMenuAccess>
    {
        private readonly ContextClass db;
        private readonly IroleMenuAccessServices _roleMenuAccessServices;

        public roleMenuAccessController(ContextClass context, ILogger<BaseController<roleMenuAccess>> logger, IroleMenuAccessServices roleMenuAccessServices) : base(context, logger)
        {
            db = context;
            this._roleMenuAccessServices = roleMenuAccessServices;
        }
        protected override IQueryable<roleMenuAccess> DbSet => db.roleMenuAccess.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("SaveRoleMenuAccess")]
        public async Task<ServiceStatusResponseModel> SaveRoleMenuAccess(RoleMenuAccessRequestModel roleMenu)
        {
            try
            {
                var result = await _roleMenuAccessServices.SaveRoleMenuAccess(roleMenu);
                return result;
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = ex.InnerException?.Message ?? "An error occurred."
                };
            }
        }

        [HttpPost("GetMenuList")]
        public async Task<ServiceStatusResponseModel> GetMenuList(menuAccess MenuAccess)
        {
            try
            {
                var result = await _roleMenuAccessServices.GetMenuList(MenuAccess);
                return result;
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = ex.InnerException?.Message ?? "An error occurred."
                };
            }
        }

    }
}
