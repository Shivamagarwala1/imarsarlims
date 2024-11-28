using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class idMasterController : BaseController<idMaster>
    {
            private readonly ContextClass db;

            public idMasterController(ContextClass context, ILogger<BaseController<idMaster>> logger) : base(context, logger)
            {
                db = context;
            }
            protected override IQueryable<idMaster> DbSet => db.idMaster.AsNoTracking().OrderBy(o => o.id);

    }
}
