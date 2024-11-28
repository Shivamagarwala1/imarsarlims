using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class sampletype_masterController : BaseController<sampletype_master>
    {
        private readonly ContextClass db;

        public sampletype_masterController(ContextClass context, ILogger<BaseController<sampletype_master>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<sampletype_master> DbSet => db.sampletype_master.AsNoTracking().OrderBy(o => o.id);

    }
}
