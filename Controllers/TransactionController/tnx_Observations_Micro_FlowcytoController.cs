using iMARSARLIMS.Model.Transaction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.TransactionController
{

    [Route("api/[controller]")]
    [ApiController]
    public class tnx_Observations_Micro_FlowcytoController : BaseController<tnx_Observations_Micro_Flowcyto>
    {
        private readonly ContextClass db;

        public tnx_Observations_Micro_FlowcytoController(ContextClass context, ILogger<BaseController<tnx_Observations_Micro_Flowcyto>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<tnx_Observations_Micro_Flowcyto> DbSet => db.tnx_Observations_Micro_Flowcyto.AsNoTracking().OrderBy(o => o.id);

    }
    
}
