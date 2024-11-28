using iMARSARLIMS.Model.Transaction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.TransactionController
{
    [Route("api/[controller]")]
    [ApiController]
    public class tnx_BookingStatusController : BaseController<tnx_BookingStatus>
    {
        private readonly ContextClass db;

        public tnx_BookingStatusController(ContextClass context, ILogger<BaseController<tnx_BookingStatus>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<tnx_BookingStatus> DbSet => db.tnx_BookingStatus.AsNoTracking().OrderBy(o => o.id);

    }
    
}
