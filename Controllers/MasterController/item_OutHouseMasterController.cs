using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class item_OutHouseMasterController : BaseController<item_OutHouseMaster>
    {
        private readonly ContextClass db;
        private readonly IitemOutHouseServices _itemOutHouseServices;

        public item_OutHouseMasterController(ContextClass context, ILogger<BaseController<item_OutHouseMaster>> logger, IitemOutHouseServices itemOutHouseServices) : base(context, logger)
        {
            db = context;
            this._itemOutHouseServices = itemOutHouseServices;
        }
        protected override IQueryable<item_OutHouseMaster> DbSet => db.item_OutHouseMaster.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("SaveOutHouseMapping")]
        public async Task<ServiceStatusResponseModel> SaveOutHouseMapping(List<item_OutHouseMaster> OutHouseMapping)
        {
            try
            {
                var result = await _itemOutHouseServices.SaveOutHouseMapping(OutHouseMapping);
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
        [HttpGet("GetOutHouseMapping")]
        public async Task<ServiceStatusResponseModel> GetOutHouseMapping(int BookingCentre, int ProcessingCentre,int DeptId)
        {
            try
            {
                var result = await _itemOutHouseServices.GetOutHouseMapping(BookingCentre, ProcessingCentre,DeptId);
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
        [HttpPost("RemoveOutHouseMapping")]
        public async Task<ServiceStatusResponseModel> RemoveOutHouseMapping(int id)
        {
            try
            {
                var result = await _itemOutHouseServices.RemoveOutHouseMapping(id);
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
    }
}
