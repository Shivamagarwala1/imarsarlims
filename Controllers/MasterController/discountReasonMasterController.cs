using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class discountReasonMasterController : BaseController<discountReasonMaster>
    {
        private readonly ContextClass db;

        public discountReasonMasterController(ContextClass context, ILogger<BaseController<discountReasonMaster>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<discountReasonMaster> DbSet => db.discountReasonMaster.AsNoTracking().OrderBy(o => o.id);
    }
}
