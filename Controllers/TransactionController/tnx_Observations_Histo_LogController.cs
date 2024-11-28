using iMARSARLIMS.Model.Transaction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.TransactionController
{
    [Route("api/[controller]")]
    [ApiController]
    public class tnx_Observations_Histo_LogController : BaseController<tnx_Observations_Histo_Log>
    {
        private readonly ContextClass db;

        public tnx_Observations_Histo_LogController(ContextClass context, ILogger<BaseController<tnx_Observations_Histo_Log>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<tnx_Observations_Histo_Log> DbSet => db.tnx_Observations_Histo_Log.AsNoTracking().OrderBy(o => o.histoObservationId);
    }
}
