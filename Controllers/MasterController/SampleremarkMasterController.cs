using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleremarkMasterController : BaseController<SampleremarkMaster>
    {
        private readonly ContextClass db;
        private readonly IlabUniversalMasterServices _labUniversalMasterServices;
        public SampleremarkMasterController(ContextClass context, ILogger<BaseController<SampleremarkMaster>> logger, IlabUniversalMasterServices labUniversalMasterServices) : base(context, logger)
        {
            db = context;
            this._labUniversalMasterServices = labUniversalMasterServices;
        }
        protected override IQueryable<SampleremarkMaster> DbSet => db.SampleremarkMaster.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("SaveUpdatesampleRemark")]
        public async Task<ServiceStatusResponseModel> SaveUpdatesampleRemark(SampleremarkMaster SampleRemark)
        {
            try
            {
                var result = await _labUniversalMasterServices.SaveUpdatesampleRemark(SampleRemark);
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

        [HttpPost("UpdateSampleRemarkStatus")]
        public async Task<ServiceStatusResponseModel> UpdateSampleRemarkStatus(int id, byte status, int userId)
        {
            try
            {
                var result = await _labUniversalMasterServices.UpdateSampleRemarkStatus(id, status, userId);
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
