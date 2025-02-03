using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using iMARSARLIMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class discountReasonMasterController : BaseController<discountReasonMaster>
    {
        private readonly ContextClass db;
        private readonly IdiscountReasonMasterServices _discountReasonMasterServices;

        public discountReasonMasterController(ContextClass context, ILogger<BaseController<discountReasonMaster>> logger, IdiscountReasonMasterServices discountReasonMasterServices) : base(context, logger)
        {
            db = context;
            this._discountReasonMasterServices = discountReasonMasterServices;
        }
        protected override IQueryable<discountReasonMaster> DbSet => db.discountReasonMaster.AsNoTracking().OrderBy(o => o.id);
        
        [HttpPost("SaveUpdateDiscountReason")]
        public async Task<ServiceStatusResponseModel> SaveUpdateDiscountReason(discountReasonMaster DiscountReason)
        {
            try
            {
                var result = await _discountReasonMasterServices.SaveUpdateDiscountReason(DiscountReason);
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

        [HttpPost("UpdateDiscountReasonStatus")]
        public async Task<ServiceStatusResponseModel> UpdateDiscountReasonStatus(int id, byte status, int userId)
        {
            try
            {
                var result = await _discountReasonMasterServices.UpdateDiscountReasonStatus(id, status, userId);
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
