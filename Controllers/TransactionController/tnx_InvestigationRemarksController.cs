using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.TransactionController
{
    [Route("api/[controller]")]
    [ApiController]
    public class tnx_InvestigationRemarksController : BaseController<tnx_InvestigationRemarks>
    {
        private readonly ContextClass db;
        private readonly ItnxInvestigationRemarksService _tnxInvestigationRemarksService;
        public tnx_InvestigationRemarksController(ContextClass context, ILogger<BaseController<tnx_InvestigationRemarks>> logger, ItnxInvestigationRemarksService tnxInvestigationRemarksService) : base(context, logger)
        {
            db = context;
            this._tnxInvestigationRemarksService = tnxInvestigationRemarksService;
        }
        protected override IQueryable<tnx_InvestigationRemarks> DbSet => db.tnx_InvestigationRemarks.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("AddSampleremark")]
        public async Task<ServiceStatusResponseModel> AddSampleRemark(tnx_InvestigationRemarks remark)
        {
            try
            {
                var result = await _tnxInvestigationRemarksService.AddSampleRemark(remark);
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

        [HttpGet("GetSampleremark")]
        public async Task<ServiceStatusResponseModel> GetSampleremark(int transacctionId,string WorkOrderId, int itemId)
        {
            try
            {
                var result = await _tnxInvestigationRemarksService.GetSampleremark(transacctionId,WorkOrderId,itemId);
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
