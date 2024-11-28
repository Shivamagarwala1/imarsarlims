using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class rateTypeMasterController : BaseController<rateTypeMaster>
    {
        private readonly ContextClass db;

        public rateTypeMasterController(ContextClass context, ILogger<BaseController<rateTypeMaster>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<rateTypeMaster> DbSet => db.rateTypeMaster.AsNoTracking().OrderBy(o => o.id);
    }
}
