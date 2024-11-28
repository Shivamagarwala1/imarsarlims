using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class outSourcelabmasterController : BaseController<outSourcelabmaster>
    {
        private readonly ContextClass db;

        public outSourcelabmasterController(ContextClass context, ILogger<BaseController<outSourcelabmaster>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<outSourcelabmaster> DbSet => db.outSourcelabmaster.AsNoTracking().OrderBy(o => o.id);

    }
}
