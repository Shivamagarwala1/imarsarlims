using iMARSARLIMS.Interface.Store;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Model.Store;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.StoreController
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndentController : BaseController<Indent>
    {
        private readonly ContextClass db;
        private readonly IindentServices _indentServices;

        public IndentController(ContextClass context, ILogger<BaseController<Indent>> logger, IindentServices indentServices) : base(context, logger)
        {
            db = context;
            this._indentServices = indentServices;
        }
        protected override IQueryable<Indent> DbSet => db.Indent.AsNoTracking().OrderBy(o => o.indentId);

        [HttpPost("CreateIndent")]
        public async Task<ServiceStatusResponseModel> CreateIndent(Indent indentdata)
        {
            try
            {
                var result = await _indentServices.CreateIndent(indentdata);
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


        [HttpGet("GetIndentDetails")]
        public async Task<ServiceStatusResponseModel> GetIndentDetails(int roleId, int empId, DateTime fromDate, DateTime todate,int UserId)
        {
            try
            {
                var result = await _indentServices.GetIndentDetails(roleId, empId,fromDate,todate,UserId);
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
        [HttpPost("Approveindent")]
        public async Task<ServiceStatusResponseModel> Approveindent(List<indentApprovatmodel> approvaldata)
        {
            try
            {
                var result = await _indentServices.Approveindent(approvaldata);
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
        [HttpGet("GetDetail")]
        public async Task<ServiceStatusResponseModel> GetDetail(int indentId)
        {
            try
            {
                var result = await _indentServices.GetDetail(indentId);
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

        [HttpPost("RejectIndent")]
        public async Task<ServiceStatusResponseModel> RejectIndent(int indentId,int UserId,string rejectionReason)
        {
            try
            {
                var result = await _indentServices.RejectIndent(indentId,UserId,rejectionReason);
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
