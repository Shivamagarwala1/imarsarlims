using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class area_masterController : BaseController<area_master>
    {
        private readonly ContextClass db;

        public area_masterController(ContextClass context, ILogger<BaseController<area_master>> logger) : base(context, logger) 
        {
            db= context;
        }
        protected override IQueryable<area_master> DbSet => db.area_master.AsNoTracking().OrderBy(o => o.id);

    }
}
