using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class smsController : BaseController<sms>
    {
        private readonly ContextClass db;

        public smsController(ContextClass context, ILogger<BaseController<sms>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<sms> DbSet => db.sms.AsNoTracking().OrderBy(o => o.id);

    }
}
