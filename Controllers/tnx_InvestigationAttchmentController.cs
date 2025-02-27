using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using iMARSARLIMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class tnx_InvestigationAttchmentController : BaseController<tnx_InvestigationAttchment>
    {
        private readonly ContextClass db;
        private readonly ItnxInvestigationAttachmentService _tnxInvestigationAttachmentService;
        public tnx_InvestigationAttchmentController(ContextClass context, ILogger<BaseController<tnx_InvestigationAttchment>> logger, ItnxInvestigationAttachmentService tnxInvestigationAttachmentService) : base(context, logger)
        {
            db = context;
            this._tnxInvestigationAttachmentService = tnxInvestigationAttachmentService;
        }
        protected override IQueryable<tnx_InvestigationAttchment> DbSet => db.tnx_InvestigationAttchment.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("AddReport")]
        public async Task<ServiceStatusResponseModel> AddReport(tnx_InvestigationAttchment attchment)
        {
            try
            {
                var result = await _tnxInvestigationAttachmentService.AddReport(attchment);
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
