using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class doctorReferalMasterController : BaseController<doctorReferalMaster>
    {
        private readonly ContextClass db;
        private readonly IdoctorReferalServices _doctorReferalServices;

        public doctorReferalMasterController(ContextClass context, ILogger<BaseController<doctorReferalMaster>> logger, IdoctorReferalServices doctorReferalServices) : base(context, logger)
        {
            db = context;
            this._doctorReferalServices = doctorReferalServices;
        }
        protected override IQueryable<doctorReferalMaster> DbSet => db.doctorReferalMaster.AsNoTracking().OrderBy(o => o.doctorId);

        [HttpPost("SaveUpdateReferDoctor")]
        public async Task<ServiceStatusResponseModel> SaveUpdateReferDoctor(doctorReferalMaster refDoc)
        {
            try
            {
                var result = await _doctorReferalServices.SaveUpdateReferDoctor(refDoc);
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

        [HttpPost("UpdateReferDoctorStatus")]
        public async Task<ServiceStatusResponseModel> UpdateReferDoctorStatus(int DoctorId, byte status, int UserId)
        {
            try
            {
                var result = await _doctorReferalServices.UpdateReferDoctorStatus(DoctorId, status,UserId);
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

        [HttpGet("ReferDoctorData")]
        public async Task<ServiceStatusResponseModel> ReferDoctorData()
        {
            try
            {
                var result = await _doctorReferalServices.ReferDoctorData();
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
        [HttpGet("DoctorBussinessReport")]
        public async Task<ServiceStatusResponseModel> DoctorBussinessReport(string DoctorId,string centreID, DateTime FromDate,DateTime ToDate )
        {
            try
            {
                var result = await _doctorReferalServices.DoctorBussinessReport( DoctorId,centreID,FromDate,ToDate);
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
        [HttpGet("DoctorBussinessReportSummury")]
        public async Task<ServiceStatusResponseModel> DoctorBussinessReportSummury(string DoctorId, string centreID, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                var result = await _doctorReferalServices.DoctorBussinessReportSummury(DoctorId, centreID, FromDate, ToDate);
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

        [HttpGet("DoctorBussinessReportPdf")]
        public IActionResult DoctorBussinessReportPdf(string DoctorId, string centreID, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                var result =  _doctorReferalServices.DoctorBussinessReportPdf(DoctorId, centreID, FromDate, ToDate);
                MemoryStream ms = new MemoryStream(result);

                return new FileStreamResult(ms, "application/pdf")
                {
                    FileDownloadName = "DoctorBussinessReport.pdf"
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("DoctorBussinessReportSummuryPdf")]
        public IActionResult DoctorBussinessReportSummuryPdf(string DoctorId, string centreID, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                var result = _doctorReferalServices.DoctorBussinessReportSummuryPdf(DoctorId, centreID, FromDate, ToDate);
                MemoryStream ms = new MemoryStream(result);

                return new FileStreamResult(ms, "application/pdf")
                {
                    FileDownloadName = "DoctorBussinessreportsummury.pdf"
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpGet("DoctorBussinessReportExcel")]
        public IActionResult DoctorBussinessReportExcel(string DoctorId, string centreID, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                var result = _doctorReferalServices.DoctorBussinessReportExcel(DoctorId, centreID, FromDate, ToDate);
                return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DoctorBussinessReport.xlsx");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("DoctorBussinessReportSummuryExcel")]
        public IActionResult DoctorBussinessReportSummuryExcel(string DoctorId, string centreID, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                var result = _doctorReferalServices.DoctorBussinessReportSummuryExcel(DoctorId, centreID, FromDate, ToDate);
                return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DoctorBussinessReportSummury.xlsx");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }


    }
}
