using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class labReportHeaderController : BaseController<labReportHeader>
    {
        private readonly ContextClass db;

        public labReportHeaderController(ContextClass context, ILogger<BaseController<labReportHeader>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<labReportHeader> DbSet => db.labReportHeader.AsNoTracking().OrderBy(o => o.id);
    }
}
