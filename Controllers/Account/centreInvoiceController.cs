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
        [HttpPost("SearchInvoiceData")]
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


        [HttpPost("GetLastInvoiceData")]
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

        [HttpPost("GetInvoices")]
        public Task<ServiceStatusResponseModel> GetInvoices(DateTime FromDate, DateTime Todate, List<int> CentreId)
        {
            try
            {
                var result = _centreInvoiceServices.GetInvoices(FromDate, Todate, CentreId);
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

        [HttpGet("PrintInvoice")]
        public IActionResult PrintInvoice(string InvoiceNo)
        {
            try
            {
                var result = _centreInvoiceServices.PrintInvoice(InvoiceNo);
                MemoryStream ms = new MemoryStream(result);

                return new FileStreamResult(ms, "application/pdf")
                {
                    FileDownloadName = "Invoice.pdf"
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("PrintInvoiceData")]
        public IActionResult PrintInvoiceData(string InvoiceNo)
        {
            try
            {
                var result = _centreInvoiceServices.PrintInvoiceData(InvoiceNo);
                MemoryStream ms = new MemoryStream(result);

                return new FileStreamResult(ms, "application/pdf")
                {
                    FileDownloadName = "InvoiceData.pdf"
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("PrintInvoiceDataExcel")]
        public IActionResult PrintInvoiceDataExcel(string InvoiceNo)
        {
            try
            {
                var result = _centreInvoiceServices.PrintInvoiceDataExcel(InvoiceNo);
                return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "InvoiceDetail.xlsx");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequest orderRequest)
        {
            if (orderRequest == null)
            {
                return BadRequest("Invalid order request");
            }

            var client = _razorpayService.CreateRazorpayClient();
            var options = new Dictionary<string, object>
    {
        { "amount", orderRequest.Amount * 100 }, // Amount in paise
        { "currency", "INR" },
        { "receipt", orderRequest.Receipt },
        { "payment_capture", 1 }
    };

            var order = client.Order.Create(options);

            await using var transaction = await db.Database.BeginTransactionAsync();
            try
            {
                var data = new razorPayOrderRequest
                {
                    Amount = orderRequest.Amount,
                    Receipt = orderRequest.Receipt,
                    workorderId = orderRequest.workorderId,
                    centreID = orderRequest.centreID,
                    payType = orderRequest.payType,
                    status = 0,
                    paymentId= "",
                    Requestdate = DateTime.UtcNow,
                    Orderid = order["id"].ToString() // Save Razorpay Order ID
                };

                db.razorPayOrderRequest.Add(data);
                await db.SaveChangesAsync(); // Save changes before committing the transaction

                await transaction.CommitAsync();

                return Ok(new { orderId = order["id"] });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new { error = ex.Message });
            }
        }
        public class OrderRequest
        {
            public decimal Amount { get; set; }
            public string Receipt { get; set; }
            public string workorderId { get; set; }
            public int centreID { get; set; }
            public string payType { get; set; }

        }

        [HttpPost("verify-payment")]
        public async Task<IActionResult> VerifyPayment([FromBody] PaymentVerificationRequest verificationRequest)
        {
            var client = _razorpayService.CreateRazorpayClient();
            var attributes = new Dictionary<string, string>
            {
                { "razorpay_payment_id", verificationRequest.PaymentId }, // pay_Q4G7e3e949pwtR
                { "razorpay_order_id", verificationRequest.OrderId }, //order_Q4G7ISJSZgYykF
                { "razorpay_signature", verificationRequest.Signature } //26224dfda977e4e48353a7899878ea10d78c1715b5a4010401200499a8e27378
            };
            await using var transaction = await db.Database.BeginTransactionAsync();
            try
            {
               
                Razorpay.Api.Utils.verifyPaymentSignature(attributes);
                var data = db.razorPayOrderRequest.Where(r=>r.Orderid== verificationRequest.OrderId).FirstOrDefault();
                if (data != null)
                {
                    data.status = 1;
                    data.paymentId = verificationRequest.PaymentId;
                    db.razorPayOrderRequest.Update(data);
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                return Ok(new { status = "success" });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(ex.Message);
            }
        }

        public class PaymentVerificationRequest
        {
            public string PaymentId { get; set; }
            public string OrderId { get; set; }
            public string Signature { get; set; }
        }
    }
}
