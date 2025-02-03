using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class roleMasterController : BaseController<roleMaster>
    {
        private readonly ContextClass db;
        private readonly IroleMenuAccessServices _roleMenuAccessServices;

        public roleMasterController(ContextClass context, ILogger<BaseController<roleMaster>> logger, IroleMenuAccessServices roleMenuAccessServices) : base(context, logger)
        {
            db = context;
            this._roleMenuAccessServices= roleMenuAccessServices;
        }
        protected override IQueryable<roleMaster> DbSet => db.roleMaster.AsNoTracking().OrderBy(o => o.id);
        [HttpPost("SaveUpdateRole")]
        public async Task<ServiceStatusResponseModel> SaveUpdateRole(roleMaster Role)
        {
            try
            {
                var result = await _roleMenuAccessServices.SaveUpdateRole(Role);
                return result;
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = ex.Message
                };

            }
        }

        [HttpPost("UpdateRoleStatus")]
        public async Task<ServiceStatusResponseModel> UpdateRoleStatus(int id, byte status, int userId)
        {
            try
            {
                var result = await _roleMenuAccessServices.UpdateRoleStatus(id, status, userId);
                return result;
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = ex.Message
                };

            }
        }

    }
}
