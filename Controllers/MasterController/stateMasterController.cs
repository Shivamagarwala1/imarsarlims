using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class stateMasterController : BaseController<stateMaster>
    {
        private readonly ContextClass db;
        private readonly ILocationsServices _LocationsServices;

        public stateMasterController(ContextClass context, ILogger<BaseController<stateMaster>> logger, ILocationsServices LocationsServices) : base(context, logger)
        {
            db = context;
            this._LocationsServices = LocationsServices;
        }
        protected override IQueryable<stateMaster> DbSet => db.stateMaster.AsNoTracking().OrderBy(o => o.id);
        [HttpPost("SaveState")]
        public async Task<ServiceStatusResponseModel> SaveState(stateMaster State)
        {
            try
            {
                var result = await _LocationsServices.SaveState(State);
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
