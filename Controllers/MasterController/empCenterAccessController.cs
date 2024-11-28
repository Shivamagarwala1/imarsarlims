using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class empCenterAccessController : BaseController<empCenterAccess>
    {
        private readonly ContextClass db;

        public empCenterAccessController(ContextClass context, ILogger<BaseController<empCenterAccess>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<empCenterAccess> DbSet => db.empCenterAccess.AsNoTracking().OrderBy(o => o.id);
    }
}
