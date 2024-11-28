using iMARSARLIMS.Model.Transaction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.TransactionController
{

    [Route("api/[controller]")]
        [ApiController]

        public class tnx_ObservationsController : BaseController<tnx_Observations>
        {
            private readonly ContextClass db;

            public tnx_ObservationsController(ContextClass context, ILogger<BaseController<tnx_Observations>> logger) : base(context, logger)
            {
                db = context;
            }
            protected override IQueryable<tnx_Observations> DbSet => db.tnx_Observations.AsNoTracking().OrderBy(o => o.id);
        }
}
