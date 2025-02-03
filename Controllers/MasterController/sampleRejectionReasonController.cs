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
    public class sampleRejectionReasonController : BaseController<sampleRejectionReason>
    {
        private readonly ContextClass db;
        private readonly IsampleRejectionReasonServices _sampleRejectionReasonServices;

        public sampleRejectionReasonController(ContextClass context, ILogger<BaseController<sampleRejectionReason>> logger, IsampleRejectionReasonServices sampleRejectionReasonServices) : base(context, logger)
        {
            db = context;
            this._sampleRejectionReasonServices = sampleRejectionReasonServices;
        }
        protected override IQueryable<sampleRejectionReason> DbSet => db.sampleRejectionReason.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("SaveUpdateRejectionReason")]
        public async Task<ServiceStatusResponseModel> SaveUpdateRejectionReason(sampleRejectionReason RejectionReason)
        {
            try
            {
                var result = await _sampleRejectionReasonServices.SaveUpdateRejectionReason(RejectionReason);
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

        [HttpPost("UpdateRejectionReasonStatus")]
        public async Task<ServiceStatusResponseModel> UpdateRejectionReasonStatus(int id, byte status, int Userid)
        {
            try
            {
                var result = await _sampleRejectionReasonServices.UpdateRejectionReasonStatus(id, status, Userid);
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
