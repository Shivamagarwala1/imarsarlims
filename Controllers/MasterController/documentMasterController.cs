using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class documentMasterController : BaseController<documentMaster>
    {
        private readonly ContextClass db;

        public documentMasterController(ContextClass context, ILogger<BaseController<documentMaster>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<documentMaster> DbSet => db.documentMaster.AsNoTracking().OrderBy(o => o.id);
    }
}
