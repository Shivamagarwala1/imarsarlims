using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class testInterpretationLogController : BaseController<testInterpretationLog>
    {
        private readonly ContextClass db;
        public testInterpretationLogController(ContextClass context, ILogger<BaseController<testInterpretationLog>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<testInterpretationLog> DbSet => db.testInterpretationLog.AsNoTracking().OrderBy(o => o.id);

    }
}
