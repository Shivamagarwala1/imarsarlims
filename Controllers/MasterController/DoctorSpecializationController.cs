using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorSpecializationController : BaseController<DoctorSpecialization>
    {
        private readonly ContextClass db;

        public DoctorSpecializationController(ContextClass context, ILogger<BaseController<DoctorSpecialization>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<DoctorSpecialization> DbSet => db.DoctorSpecialization.AsNoTracking().OrderBy(o => o.id);

    }
}
