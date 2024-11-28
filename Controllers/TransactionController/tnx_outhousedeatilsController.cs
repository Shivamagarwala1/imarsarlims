using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Model.Transaction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.TransactionController
{
    [Route("api/[controller]")]
    [ApiController]
    public class tnx_outhousedeatilsController : BaseController<tnx_outhousedeatils>
    {
        private readonly ContextClass db;

        public tnx_outhousedeatilsController(ContextClass context, ILogger<BaseController<tnx_outhousedeatils>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<tnx_outhousedeatils> DbSet => db.tnx_outhousedeatils.AsNoTracking().OrderBy(o => o.id);
    }
}
