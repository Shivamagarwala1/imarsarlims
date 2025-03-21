using iMARSARLIMS.Model.SupportTicket;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.SupportTicket
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportTicketTypeController : BaseController<SupportTicketType>
    {
        private readonly ContextClass db;
        public SupportTicketTypeController(ContextClass context, ILogger<BaseController<SupportTicketType>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<SupportTicketType> DbSet => db.SupportTicketType.AsNoTracking().OrderBy(o => o.id);

    }
}
