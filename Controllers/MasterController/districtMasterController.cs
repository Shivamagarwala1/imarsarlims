using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class districtMasterController : BaseController<districtMaster>
    {
        private readonly ContextClass db;

        private readonly ILocationsServices _LocationsServices;

        public districtMasterController(ContextClass context, ILogger<BaseController<districtMaster>> logger, ILocationsServices LocationsServices) : base(context, logger)
        {
            db = context;
            this._LocationsServices = LocationsServices;
        }
        protected override IQueryable<districtMaster> DbSet => db.districtMaster.AsNoTracking().OrderBy(o => o.id);
        [HttpPost("SaveDistrict")]
        public async Task<ServiceStatusResponseModel> SaveDistrict(districtMaster District)
        {
            try
            {
                var result = await _LocationsServices.SaveDistrict(District);
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
