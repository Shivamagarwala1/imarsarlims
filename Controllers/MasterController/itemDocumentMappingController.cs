using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class itemDocumentMappingController : BaseController<itemDocumentMapping>
    {
        private readonly ContextClass db;

        public itemDocumentMappingController(ContextClass context, ILogger<BaseController<itemDocumentMapping>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<itemDocumentMapping> DbSet => db.itemDocumentMapping.AsNoTracking().OrderBy(o => o.id);

    }
}
