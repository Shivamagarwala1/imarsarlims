using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class testInterpretationController : BaseController<testInterpretation>
    {
        private readonly ContextClass db;

        public testInterpretationController(ContextClass context, ILogger<BaseController<testInterpretation>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<testInterpretation> DbSet => db.testInterpretation.AsNoTracking().OrderBy(o => o.id);

    }
}
