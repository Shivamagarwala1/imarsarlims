using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Model.Transaction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{

    [Route("api/[controller]")]
    [ApiController]

    public class ReportEmailController : BaseController<ReportEmail>
    {
        private readonly ContextClass db;
        public ReportEmailController(ContextClass context, ILogger<BaseController<ReportEmail>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<ReportEmail> DbSet => db.ReportEmail.AsNoTracking().OrderBy(o => o.id);

    }
}
