using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.TransactionController
{
    [Route("api/[controller]")]
    [ApiController]
    public class tnx_OutsourceDetailController : BaseController<tnx_OutsourceDetail>
    {
        private readonly ContextClass db;
        private readonly Itnx_OutsourceDetailServices _tnx_OutsourceDetailServices;

        public tnx_OutsourceDetailController(ContextClass context, ILogger<BaseController<tnx_OutsourceDetail>> logger, Itnx_OutsourceDetailServices tnx_OutsourceDetailServices) : base(context, logger)
        {
            db = context;
            this._tnx_OutsourceDetailServices = tnx_OutsourceDetailServices;
        }
        protected override IQueryable<tnx_OutsourceDetail> DbSet => db.tnx_OutsourceDetail.AsNoTracking().OrderBy(o => o.id);

        [HttpGet("GetOutsourceData")]
        public async Task<ServiceStatusResponseModel> GetOutsourceData(DateTime FromDate, DateTime Todate,string SearchValue)
        {
            try
            {
                var result = await _tnx_OutsourceDetailServices.GetOutsourceData(FromDate,Todate,SearchValue);
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
        [HttpPost("SaveUpdateOutsourceData")]
        public async Task<ServiceStatusResponseModel> SaveUpdateOutsourceData(List<tnx_OutsourceDetail> outSourcedata)
        {
            try
            {
                var result = await _tnx_OutsourceDetailServices.SaveUpdateOutsourceData(outSourcedata);
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

        [HttpGet("GetOutsourceReportExcel")]
        public IActionResult GetOutsourceReportExcel(DateTime FromDate, DateTime Todate)
        {
            try
            {
                var result =  _tnx_OutsourceDetailServices.GetOutsourceReportExcel(FromDate, Todate);
                return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "OutsourceReport.xlsx");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
