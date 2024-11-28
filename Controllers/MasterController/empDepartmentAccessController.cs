using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class empDepartmentAccessController : BaseController<empDepartmentAccess>
    {
        private readonly ContextClass db;

        public empDepartmentAccessController(ContextClass context, ILogger<BaseController<empDepartmentAccess>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<empDepartmentAccess> DbSet => db.empDepartmentAccess.AsNoTracking().OrderBy(o => o.id);
    }
}
