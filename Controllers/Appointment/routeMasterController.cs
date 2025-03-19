using iMARSARLIMS.Interface;
using iMARSARLIMS.Interface.appointment;
using iMARSARLIMS.Model.Appointment;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using iMARSARLIMS.Services;
using iMARSARLIMS.Services.Appointment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.Appointment
{
    [Route("api/[controller]")]
    [ApiController]
    public class routeMasterController : BaseController<routeMaster>
    {
        private readonly ContextClass db;
        private readonly IrouteMasterServices _routeMasterServices;

        public routeMasterController(ContextClass context, ILogger<BaseController<routeMaster>> logger, IrouteMasterServices routeMasterServices) : base(context, logger)
        {
            db = context;
            this._routeMasterServices = routeMasterServices;
        }
        protected override IQueryable<routeMaster> DbSet => db.routeMaster.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("SaveUpdateRoute")]
        public async Task<ServiceStatusResponseModel> SaveUpdateRoute(routeMaster Route)
        {
            try
            {
                var result = await _routeMasterServices.SaveUpdateRoute(Route);
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

        [HttpPost("UpdateRouteStatus")]
        public async Task<ServiceStatusResponseModel> UpdateRouteStatus(int id, byte status, int Userid)
        {
            try
            {
                var result = await _routeMasterServices.UpdateRouteStatus(id,status,Userid);
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
