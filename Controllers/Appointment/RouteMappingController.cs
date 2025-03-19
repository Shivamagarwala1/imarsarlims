using iMARSARLIMS.Interface.appointment;
using iMARSARLIMS.Model.Appointment;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.Appointment
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouteMappingController : BaseController<RouteMapping>
    {
        private readonly ContextClass db;
        private readonly IrouteMasterServices _routeMasterServices;

        public RouteMappingController(ContextClass context, ILogger<BaseController<RouteMapping>> logger, IrouteMasterServices routeMasterServices) : base(context, logger)
        {
            db = context;
            this._routeMasterServices = routeMasterServices;
        }
        protected override IQueryable<RouteMapping> DbSet => db.RouteMapping.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("SaveUpdateRouteMapping")]
        public async Task<ServiceStatusResponseModel> SaveUpdateRouteMapping(List<RouteMapping> Route)
        {
            try
            {
                var result = await _routeMasterServices.SaveUpdateRouteMapping(Route);
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

        [HttpPost("UpdateRouteMappingStatus")]
        public async Task<ServiceStatusResponseModel> UpdateRouteMappingStatus(int id, byte status, int Userid)
        {
            try
            {
                var result = await _routeMasterServices.UpdateRouteMappingStatus(id, status, Userid);
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
        [HttpGet("GetRouteMapping")]
        public async Task<ServiceStatusResponseModel> GetRouteMapping(int PheleboId)
        {
            try
            {
                var result = await _routeMasterServices.GetRouteMapping(PheleboId);
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
