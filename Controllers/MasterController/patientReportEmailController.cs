using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class patientReportEmailController : BaseController<patientReportEmail>
    {
        private readonly ContextClass db;

        public patientReportEmailController(ContextClass context, ILogger<BaseController<patientReportEmail>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<patientReportEmail> DbSet => db.patientReportEmail.AsNoTracking().OrderBy(o => o.id);

    }
}
