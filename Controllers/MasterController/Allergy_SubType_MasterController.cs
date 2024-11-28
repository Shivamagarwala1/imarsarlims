using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class Allergy_SubType_MasterController : BaseController<Allergy_SubType_Master>
    {
        private readonly ContextClass db;

        public Allergy_SubType_MasterController(ContextClass context, ILogger<BaseController<Allergy_SubType_Master>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<Allergy_SubType_Master> DbSet => db.Allergy_SubType_Master.AsNoTracking().OrderBy(o => o.id);
    }
}
