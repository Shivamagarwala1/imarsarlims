using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Model.Transaction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.TransactionController
{
    [Route("api/[controller]")]
    [ApiController]
    public class tnx_ReceiptDetailsController : BaseController<tnx_ReceiptDetails>
    {
        private readonly ContextClass db;

        public tnx_ReceiptDetailsController(ContextClass context, ILogger<BaseController<tnx_ReceiptDetails>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<tnx_ReceiptDetails> DbSet => db.tnx_ReceiptDetails.AsNoTracking().OrderBy(o => o.id);
    }
}
