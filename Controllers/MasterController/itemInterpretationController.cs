using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class itemInterpretationController : BaseController<itemInterpretation>
    {
        private readonly ContextClass db;

        public itemInterpretationController(ContextClass context, ILogger<BaseController<itemInterpretation>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<itemInterpretation> DbSet => db.itemInterpretation.AsNoTracking().OrderBy(o => o.id);

    }
}
