using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class Allergy_TypeMasterController : BaseController<Allergy_TypeMaster>
    {
        private readonly ContextClass db;

        public Allergy_TypeMasterController(ContextClass context, ILogger<BaseController<Allergy_TypeMaster>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<Allergy_TypeMaster> DbSet => db.Allergy_TypeMaster.AsNoTracking().OrderBy(o => o.id);
    }
}
