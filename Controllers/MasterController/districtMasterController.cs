using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class districtMasterController : BaseController<districtMaster>
    {
        private readonly ContextClass db;

        public districtMasterController(ContextClass context, ILogger<BaseController<districtMaster>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<districtMaster> DbSet => db.districtMaster.AsNoTracking().OrderBy(o => o.id);
    }
}
