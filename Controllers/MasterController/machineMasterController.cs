using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class machineMasterController : BaseController<machineMaster>
    {
        private readonly ContextClass db;

        public machineMasterController(ContextClass context, ILogger<BaseController<machineMaster>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<machineMaster> DbSet => db.machineMaster.AsNoTracking().OrderBy(o => o.id);
    }
}
