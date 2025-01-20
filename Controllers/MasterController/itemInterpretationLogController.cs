using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class itemInterpretationLogController : BaseController<itemInterpretationLog>
    {
        private readonly ContextClass db;
        public itemInterpretationLogController(ContextClass context, ILogger<BaseController<itemInterpretationLog>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<itemInterpretationLog> DbSet => db.itemInterpretationLog.AsNoTracking().OrderBy(o => o.id);

    }
}
