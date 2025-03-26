using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Account;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using iText.Kernel.XMP.Impl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Razorpay.Api;

namespace iMARSARLIMS.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class centreInvoiceController : BaseController<centreInvoice>
    {
        private readonly ContextClass db;
        private readonly IcentreInvoiceServices _centreInvoiceServices;
        private readonly RazorpayService _razorpayService;

        public centreInvoiceController(ContextClass context, ILogger<BaseController<centreInvoice>> logger, IcentreInvoiceServices centreInvoiceServices, RazorpayService razorpayService) : base(context, logger)
        {
            db = context;
            this._centreInvoiceServices = centreInvoiceServices;
            this._razorpayService = razorpayService;
        }
        protected override IQueryable<centreInvoice> DbSet => db.centreInvoice.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("CreateInvoice")]
        public async Task<ServiceStatusResponseModel> CreateInvoice(centreInvoiceRequestModel CentreInvoice)
        {
            try
            {
                var result = await _centreInvoiceServices.CreateInvoice(CentreInvoice);
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
        [HttpGet("SearchInvoiceData")]
        public Task<ServiceStatusResponseModel> SearchInvoiceData(DateTime FromDate, DateTime Todate, List<int> CentreId)
        {
            try
            {
                var result = _centreInvoiceServices.SearchInvoiceData(FromDate, Todate, CentreId);
                return result;
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
        [HttpGet("GetLastInvoiceData")]
        public Task<ServiceStatusResponseModel> GetLastInvoiceData(List<int> CentreId)
        {
            try
            {
                var result = _centreInvoiceServices.GetLastInvoiceData(CentreId);
                return result;
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("create-order")]
        public IActionResult CreateOrder([FromBody] OrderRequest orderRequest)
        {
            var client = _razorpayService.CreateRazorpayClient();
            var options = new Dictionary<string, object>
            {
                { "amount", orderRequest.Amount * 100 }, // Amount in paise
                { "currency", "INR" },
                { "receipt", orderRequest.Receipt },
                { "payment_capture", 1 }
            };
              var order = client.Order.Create(options);
              return Ok(new { orderId = order["id"] });
        }
        public class OrderRequest
        {
            public decimal Amount { get; set; }
            public string Receipt { get; set; }
        }

        [HttpPost("verify-payment")]
        public IActionResult VerifyPayment([FromBody] PaymentVerificationRequest verificationRequest)
        {
            var client = _razorpayService.CreateRazorpayClient();
            var attributes = new Dictionary<string, string>
            {
                { "razorpay_payment_id", verificationRequest.PaymentId }, // pay_Q4G7e3e949pwtR
                { "razorpay_order_id", verificationRequest.OrderId }, //order_Q4G7ISJSZgYykF
                { "razorpay_signature", verificationRequest.Signature } //26224dfda977e4e48353a7899878ea10d78c1715b5a4010401200499a8e27378
            };
            Razorpay.Api.Utils.verifyPaymentSignature(attributes);
            return Ok(new { status = "success" });
        }

        public class PaymentVerificationRequest
        {
            public string PaymentId { get; set; }
            public string OrderId { get; set; }
            public string Signature { get; set; }
        }
    }
}
