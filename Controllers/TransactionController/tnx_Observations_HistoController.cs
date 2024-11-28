using iMARSARLIMS.Model.Transaction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.TransactionController
{
    [Route("api/[controller]")]
    [ApiController]
    public class tnx_Observations_HistoController  : BaseController<tnx_Observations_Histo> 
    {
        private readonly ContextClass db;

        public tnx_Observations_HistoController(ContextClass context, ILogger<BaseController<tnx_Observations_Histo>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<tnx_Observations_Histo> DbSet => db.tnx_Observations_Histo.AsNoTracking().OrderBy(o => o.histoObservationId);

    }
}
