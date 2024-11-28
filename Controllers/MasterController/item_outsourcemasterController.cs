using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class item_outsourcemasterController : BaseController<item_outsourcemaster>
    {
        private readonly ContextClass db;

        public item_outsourcemasterController(ContextClass context, ILogger<BaseController<item_outsourcemaster>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<item_outsourcemaster> DbSet => db.item_outsourcemaster.AsNoTracking().OrderBy(o => o.id);

    }
}
