using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class machineMasterController : BaseController<machineMaster>
    {
        private readonly ContextClass db;
        private readonly ImachineMasterServices _machineMasterServices;

        public machineMasterController(ContextClass context, ILogger<BaseController<machineMaster>> logger, ImachineMasterServices machineMasterServices) : base(context, logger)
        {
            db = context;
            this._machineMasterServices = machineMasterServices;
        }
        protected override IQueryable<machineMaster> DbSet => db.machineMaster.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("SaveMachineMaster")]
        public async Task<ServiceStatusResponseModel> SaveMachineMaster(machineMaster Machine)
        {
            try
            {
                var result = await _machineMasterServices.SaveMachineMaster(Machine);
                return result;
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = ex.InnerException?.Message ?? "An error occurred."
                };
            }
        }
    }
}
