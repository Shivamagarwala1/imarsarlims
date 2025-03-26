using iMARSARLIMS.Interface;
using iMARSARLIMS.Interface.Sales;
using iMARSARLIMS.Model.Sales;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.Sales
{
    public class SalesEmployeeTaggingController : BaseController<SalesEmployeeTagging>
    {
        private readonly ContextClass db;
        private readonly ISalesEmployeeTaggingService _SalesEmployeeTaggingService;

        public SalesEmployeeTaggingController(ContextClass context, ILogger<BaseController<SalesEmployeeTagging>> logger, ISalesEmployeeTaggingService SalesEmployeeTaggingService) : base(context, logger)
        {
            db = context;
            this._SalesEmployeeTaggingService = SalesEmployeeTaggingService;
        }
        protected override IQueryable<SalesEmployeeTagging> DbSet => db.SalesEmployeeTagging.AsNoTracking().OrderBy(o => o.id);


        [HttpPost("CreateSalestagging")]
        public async Task<ServiceStatusResponseModel> CreateSalestagging(List<SalesEmployeeTagging> SalesTagging)
        {
            try
            {
                var result = await _SalesEmployeeTaggingService.CreateSalestagging(SalesTagging);
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

        [HttpGet("GetSalesTagging")]
        public async Task<ServiceStatusResponseModel> GetSalesTagging(int tagged)
        {
            try
            {
                var result = await _SalesEmployeeTaggingService.GetSalesTagging(tagged);
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

        [HttpGet("RemoveSalesTagging")]
        public async Task<ServiceStatusResponseModel> RemoveSalesTagging(int id)
        {
            try
            {
                var result = await _SalesEmployeeTaggingService.RemoveSalesTagging(id);
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
