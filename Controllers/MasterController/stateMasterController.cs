using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class stateMasterController : BaseController<stateMaster>
    {
        private readonly ContextClass db;

        public stateMasterController(ContextClass context, ILogger<BaseController<stateMaster>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<stateMaster> DbSet => db.stateMaster.AsNoTracking().OrderBy(o => o.id);
    }
}
