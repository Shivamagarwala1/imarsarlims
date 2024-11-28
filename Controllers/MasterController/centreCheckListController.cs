using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class centreCheckListController : BaseController<centreCheckList>
    {
        
        private readonly ContextClass db;

        public centreCheckListController(ContextClass context, ILogger<BaseController<centreCheckList>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<centreCheckList> DbSet => db.centreCheckList.AsNoTracking().OrderBy(o => o.id);

    }
}
