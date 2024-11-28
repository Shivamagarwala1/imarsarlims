using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class formulaMasterController : BaseController<formulaMaster>
    {
        private readonly ContextClass db;

        public formulaMasterController(ContextClass context, ILogger<BaseController<formulaMaster>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<formulaMaster> DbSet => db.formulaMaster.AsNoTracking().OrderBy(o => o.id);
    }
}
