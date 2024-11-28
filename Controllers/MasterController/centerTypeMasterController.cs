using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class centerTypeMasterController : BaseController<centerTypeMaster>
    {
        private readonly ContextClass db;

        public centerTypeMasterController(ContextClass context, ILogger<BaseController<centerTypeMaster>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<centerTypeMaster> DbSet => db.centerTypeMaster.AsNoTracking().OrderBy(o => o.id);
    }
}
