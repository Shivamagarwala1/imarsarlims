using iMARSARLIMS.Model.Transaction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.TransactionController
{
    [Route("api/[controller]")]
    [ApiController]
    public class tnx_Allergy_ResultEntryController : BaseController<tnx_Allergy_ResultEntry>
    {
        private readonly ContextClass db;

        public tnx_Allergy_ResultEntryController(ContextClass context, ILogger<BaseController<tnx_Allergy_ResultEntry>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<tnx_Allergy_ResultEntry> DbSet => db.tnx_Allergy_ResultEntry.AsNoTracking().OrderBy(o => o.id);
    }
}
