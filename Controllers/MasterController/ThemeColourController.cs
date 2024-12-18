using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThemeColourController : BaseController<ThemeColour>
    {
        public readonly ContextClass db;
        public ThemeColourController(ContextClass context, ILogger<BaseController<ThemeColour>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<ThemeColour> DbSet => db.ThemeColour.AsNoTracking().OrderBy(o => o.id);

    }
}
