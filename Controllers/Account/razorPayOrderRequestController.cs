using iMARSARLIMS.Model.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class razorPayOrderRequestController : BaseController<razorPayOrderRequest>
    {
        private readonly ContextClass db;
        public razorPayOrderRequestController(ContextClass context, ILogger<BaseController<razorPayOrderRequest>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<razorPayOrderRequest> DbSet => db.razorPayOrderRequest.AsNoTracking().OrderBy(o => o.id);

    }
}
