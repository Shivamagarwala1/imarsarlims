using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class LegendColorMasterController : BaseController<LegendColorMaster>
    {
        private readonly ContextClass db;

        public LegendColorMasterController(ContextClass context, ILogger<BaseController<LegendColorMaster>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<LegendColorMaster> DbSet => db.LegendColorMaster.AsNoTracking().OrderBy(o => o.id);
    }
}
