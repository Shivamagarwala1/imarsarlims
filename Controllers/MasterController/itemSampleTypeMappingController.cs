using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class itemSampleTypeMappingController : BaseController<itemSampleTypeMapping>
    {
        private readonly ContextClass db;

        public itemSampleTypeMappingController(ContextClass context, ILogger<BaseController<itemSampleTypeMapping>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<itemSampleTypeMapping> DbSet => db.itemSampleTypeMapping.AsNoTracking().OrderBy(o => o.id);
    }
}
