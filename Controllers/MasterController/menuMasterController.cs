using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class menuMasterController : BaseController<menuMaster>
    {
        private readonly ContextClass db;

        public menuMasterController(ContextClass context, ILogger<BaseController<menuMaster>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<menuMaster> DbSet => db.menuMaster.AsNoTracking().OrderBy(o => o.id);
    }
}
