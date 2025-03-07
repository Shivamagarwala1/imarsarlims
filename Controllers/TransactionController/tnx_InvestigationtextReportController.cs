using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Transaction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.TransactionController
{
    [Route("api/[controller]")]
    [ApiController]
    public class tnx_InvestigationtextReportController : BaseController<tnx_investigationtext_Report>
    {
        private readonly ContextClass db;
        
        public tnx_InvestigationtextReportController(ContextClass context, ILogger<BaseController<tnx_investigationtext_Report>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<tnx_investigationtext_Report> DbSet => db.tnx_investigationtext_Report.AsNoTracking().OrderBy(o => o.id);

    }
}
