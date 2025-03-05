using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class labReportFooterTextController : BaseController<labReportFooterText>
    {
        private readonly ContextClass db;
        private readonly IlabUniversalMasterServices _labUniversalMasterServices;
        public labReportFooterTextController(ContextClass context, ILogger<BaseController<labReportFooterText>> logger, IlabUniversalMasterServices labUniversalMasterServices) : base(context, logger)
        {
            db = context;
            this._labUniversalMasterServices = labUniversalMasterServices;
        }
        protected override IQueryable<labReportFooterText> DbSet => db.labReportFooterText.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("SaveUpdateFooterText")]
        public async Task<ServiceStatusResponseModel> SaveUpdateFooterText(labReportFooterText FooterText)
        {
            try
            {
                var result = await _labUniversalMasterServices.SaveUpdateFooterText(FooterText);
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

        [HttpPost("UpdateFooterTextStatus")]
        public async Task<ServiceStatusResponseModel> UpdateFooterTextStatus(int id, byte status, int userId)
        {
            try
            {
                var result = await _labUniversalMasterServices.UpdateFooterTextStatus(id, status, userId);
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

        [HttpGet("GetFooterText")]
        public async Task<ServiceStatusResponseModel> GetFooterText()
        {
            try
            {
                var result = await _labUniversalMasterServices.GetFooterText();
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
