using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class designationMasterController : BaseController<designationMaster>
    {
        private readonly ContextClass db;

        public designationMasterController(ContextClass context, ILogger<BaseController<designationMaster>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<designationMaster> DbSet => db.designationMaster.AsNoTracking().OrderBy(o => o.id);
    }
}
