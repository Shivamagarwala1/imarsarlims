using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using iMARSARLIMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
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

        [HttpGet("GetAllRoleMenuAcess")]
        public async Task<ActionResult<ServiceStatusResponseModel>> GetAllRoleMenuAcess(ODataQueryOptions<roleMenuAccess> queryOptions)
        {
            try
            {
                var result = await _roleMenuAccessServices.GetAllRoleMenuAcess(queryOptions);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("EmpPageAccessRemove")]
        public async Task<ActionResult<ServiceStatusResponseModel>> EmpPageAccessRemove(int Id)
        {
            try
            {
                var result = await _roleMenuAccessServices.EmpPageAccessRemove(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

    }
}
