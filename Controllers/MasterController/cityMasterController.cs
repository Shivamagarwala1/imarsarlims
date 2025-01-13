using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class cityMasterController : BaseController<cityMaster>
    {
        private readonly ContextClass db;
        private readonly ILocationsServices _LocationsServices;

        public cityMasterController(ContextClass context, ILogger<BaseController<cityMaster>> logger, ILocationsServices LocationsServices) : base(context, logger)
        {
            db = context;
            this._LocationsServices = LocationsServices;
        }
        protected override IQueryable<cityMaster> DbSet => db.cityMaster.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("SaveCity")]
        public async Task<ServiceStatusResponseModel> SaveCity(cityMaster City)
        {
            try
            {
                var result = await _LocationsServices.SaveCity(City);
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
