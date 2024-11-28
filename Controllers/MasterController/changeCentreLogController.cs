using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class changeCentreLogController : BaseController<changeCentreLog>
    {
        private readonly ContextClass db;

        public changeCentreLogController(ContextClass context, ILogger<BaseController<changeCentreLog>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<changeCentreLog> DbSet => db.changeCentreLog.AsNoTracking().OrderBy(o => o.id);
    }
}
