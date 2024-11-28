using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class logoDetailsController : BaseController<logoDetails>
    {
        private readonly ContextClass db;

        public logoDetailsController(ContextClass context, ILogger<BaseController<logoDetails>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<logoDetails> DbSet => db.logoDetails.AsNoTracking().OrderBy(o => o.id);

    }
}
