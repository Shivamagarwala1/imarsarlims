using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class patientDemographicLogController : BaseController<patientDemographicLog>
    {
        private readonly ContextClass db;

        public patientDemographicLogController(ContextClass context, ILogger<BaseController<patientDemographicLog>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<patientDemographicLog> DbSet => db.patientDemographicLog.AsNoTracking().OrderBy(o => o.id);

    }
}
