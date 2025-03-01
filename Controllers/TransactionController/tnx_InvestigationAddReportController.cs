
using iMARSARLIMS.Model.Transaction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.TransactionController
{
    [Route("api/[controller]")]
    [ApiController]
    public class tnx_InvestigationAddReportController : BaseController<tnx_InvestigationAddReport>
    {
        private readonly ContextClass db;
        public tnx_InvestigationAddReportController(ContextClass context, ILogger<BaseController<tnx_InvestigationAddReport>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<tnx_InvestigationAddReport> DbSet => db.tnx_InvestigationAddReport.AsNoTracking().OrderBy(o => o.id);

    }
}
