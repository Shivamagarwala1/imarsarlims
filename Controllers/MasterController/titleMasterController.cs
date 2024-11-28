using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class titleMasterController : BaseController<titleMaster>
    {
        private readonly ContextClass db;

        public titleMasterController(ContextClass context, ILogger<BaseController<titleMaster>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<titleMaster> DbSet => db.titleMaster.AsNoTracking().OrderBy(o => o.id);
    }
}
