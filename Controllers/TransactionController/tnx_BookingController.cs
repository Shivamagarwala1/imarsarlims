using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.transactionController
{

    [Route("api/[controller]")]
    [ApiController]
    public class tnx_BookingController : BaseController<tnx_Booking>
    {
        private readonly ContextClass db;
        private readonly ItnxBookingServices _tnxBookingServices;

        public tnx_BookingController(ContextClass context, ILogger<BaseController<tnx_Booking>> logger, ItnxBookingServices tnxBookingServices) : base(context, logger)
        {
            db = context;
            this._tnxBookingServices = tnxBookingServices;
        }
        protected override IQueryable<tnx_Booking> DbSet => db.tnx_Booking.AsNoTracking().OrderBy(o => o.transactionId);

        [HttpPost("GetPatientData")]
        public async Task<ServiceStatusResponseModel> GetPatientData(patientDataRequestModel patientdata)
        {
            try
            {
                var result = await _tnxBookingServices.GetPatientData(patientdata);
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

        [HttpPost("GetPatientDocumnet")]
        public IActionResult GetPatientDocumnet(string workOrderId)
        {
            try
            {
                var result = _tnxBookingServices.GetPatientDocumnet(workOrderId);
                byte[] fileBytes = Convert.FromBase64String(result);

                // Check the type of content based on the first few bytes (signature)
                string contentType = "application/pdf"; // Default assumption is PDF
                if (fileBytes.Length > 4 && fileBytes[0] == 0xFF && fileBytes[1] == 0xD8) // JPEG Check
                {
                    contentType = "image/jpeg";
                }
                else if (fileBytes.Length > 4 && fileBytes[0] == 0x89 && fileBytes[1] == 0x50) // PNG Check
                {
                    contentType = "image/png";
                }
                else if (fileBytes.Length > 4 && fileBytes[0] == 0x47 && fileBytes[1] == 0x49) // GIF Check
                {
                    contentType = "image/gif";
                }

                // Create a MemoryStream with the byte array
                MemoryStream ms = new MemoryStream(fileBytes);

                // Return the file with the appropriate content type
                return new FileStreamResult(ms, contentType)
                {
                    FileDownloadName = contentType.Contains("pdf") ? "ProfitLossReport.pdf" : "ImageFile" // Change the filename based on the content type
                };

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
