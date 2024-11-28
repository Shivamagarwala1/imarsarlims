using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class zoneMasterController : BaseController<zoneMaster>
    {
        private readonly ContextClass db;

        public zoneMasterController(ContextClass context, ILogger<BaseController<zoneMaster>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<zoneMaster> DbSet => db.zoneMaster.AsNoTracking().OrderBy(o => o.id);
    }
}
