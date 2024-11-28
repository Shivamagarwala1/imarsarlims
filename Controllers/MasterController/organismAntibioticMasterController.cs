using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class organismAntibioticMasterController : BaseController<organismAntibioticMaster>
    {
        private readonly ContextClass db;

        public organismAntibioticMasterController(ContextClass context, ILogger<BaseController<organismAntibioticMaster>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<organismAntibioticMaster> DbSet => db.organismAntibioticMaster.AsNoTracking().OrderBy(o => o.id);
    }
}
