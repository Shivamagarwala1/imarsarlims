using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Model.Transaction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorApprovalSignController : BaseController<DoctorApprovalSign>
    {
        private readonly ContextClass db;
        
        public DoctorApprovalSignController(ContextClass context, ILogger<BaseController<DoctorApprovalSign>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<DoctorApprovalSign> DbSet => db.DoctorApprovalSign.AsNoTracking().OrderBy(o => o.id);

    }
}
