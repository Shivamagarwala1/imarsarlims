using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using iMARSARLIMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.TransactionController
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
            _tnxInvestigationAttachmentService = tnxInvestigationAttachmentService;
        }
        protected override IQueryable<tnx_InvestigationAttchment> DbSet => db.tnx_InvestigationAttchment.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("AddReport")]
        public async Task<ServiceStatusResponseModel> AddReport(tnx_InvestigationAddReport Report)
        {
            try
            {
                var result = await _tnxInvestigationAttachmentService.AddReport(Report);
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
        [HttpPost("AddAttchment")]
        public async Task<ServiceStatusResponseModel> AddAttchment(tnx_InvestigationAttchment attchment)
        {
            try
            {
                var result = await _tnxInvestigationAttachmentService.AddAttchment(attchment);
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

        [HttpGet("ViewDocument")]
        public async Task<IActionResult> ViewDocument(string Documentpath)
        {
            try
            {
                if (string.IsNullOrEmpty(Documentpath) || !System.IO.File.Exists(Documentpath))
                {
                    return NotFound("Document not found.");
                }

                var fileBytes = await System.IO.File.ReadAllBytesAsync(Documentpath);

                return File(fileBytes, "application/pdf", Path.GetFileName(Documentpath));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
