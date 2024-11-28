using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Model.Transaction;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.UriParser;
using Org.BouncyCastle.Asn1.Crmf;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Ocsp;
using System.ComponentModel.Design;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Reflection.Metadata;

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
