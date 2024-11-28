using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Account;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class centreInvoiceController : BaseController<centreInvoice>
    {
        private readonly ContextClass db;
        private readonly IcentreInvoiceServices _centreInvoiceServices;

        public centreInvoiceController(ContextClass context, ILogger<BaseController<centreInvoice>> logger, IcentreInvoiceServices centreInvoiceServices) : base(context, logger)
        {
            db = context;
            this._centreInvoiceServices=centreInvoiceServices;
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
                var result = _centreInvoiceServices.SearchInvoiceData( FromDate,  Todate, CentreId);
                return Task.FromResult(result);
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
    }
}
