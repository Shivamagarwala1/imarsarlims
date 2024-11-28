using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class helpMenuMappingController : BaseController<helpMenuMapping>
    {
        private readonly ContextClass db;

        public helpMenuMappingController(ContextClass context, ILogger<BaseController<helpMenuMapping>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<helpMenuMapping> DbSet => db.helpMenuMapping.AsNoTracking().OrderBy(o => o.id);
    }
}
