using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class observationCommentController : BaseController<observationComment>
    {
        private readonly ContextClass db;

        public observationCommentController(ContextClass context, ILogger<BaseController<observationComment>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<observationComment> DbSet => db.observationComment.AsNoTracking().OrderBy(o => o.id);

    }
}
