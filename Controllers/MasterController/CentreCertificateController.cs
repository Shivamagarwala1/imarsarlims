using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class CentreCertificateController : BaseController<CentreCertificate>
    {
        private readonly ContextClass db;
        private readonly ICentreCertificateServices _CentreCertificateServices;

        public CentreCertificateController(ContextClass context, ILogger<BaseController<CentreCertificate>> logger, ICentreCertificateServices centreCertificateServices) : base(context, logger)
        {
            db = context;
            this._CentreCertificateServices = centreCertificateServices;
        }
        protected override IQueryable<CentreCertificate> DbSet => db.CentreCertificate.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("GenerateCertificate")]
        public async Task<ServiceStatusResponseModel> GenerateCertificate(CentreCertificate Certificate)
        {
            try
            {
                var result = await _CentreCertificateServices.GenerateCertificate(Certificate);
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
        [HttpPost("UploadCertificate")]
        public async Task<ServiceStatusResponseModel> UploadCertificate(int CentreId, string Certificate)
        {
            try
            {
                var result = await _CentreCertificateServices.UploadCertificate(CentreId,  Certificate);
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
        [HttpPost("UploadAggreement")]
        public async Task<ServiceStatusResponseModel> UploadAggreement(int CentreId, string AggrimenstDocumnet)
        {
            try
            {
                var result = await _CentreCertificateServices.UploadAggreement(CentreId, AggrimenstDocumnet);
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

        [HttpPost("UploadDocument")]

        public async Task<ServiceStatusResponseModel> UploadDocument(IFormFile Document)
        {
            try
            {
                var result = await _CentreCertificateServices.UploadDocument(Document);
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

        [HttpGet("GetAggreement")]

        public async Task<ServiceStatusResponseModel> GetAggreement(int centreId)
        {
            try
            {
                var result = await _CentreCertificateServices.GetAggreement(centreId);
                return result;
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel { Success = false, Message = ex.Message };
            }
        }

        [HttpGet("DownloadCertificate")]

        public IActionResult DownloadCertificate(int Id)
        {
            try
            {
                var result =  _CentreCertificateServices.DownloadCertificate(Id);
                
                MemoryStream ms = new MemoryStream(result);
                return new FileStreamResult(ms, "application/pdf")
                {
                    FileDownloadName = "Certificate.pdf"
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
