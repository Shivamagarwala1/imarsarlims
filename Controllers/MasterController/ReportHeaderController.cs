using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Account;
using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportHeaderController : BaseController<ReportHeader>
    {
        private readonly ContextClass db;
        public ReportHeaderController(ContextClass context, ILogger<BaseController<ReportHeader>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<ReportHeader> DbSet => db.ReportHeader.AsNoTracking().OrderBy(o => o.id);

    }
}
