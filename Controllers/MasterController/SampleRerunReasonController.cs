using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleRerunReasonController : BaseController<SampleRerunReason>
    {
        private readonly ContextClass db;
        private readonly IlabUniversalMasterServices _labUniversalMasterServices;
        public SampleRerunReasonController(ContextClass context, ILogger<BaseController<SampleRerunReason>> logger, IlabUniversalMasterServices labUniversalMasterServices) : base(context, logger)
        {
            db = context;
            this._labUniversalMasterServices = labUniversalMasterServices;
        }
        protected override IQueryable<SampleRerunReason> DbSet => db.SampleRerunReason.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("SaveUpdateSampleRerunReason")]
        public async Task<ServiceStatusResponseModel> SaveUpdateSampleRerunReason(SampleRerunReason SampleReason)
        {
            try
            {
                var result = await _labUniversalMasterServices.SaveUpdateSampleRerunReason(SampleReason);
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

        [HttpPost("UpdateSampleReasonStatus")]
        public async Task<ServiceStatusResponseModel> UpdateSampleReasonStatus(int id, byte status, int userId)
        {
            try
            {
                var result = await _labUniversalMasterServices.UpdateSampleReasonStatus(id, status, userId);
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
