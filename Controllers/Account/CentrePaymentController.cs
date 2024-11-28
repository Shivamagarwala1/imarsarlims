using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Account;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using iMARSARLIMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class CentrePaymentController : BaseController<CentrePayment>
    {
        private readonly ContextClass db;
        private readonly ICentrePaymentServices _CentrePaymentServices;


        public CentrePaymentController(ContextClass context, ILogger<BaseController<CentrePayment>> logger, ICentrePaymentServices CentrePaymentServices) : base(context, logger)
        {
            db = context;
            this._CentrePaymentServices = CentrePaymentServices;
        }
        protected override IQueryable<CentrePayment> DbSet => db.CentrePayment.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("SubmitPayment")]
        public async Task<ServiceStatusResponseModel> SubmitPayment(CentrePaymentRequestModel centrePayments)
        {
            try
            {
                var result = await _CentrePaymentServices.SubmitPayment(centrePayments);
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
        [HttpPost("PaymentApproveReject")]
        public async Task<ServiceStatusResponseModel> PaymentApproveReject(CentrePaymetVerificationRequestModel CentrePaymetVerificationRequest)
        {
            try
            {
                var result = await _CentrePaymentServices.PaymentApproveReject(CentrePaymetVerificationRequest);
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
