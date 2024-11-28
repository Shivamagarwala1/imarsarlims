using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class whatsappController : BaseController<whatsapp>
    {
        private readonly ContextClass db;

        public whatsappController(ContextClass context, ILogger<BaseController<whatsapp>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<whatsapp> DbSet => db.whatsapp.AsNoTracking().OrderBy(o => o.id);

    }
}
