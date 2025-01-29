using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using iMARSARLIMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class rateTypeTaggingController : BaseController<rateTypeTagging>
    {
        public readonly ContextClass db;
        private readonly IrateTypeMasterServices _rateTypeMasterServices;
        public rateTypeTaggingController(ContextClass context, ILogger<BaseController<rateTypeTagging>> logger, IrateTypeMasterServices rateTypeMasterServices) : base(context, logger)
        {
            db = context;
            this._rateTypeMasterServices = rateTypeMasterServices;
        }
        protected override IQueryable<rateTypeTagging> DbSet => db.rateTypeTagging.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("GetRatetypeTagging")]
        public async Task<ServiceStatusResponseModel> GetRatetypeTagging()
        {
            try
            {
                var result = await _rateTypeMasterServices.GetRatetypeTagging();
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
