using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemObservationMappingController : BaseController<ItemObservationMapping>
    {
        private readonly ContextClass db;
        private readonly IitemObservationMappingServices _itemObservationMappingServices;

        public ItemObservationMappingController(ContextClass context, ILogger<BaseController<ItemObservationMapping>> logger, IitemObservationMappingServices itemObservationMappingServices) : base(context, logger)
        {
            db = context;
            this._itemObservationMappingServices= itemObservationMappingServices;
        }
        protected override IQueryable<ItemObservationMapping> DbSet => db.ItemObservationMapping.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("SaveObservationMapping")]
        public async Task<ActionResult<ServiceStatusResponseModel>> SaveObservationMapping(List<ItemObservationMapping> itemObservationMapping)
        {
            try
            {
                var result = await _itemObservationMappingServices.SaveObservationMapping(itemObservationMapping);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
