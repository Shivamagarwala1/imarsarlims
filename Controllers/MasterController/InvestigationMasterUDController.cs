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
    public class InvestigationMasterUDController : BaseController<InvestigationMasterUD>
    {
        private readonly ContextClass db;
        private readonly IinvestigationUDServices _investigationUDServices;
        public InvestigationMasterUDController(ContextClass context, ILogger<BaseController<InvestigationMasterUD>> logger, IinvestigationUDServices investigationUDServices) : base(context, logger)
        {
            db = context;
            this._investigationUDServices = investigationUDServices;
        }
        protected override IQueryable<InvestigationMasterUD> DbSet => db.InvestigationMasterUD.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("UpdateInvestigationFormat")]
        public async Task<ServiceStatusResponseModel> UpdateInvestigationFormat(InvestigationMasterUD FormatData)
        {
            try
            {
                var result = await _investigationUDServices.UpdateInvestigationFormat(FormatData);
                return result;
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = ex.Message ?? "An error occurred."
                };
            }
        }
        [HttpPost("RemoveInvestigationFormat")]
        public async Task<ServiceStatusResponseModel> RemoveInvestigationFormat(int Id)
        {
            try
            {
                var result = await _investigationUDServices.RemoveInvestigationFormat(Id);
                return result;
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = ex.Message ?? "An error occurred."
                };
            }
        }
        [HttpPost("UploadDocument")]

        public async Task<ServiceStatusResponseModel> UploadDocument(IFormFile Document)
        {
            try
            {
                var result = await _investigationUDServices.UploadDocument(Document);
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
