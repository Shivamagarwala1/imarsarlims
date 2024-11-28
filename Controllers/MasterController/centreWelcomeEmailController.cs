using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class centreWelcomeEmailController : BaseController<centreWelcomeEmail>
    {
        private readonly ContextClass db;

        public centreWelcomeEmailController(ContextClass context, ILogger<BaseController<centreWelcomeEmail>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<centreWelcomeEmail> DbSet => db.centreWelcomeEmail.AsNoTracking().OrderBy(o => o.id);

    }
}
