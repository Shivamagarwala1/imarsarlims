using iMARSARLIMS.Model.Transaction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.transactionController
{

    [Route("api/[controller]")]
    [ApiController]
    public class tnx_BookingController : BaseController<tnx_Booking>
    {
        private readonly ContextClass db;

        public tnx_BookingController(ContextClass context, ILogger<BaseController<tnx_Booking>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<tnx_Booking> DbSet => db.tnx_Booking.AsNoTracking().OrderBy(o => o.transactionId);
       
    }
}
