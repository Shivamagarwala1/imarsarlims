using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class discountTypeMasterController : BaseController<discountTypeMaster>
    {
        private readonly ContextClass db;

        public discountTypeMasterController(ContextClass context, ILogger<BaseController<discountTypeMaster>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<discountTypeMaster> DbSet => db.discountTypeMaster.AsNoTracking().OrderBy(o => o.id);

    }
}
