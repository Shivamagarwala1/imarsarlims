using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class rateTypeWiseRateListController : BaseController<rateTypeWiseRateList>
    {
        private readonly ContextClass db;
        private readonly IrateTypeWiseRateListServices _rateTypeWiseRateListServices;

        public rateTypeWiseRateListController(ContextClass context, ILogger<BaseController<rateTypeWiseRateList>> logger, IrateTypeWiseRateListServices rateTypeWiseRateListServices) : base(context, logger)
        {
            db = context;
            this._rateTypeWiseRateListServices = rateTypeWiseRateListServices;
        }
        protected override IQueryable<rateTypeWiseRateList> DbSet => db.rateTypeWiseRateList.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("SaveRateList")]
        public async Task<ActionResult<ServiceStatusResponseModel>> SaveRateList(List<rateTypeWiseRateList> RateTypeWiseRateList)
        {
            try
            {
                var result = await _rateTypeWiseRateListServices.SaveRateList(RateTypeWiseRateList);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("SaveRateListFromExcel")]
        public async Task<ActionResult<ServiceStatusResponseModel>> SaveRateListFromExcel(IFormFile ratelistexcel)
        {
            try
            {
                var result = await _rateTypeWiseRateListServices.SaveRateListFromExcel(ratelistexcel);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
