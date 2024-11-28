using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class machine_resultController : BaseController<machine_result>
    {
        private readonly ContextClass db;

        public machine_resultController(ContextClass context, ILogger<BaseController<machine_result>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<machine_result> DbSet => db.machine_result.AsNoTracking().OrderBy(o => o.id);
    }
}
