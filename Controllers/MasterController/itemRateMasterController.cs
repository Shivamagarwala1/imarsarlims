using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class itemRateMasterController : BaseController<itemRateMaster>
    {
        private readonly ContextClass db;

        public itemRateMasterController(ContextClass context, ILogger<BaseController<itemRateMaster>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<itemRateMaster> DbSet => db.itemRateMaster.AsNoTracking().OrderBy(o => o.id);

    }
}
