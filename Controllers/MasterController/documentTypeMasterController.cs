using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class documentTypeMasterController : BaseController<documentTypeMaster>
    {
        private readonly ContextClass db;

        public documentTypeMasterController(ContextClass context, ILogger<BaseController<documentTypeMaster>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<documentTypeMaster> DbSet => db.documentTypeMaster.AsNoTracking().OrderBy(o => o.id);
    }
}
