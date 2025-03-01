using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.TransactionController
{
    [Route("api/[controller]")]
    [ApiController]
    public class tnx_Observations_HistoController  : BaseController<tnx_Observations_Histo> 
    {
        private readonly ContextClass db;
        private readonly Ihistoreportservices _historeportservices;

        public tnx_Observations_HistoController(ContextClass context, ILogger<BaseController<tnx_Observations_Histo>> logger, Ihistoreportservices historeportservices) : base(context, logger)
        {
            db = context;
            this._historeportservices= historeportservices;
        }
        protected override IQueryable<tnx_Observations_Histo> DbSet => db.tnx_Observations_Histo.AsNoTracking().OrderBy(o => o.histoObservationId);

        [HttpGet("GetHistoReport")]
        public IActionResult GetHistoReport(string testId)
        {
            try
            {
                var result = _historeportservices.GetHistoReport(testId);
                if (result == null || result.Length == 0)
                {
                    return NotFound($"test ID '{testId}' not found or no report data available.");
                }
                MemoryStream ms = new MemoryStream(result);
                return new FileStreamResult(ms, "application/pdf")
                {
                    FileDownloadName = $"Historeport_{testId}.pdf"
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing the request: {ex.Message}");
            }
        }
        [HttpGet("GetMicroReport")]
        public IActionResult GetMicroReport(string testId)
        {
            try
            {
                var result = _historeportservices.GetMicroReport(testId);
                if (result == null || result.Length == 0)
                {
                    return NotFound($"test ID '{testId}' not found or no report data available.");
                }
                MemoryStream ms = new MemoryStream(result);
                return new FileStreamResult(ms, "application/pdf")
                {
                    FileDownloadName = $"Microreport_{testId}.pdf"
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing the request: {ex.Message}");
            }
        }
    }
}
