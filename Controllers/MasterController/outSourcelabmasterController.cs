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
    public class outSourcelabmasterController : BaseController<outSourcelabmaster>
    {
        private readonly ContextClass db;
        private readonly IlabUniversalMasterServices _labUniversalMasterServices;

        public outSourcelabmasterController(ContextClass context, ILogger<BaseController<outSourcelabmaster>> logger, IlabUniversalMasterServices labUniversalMasterServices) : base(context, logger)
        {
            db = context;
            this._labUniversalMasterServices= labUniversalMasterServices;
        }
        protected override IQueryable<outSourcelabmaster> DbSet => db.outSourcelabmaster.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("SaveUpdateOutsourceLab")]
        public async Task<ServiceStatusResponseModel> SaveUpdateOutsourceLab(outSourcelabmaster OutsourceLab)
        {
            try
            {
                var result = await _labUniversalMasterServices.SaveUpdateOutsourceLab(OutsourceLab);
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

        [HttpPost("UpdateOutsourceLabStatus")]
        public async Task<ServiceStatusResponseModel> UpdateOutsourceLabStatus(int id, byte status, int userId)
        {
            try
            {
                var result = await _labUniversalMasterServices.UpdateOutsourceLabStatus(id, status, userId);
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
