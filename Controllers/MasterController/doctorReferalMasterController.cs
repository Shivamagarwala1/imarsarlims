using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class doctorReferalMasterController : BaseController<doctorReferalMaster>
    {
        private readonly ContextClass db;

        public doctorReferalMasterController(ContextClass context, ILogger<BaseController<doctorReferalMaster>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<doctorReferalMaster> DbSet => db.doctorReferalMaster.AsNoTracking().OrderBy(o => o.doctorId);

    }
}
