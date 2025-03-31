using iMARSARLIMS.Interface;
using iMARSARLIMS.Interface.Store;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Model.Store;
using iMARSARLIMS.Response_Model;
using iMARSARLIMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketingDashBoardController : BaseController<MarketingDashBoard>
    {
        private readonly ContextClass db;
        private readonly IMarketingDashBoardServices _MarketingDashBoardServices;

        public MarketingDashBoardController(ContextClass context, ILogger<BaseController<MarketingDashBoard>> logger, IMarketingDashBoardServices MarketingDashBoardServices) : base(context, logger)
        {
            db = context;
            this._MarketingDashBoardServices = MarketingDashBoardServices;
        }
        protected override IQueryable<MarketingDashBoard> DbSet => db.MarketingDashBoard.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("SaveUpdateDashboard")]
        public async Task<ServiceStatusResponseModel> SaveUpdateDashboard(MarketingDashBoard DashboardData)
        {
            try
            {
                var result = await _MarketingDashBoardServices.SaveUpdateDashboard(DashboardData);
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

        [HttpPost("DeactiveDashboardImage")]
        public async Task<ServiceStatusResponseModel> DeactiveDashboardImage(int id,int userid, byte status)
        {
            try
            {
                var result = await _MarketingDashBoardServices.DeactiveDashboardImage(id,userid,status);
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
        [HttpPost("UploadDocument")]

        public async Task<ActionResult<ServiceStatusResponseModel>> UploadDocument(IFormFile Document)
        {
            try
            {
                var result = await _MarketingDashBoardServices.UploadDocument(Document);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetDashBoardData")]

        public async Task<ActionResult<ServiceStatusResponseModel>> GetDashBoardData(string type)
        {
            try
            {
                var result = await _MarketingDashBoardServices.GetDashBoardData(type);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ViewMarketingDashboard")]

        public async Task<ActionResult<ServiceStatusResponseModel>> ViewMarketingDashboard(string type)
        {
            try
            {
                var result = await _MarketingDashBoardServices.ViewMarketingDashboard(type);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
