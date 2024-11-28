using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Model.Transaction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.TransactionController
{
    [Route("api/[controller]")]
    [ApiController]
    public class tnx_InvestigationRemarksController : BaseController<tnx_InvestigationRemarks>
    {


        private readonly ContextClass db;

        public tnx_InvestigationRemarksController(ContextClass context, ILogger<BaseController<tnx_InvestigationRemarks>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<tnx_InvestigationRemarks> DbSet => db.tnx_InvestigationRemarks.AsNoTracking().OrderBy(o => o.id);
    }
}
