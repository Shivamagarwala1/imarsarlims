using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class rateTypeTaggingController : BaseController<rateTypeTagging>
    {
        public readonly ContextClass db;
        public rateTypeTaggingController(ContextClass context, ILogger<BaseController<rateTypeTagging>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<rateTypeTagging> DbSet => db.rateTypeTagging.AsNoTracking().OrderBy(o => o.id);
    }
}
