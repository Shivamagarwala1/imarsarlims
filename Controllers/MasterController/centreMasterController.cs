using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class centreMasterController : BaseController<centreMaster>
    {
        private readonly ContextClass db;
        private readonly IcentreMasterServices _centreMasterServices;
        public centreMasterController(ContextClass context, ILogger<BaseController<centreMaster>> logger, IcentreMasterServices centreMasterServices) : base(context, logger)
        {
            db = context;
            this._centreMasterServices = centreMasterServices;
        }
        protected override IQueryable<centreMaster> DbSet => db.centreMaster.AsNoTracking().OrderBy(o => o.centreId);

        [HttpPost("SaveCentreDetail")]
        public async Task<ServiceStatusResponseModel> SaveCentreDetail(centreMaster centremaster)
        {
            try
            {
                var result = await _centreMasterServices.SaveCentreDetail(centremaster);
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

        [HttpPost("UpdateCentreStatus")]
        public async Task<ServiceStatusResponseModel> UpdateCentreStatus(int CentreId, bool status, int UserId)
        {
            try
            {
                var result = await _centreMasterServices.UpdateCentreStatus(CentreId, status, UserId);
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

        [HttpGet("GetParentCentre")]
        public async Task<ServiceStatusResponseModel> GetParentCentre()
        {
            try
            {
                var result = await _centreMasterServices.GetParentCentre();
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

        [HttpGet("GetRateType")]
        public async Task<ServiceStatusResponseModel> GetRateType()
        {
            try
            {
                var result = await _centreMasterServices.GetRateType();
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
        [HttpGet("GetCentreData")]
        public async Task<ServiceStatusResponseModel> GetCentreData(int centreId)
        {
            try
            {
                var result = await _centreMasterServices.GetCentreData(centreId);
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
