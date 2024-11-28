using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class doctorApprovalDepartmentsController : BaseController<doctorApprovalDepartments>
    {
        private readonly ContextClass db;

        public doctorApprovalDepartmentsController(ContextClass context, ILogger<BaseController<doctorApprovalDepartments>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<doctorApprovalDepartments> DbSet => db.doctorApprovalDepartments.AsNoTracking().OrderBy(o => o.id);

    }
}
