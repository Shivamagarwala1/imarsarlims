using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Model.Transaction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.TransactionController
{
    [Route("api/[controller]")]
    [ApiController]
    public class tnx_Observations_Micro_flowcyto_LogController : BaseController<tnx_Observations_Micro_flowcyto_Log>
    {
        private readonly ContextClass db;

        public tnx_Observations_Micro_flowcyto_LogController(ContextClass context, ILogger<BaseController<tnx_Observations_Micro_flowcyto_Log>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<tnx_Observations_Micro_flowcyto_Log> DbSet => db.tnx_Observations_Micro_flowcyto_Log.AsNoTracking().OrderBy(o => o.testId);
    }
}
