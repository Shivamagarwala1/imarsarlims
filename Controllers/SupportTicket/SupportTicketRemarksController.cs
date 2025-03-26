using iMARSARLIMS.Model.SupportTicket;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.SupportTicket
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportTicketRemarksController : BaseController<SupportTicketRemarks>
    {
        private readonly ContextClass db;
        public SupportTicketRemarksController(ContextClass context, ILogger<BaseController<SupportTicketRemarks>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<SupportTicketRemarks> DbSet => db.SupportTicketRemarks.AsNoTracking().OrderBy(o => o.id);

    }
}
