using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class bank_masterController : BaseController<bank_master>
    {
        private readonly ContextClass db;

        public bank_masterController(ContextClass context, ILogger<BaseController<bank_master>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<bank_master> DbSet => db.bank_master.AsNoTracking().OrderBy(o => o.id);
    }
}
