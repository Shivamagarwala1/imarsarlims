using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestingController : BaseController<Testing>
    {
        private readonly ContextClass db;

        public TestingController(ContextClass context, ILogger<BaseController<Testing>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<Testing> DbSet => db.Testing.AsNoTracking().OrderBy(o => o.id);


    }
}
