using iMARSARLIMS.Interface;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using iMARSARLIMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using machineRerunTestDetail = iMARSARLIMS.Model.Transaction.machineRerunTestDetail;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class machineRerunTestDetailController : BaseController<machineRerunTestDetail>
    {
        private readonly ContextClass db;
        private readonly ImachineRerunTestDetailServices _machineRerunTestDetailServices;
        public machineRerunTestDetailController(ContextClass context, ILogger<BaseController<machineRerunTestDetail>> logger, ImachineRerunTestDetailServices machineRerunTestDetailServices) : base(context, logger)
        {
            db = context;
            this._machineRerunTestDetailServices = machineRerunTestDetailServices;
        }
        protected override IQueryable<machineRerunTestDetail> DbSet => db.machineRerunTestDetail.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("SaveRerun")]
        public async Task<ServiceStatusResponseModel> SaveRerun(List<machineRerunTestDetail> RerunDetail)
        {
            try
            {
                var result = await _machineRerunTestDetailServices.SaveRerun(RerunDetail);
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
