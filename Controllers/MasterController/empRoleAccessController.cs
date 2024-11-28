using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class empRoleAccessController : BaseController<empRoleAccess>
    {
        private readonly ContextClass db;

        public empRoleAccessController(ContextClass context, ILogger<BaseController<empRoleAccess>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<empRoleAccess> DbSet => db.empRoleAccess.AsNoTracking().OrderBy(o => o.id);
    }
}
