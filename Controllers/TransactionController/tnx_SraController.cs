using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Model.Transaction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.TransactionController
{
    [Route("api/[controller]")]
    [ApiController]
    public class tnx_SraController : BaseController<tnx_Sra>
    {
        private readonly ContextClass db;

        public tnx_SraController(ContextClass context, ILogger<BaseController<tnx_Sra>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<tnx_Sra> DbSet => db.tnx_Sra.AsNoTracking().OrderBy(o => o.id);

    }
}
