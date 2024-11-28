using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class centerAccessController : BaseController<centerAccess>
    {
        private readonly ContextClass db;

        public centerAccessController(ContextClass context, ILogger<BaseController<centerAccess>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<centerAccess> DbSet => db.centerAccess.AsNoTracking().OrderBy(o => o.id);

    }
}
