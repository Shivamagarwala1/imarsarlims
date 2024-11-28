using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class itemCommentMasterController : BaseController<itemCommentMaster>
    {
        private readonly ContextClass db;

        public itemCommentMasterController(ContextClass context, ILogger<BaseController<itemCommentMaster>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<itemCommentMaster> DbSet => db.itemCommentMaster.AsNoTracking().OrderBy(o => o.id);

    }
}
