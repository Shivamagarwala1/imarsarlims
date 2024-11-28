using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class observationReferenceRangesController : BaseController<observationReferenceRanges>
    {
        private readonly ContextClass db;
        private readonly IobservationReferenceRangesServices _observationReferenceRangesServices;

        public observationReferenceRangesController(ContextClass context, ILogger<BaseController<observationReferenceRanges>> logger, IobservationReferenceRangesServices observationReferenceRangesServices) : base(context, logger)
        {
            db = context;
            this._observationReferenceRangesServices = observationReferenceRangesServices;
        }
        protected override IQueryable<observationReferenceRanges> DbSet => db.observationReferenceRanges.AsNoTracking().OrderBy(o => o.id);
        
        [HttpPost("SaveUpdateReferenceRange")]
        public async Task<ServiceStatusResponseModel> SaveUpdateReferenceRange(List<observationReferenceRanges> ObservationReferenceRanges)
        {
            try
            {
                var result = await _observationReferenceRangesServices.SaveUpdateReferenceRange(ObservationReferenceRanges);
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
