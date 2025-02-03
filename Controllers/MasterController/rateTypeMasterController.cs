using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class rateTypeMasterController : BaseController<rateTypeMaster>
    {
        private readonly ContextClass db;
        private readonly IrateTypeMasterServices _rateTypeMasterServices;

        public rateTypeMasterController(ContextClass context, ILogger<BaseController<rateTypeMaster>> logger, IrateTypeMasterServices rateTypeMasterServices) : base(context, logger)
        {
            db = context;
            this._rateTypeMasterServices = rateTypeMasterServices;
        }
        protected override IQueryable<rateTypeMaster> DbSet => db.rateTypeMaster.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("SaveUpdateRateType")]
        public async Task<ServiceStatusResponseModel> SaveUpdateRateType(int rateTypeId, string rateTypeName, string CentreId, int userId)
        {
            try
            {
                var result = await _rateTypeMasterServices.SaveUpdateRateType(rateTypeId, rateTypeName, CentreId, userId);
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
        [HttpPost("UpdateRateTypeStatus")]
        public async Task<ServiceStatusResponseModel> UpdateRateTypeStatus(int id, byte status, int userId)
        {
            try
            {
                var result = await _rateTypeMasterServices.UpdateRateTypeStatus(id, status, userId);
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
