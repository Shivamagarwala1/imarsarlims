using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class sampleRejectionReasonController : BaseController<sampleRejectionReason>
    {
        private readonly ContextClass db;

        public sampleRejectionReasonController(ContextClass context, ILogger<BaseController<sampleRejectionReason>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<sampleRejectionReason> DbSet => db.sampleRejectionReason.AsNoTracking().OrderBy(o => o.id);

    }
}
