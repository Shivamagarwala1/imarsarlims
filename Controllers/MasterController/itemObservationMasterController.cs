using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class itemObservationMasterController : BaseController<itemObservationMaster>
    {
        private readonly ContextClass db;
        private readonly IObservationMasterServices _ObservationMasterServices;

        public itemObservationMasterController(ContextClass context, ILogger<BaseController<itemObservationMaster>> logger, IObservationMasterServices ObservationMasterServices) : base(context, logger)
        {
            db = context;
            this._ObservationMasterServices = ObservationMasterServices;
        }
        protected override IQueryable<itemObservationMaster> DbSet => db.itemObservationMaster.AsNoTracking().OrderBy(o => o.id);
        [HttpPost("SaveUpdateObservationMater")]
        public async Task<ServiceStatusResponseModel> SaveUpdateObservationMater(itemObservationMaster Observation)
        {
            try
            {
                var result = await _ObservationMasterServices.SaveUpdateObservationMater(Observation);
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
