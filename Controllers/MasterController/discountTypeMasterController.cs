using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class discountTypeMasterController : BaseController<discountTypeMaster>
    {
        private readonly ContextClass db;
        private readonly IdiscountReasonMasterServices _discountReasonMasterServices;

        public discountTypeMasterController(ContextClass context, ILogger<BaseController<discountTypeMaster>> logger, IdiscountReasonMasterServices discountReasonMasterServices) : base(context, logger)
        {
            db = context;
            this._discountReasonMasterServices = discountReasonMasterServices;
        }
        protected override IQueryable<discountTypeMaster> DbSet => db.discountTypeMaster.AsNoTracking().OrderBy(o => o.id);
        [HttpPost("SaveUpdateDiscountType")]
        public async Task<ServiceStatusResponseModel> SaveUpdateDiscountType(discountTypeMaster Discount)
        {
            try
            {
                var result = await _discountReasonMasterServices.SaveUpdateDiscountType(Discount);
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

        [HttpPost("UpdateDiscountTypeStatus")]
        public async Task<ServiceStatusResponseModel> UpdateDiscountTypeStatus(int id, byte status, int userId)
        {
            try
            {
                var result = await _discountReasonMasterServices.UpdateDiscountTypeStatus(id, status, userId);
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
