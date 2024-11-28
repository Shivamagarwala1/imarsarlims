using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class roleMasterController : BaseController<roleMaster>
    {
        private readonly ContextClass db;

        public roleMasterController(ContextClass context, ILogger<BaseController<roleMaster>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<roleMaster> DbSet => db.roleMaster.AsNoTracking().OrderBy(o => o.id);

    }
}
