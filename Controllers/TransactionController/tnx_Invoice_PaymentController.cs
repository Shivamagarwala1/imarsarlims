using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Model.Transaction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.TransactionController
{
    [Route("api/[controller]")]
    [ApiController]
    public class tnx_invoice_paymentController : BaseController<tnx_Invoice_Payment>
    {
        private readonly ContextClass db;

        public tnx_invoice_paymentController(ContextClass context, ILogger<BaseController<tnx_Invoice_Payment>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<tnx_Invoice_Payment> DbSet => db.tnx_Invoice_Payment.AsNoTracking().OrderBy(o => o.id);
    }
}
