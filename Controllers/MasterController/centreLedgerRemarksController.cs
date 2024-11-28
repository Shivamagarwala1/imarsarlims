using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class centreLedgerRemarksController : BaseController<centreLedgerRemarks>
    {
        private readonly ContextClass db;

        public centreLedgerRemarksController(ContextClass context, ILogger<BaseController<centreLedgerRemarks>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<centreLedgerRemarks> DbSet => db.centreLedgerRemarks.AsNoTracking().OrderBy(o => o.id);

    }
}
