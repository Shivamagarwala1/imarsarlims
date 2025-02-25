using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabRemarkMasterController : BaseController<LabRemarkMaster>
    {
        private readonly ContextClass db;

        public LabRemarkMasterController(ContextClass context, ILogger<BaseController<LabRemarkMaster>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<LabRemarkMaster> DbSet => db.LabRemarkMaster.AsNoTracking().OrderBy(o => o.id);

    }
}
