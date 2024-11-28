using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class containerColorMasterController : BaseController<containerColorMaster>
    {
        private readonly ContextClass db;

        public containerColorMasterController(ContextClass context, ILogger<BaseController<containerColorMaster>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<containerColorMaster> DbSet => db.containerColorMaster.AsNoTracking().OrderBy(o => o.id);
    }
}
