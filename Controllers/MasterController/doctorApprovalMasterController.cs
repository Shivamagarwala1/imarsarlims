using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class doctorApprovalMasterController : BaseController<doctorApprovalMaster>
    {
        private readonly ContextClass db;

        public doctorApprovalMasterController(ContextClass context, ILogger<BaseController<doctorApprovalMaster>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<doctorApprovalMaster> DbSet => db.doctorApprovalMaster.AsNoTracking().OrderBy(o => o.id);

    }
}
