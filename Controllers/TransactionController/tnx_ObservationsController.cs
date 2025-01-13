using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.TransactionController
{

    [Route("api/[controller]")]
    [ApiController]

    public class tnx_ObservationsController : BaseController<tnx_Observations>
    {
        private readonly ContextClass db;
        private readonly IPatientReportServices _PatientReportServices;

        public tnx_ObservationsController(ContextClass context, ILogger<BaseController<tnx_Observations>> logger, IPatientReportServices PatientReportServices) : base(context, logger)
        {
            db = context;
            this._PatientReportServices = PatientReportServices;
        }
        protected override IQueryable<tnx_Observations> DbSet => db.tnx_Observations.AsNoTracking().OrderBy(o => o.id);

        [HttpGet("GetPatientReportType1")]
        public IActionResult GetPatientReportType1(string TestId)
        {
            try
            {
                var result = _PatientReportServices.GetPatientReportType1(TestId);
                MemoryStream ms = new MemoryStream(result);

                return new FileStreamResult(ms, "application/pdf")
                {
                    FileDownloadName = "PateintReport.pdf"
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        [HttpGet("GetPatientReportType2")]
        public IActionResult GetPatientReportType2(string TestId)
        {
            try
            {
                var result = _PatientReportServices.GetPatientReportType2(TestId);
                MemoryStream ms = new MemoryStream(result);

                return new FileStreamResult(ms, "application/pdf")
                {
                    FileDownloadName = "PateintReport.pdf"
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        [HttpGet("GetPatientReportType3")]
        public IActionResult GetPatientReportType3(string TestId)
        {
            try
            {
                var result = _PatientReportServices.GetPatientReportType3(TestId);
                MemoryStream ms = new MemoryStream(result);

                return new FileStreamResult(ms, "application/pdf")
                {
                    FileDownloadName = "PateintReport.pdf"
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
    }
}
