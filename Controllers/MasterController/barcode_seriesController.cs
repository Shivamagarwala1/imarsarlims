using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class barcode_seriesController : BaseController<barcode_series>
    {
        private readonly ContextClass db;

        public barcode_seriesController(ContextClass context, ILogger<BaseController<barcode_series>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<barcode_series> DbSet => db.barcode_series.AsNoTracking().OrderBy(o => o.id);
    }
}
