using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{

    [Route("api/[controller]")]
    [ApiController]
    public class RolePageAccessController : BaseController<RolePageAccess>
    {
        private readonly ContextClass db;

        public RolePageAccessController(ContextClass context, ILogger<BaseController<RolePageAccess>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<RolePageAccess> DbSet => db.RolePageAccess.AsNoTracking().OrderBy(o => o.id);
    }
}
