using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class itemMasterController : BaseController<itemMaster>
    {
        private readonly ContextClass db;
        private readonly IitemMasterServices _itemMasterServices;

        public itemMasterController(ContextClass context, ILogger<BaseController<itemMaster>> logger, IitemMasterServices itemMasterServices) : base(context, logger)
        {
            db = context;
            this._itemMasterServices = itemMasterServices;
        }
        protected override IQueryable<itemMaster> DbSet => db.itemMaster.AsNoTracking().OrderBy(o => o.itemId);

        [HttpPost("SaveItemMaster")]
        public async Task<ServiceStatusResponseModel> SaveItemMaster(itemMaster itemmaster)
        {
            try
            {
                var result = await _itemMasterServices.SaveItemMaster(itemmaster);
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
        [HttpPost("updateItemStatus")]
        public async Task<ServiceStatusResponseModel> updateItemStatus(int ItemId,bool Status, int UserId)
        {
            try
            {
                var result = await _itemMasterServices.updateItemStatus(ItemId,Status,UserId);
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
        [HttpGet("GetItemMaster")]
        public async Task<ServiceStatusResponseModel> GetItemMaster(int ItemId)
        {
            var result = await db.itemMaster.Where(p => p.itemId == ItemId)
                              .Include(p => p.AddSampletype).FirstOrDefaultAsync();
            if (result == null)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = "Please enter correct ItemId"
                };
            }
            return new ServiceStatusResponseModel
            {
                Success = true,
                Data = result
            };
        }

    }
}

