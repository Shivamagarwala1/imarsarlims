using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class centreCheckListMappingController : BaseController<centreCheckListMapping>
    {
        private readonly ContextClass db;

        public centreCheckListMappingController(ContextClass context, ILogger<BaseController<centreCheckListMapping>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<centreCheckListMapping> DbSet => db.centreCheckListMapping.AsNoTracking().OrderBy(o => o.id);

    }
}
