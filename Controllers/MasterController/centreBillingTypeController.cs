using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class centreBillingTypeController : BaseController<centreBillingType>
    {
        private readonly ContextClass db;
        public centreBillingTypeController(ContextClass context, ILogger<BaseController<centreBillingType>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<centreBillingType> DbSet => db.centreBillingType.AsNoTracking().OrderBy(o => o.id);

    }
}
