using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class cityMasterController : BaseController<cityMaster>
    {
        private readonly ContextClass db;

        public cityMasterController(ContextClass context, ILogger<BaseController<cityMaster>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<cityMaster> DbSet => db.cityMaster.AsNoTracking().OrderBy(o => o.id);
    }
}
