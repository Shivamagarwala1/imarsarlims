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
        public async Task<ServiceStatusResponseModel> SaveRateList(List<rateTypeWiseRateList> RateTypeWiseRateList)
        {
            try
            {
                var result = await _rateTypeWiseRateListServices.SaveRateList(RateTypeWiseRateList);
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

        [HttpPost("SaveRateListitemWise")]
        public async Task<ServiceStatusResponseModel> SaveRateListitemWise(List<rateTypeWiseRateList> RateTypeWiseRateList)
        {
            try
            {
                var result = await _rateTypeWiseRateListServices.SaveRateListitemWise(RateTypeWiseRateList);
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
        [HttpGet("GetRateListExcel")]
        public IActionResult GetRateListExcel(int RatetypeId)
        {
            try
            {
                var result = _rateTypeWiseRateListServices.GetRateListExcel(RatetypeId);
                return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "RateList.xlsx");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetItemrateListData")]
        public async Task<ServiceStatusResponseModel> GetItemrateListData(int itemid)
        {
            try
            {
                var result = await _rateTypeWiseRateListServices.GetItemrateListData(itemid);
                return result;
            }
            catch (Exception ex)
            {
               return new ServiceStatusResponseModel
               { Success = false,Message = ex.Message};
            }
        }
        [HttpGet("GetRateTypeRateListData")]
        public async Task<ServiceStatusResponseModel> GetRateTypeRateListData(int ratetypeid,int deptId)
        {
            try
            {
                var result = await _rateTypeWiseRateListServices.GetRateTypeRateListData(ratetypeid,deptId);
                return result;
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                { Success = false, Message = ex.Message };
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
