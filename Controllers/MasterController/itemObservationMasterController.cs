using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class itemObservationMasterController : BaseController<itemObservationMaster>
    {
        private readonly ContextClass db;

        public itemObservationMasterController(ContextClass context, ILogger<BaseController<itemObservationMaster>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<itemObservationMaster> DbSet => db.itemObservationMaster.AsNoTracking().OrderBy(o => o.id);
    }
}
