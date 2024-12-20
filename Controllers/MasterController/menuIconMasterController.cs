using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class menuIconMasterController : BaseController<menuIconMaster>
    {
        public readonly ContextClass db;
        public menuIconMasterController(ContextClass context, ILogger<BaseController<menuIconMaster>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<menuIconMaster> DbSet => db.menuIconMaster.AsNoTracking().OrderBy(o => o.id);
    }
}
