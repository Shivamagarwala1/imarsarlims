using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class degreeMasterController : BaseController<degreeMaster>
    {
        private readonly ContextClass db;

        public degreeMasterController(ContextClass context, ILogger<BaseController<degreeMaster>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<degreeMaster> DbSet => db.degreeMaster.AsNoTracking().OrderBy(o => o.id);
    }
}
