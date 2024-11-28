using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class labDepartmentController : BaseController<labDepartment>
    {
        private readonly ContextClass db;

        public labDepartmentController(ContextClass context, ILogger<BaseController<labDepartment>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<labDepartment> DbSet => db.labDepartment.AsNoTracking().OrderBy(o => o.id);
    }
}
