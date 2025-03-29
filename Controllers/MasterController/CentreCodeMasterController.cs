using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class CentreCodeMasterController : BaseController<CentreCodeMaster>
    {
        private readonly ContextClass db;
        public CentreCodeMasterController(ContextClass context, ILogger<BaseController<CentreCodeMaster>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<CentreCodeMaster> DbSet => db.CentreCodeMaster.AsNoTracking().OrderBy(o => o.id);

    }
}
