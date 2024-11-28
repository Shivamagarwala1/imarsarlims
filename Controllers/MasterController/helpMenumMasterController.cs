using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class helpMenumMasterController : BaseController<helpMenuMaster>
    {
        private readonly ContextClass db;

        public helpMenumMasterController(ContextClass context, ILogger<BaseController<helpMenuMaster>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<helpMenuMaster> DbSet => db.helpMenuMaster.AsNoTracking().OrderBy(o => o.id);
    }
}
