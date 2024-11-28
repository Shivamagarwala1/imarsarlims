using iMARSARLIMS.Model.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    public class machineObservationMappingController : BaseController<machineObservationMapping>
    {
        private readonly ContextClass db;

        public machineObservationMappingController(ContextClass context, ILogger<BaseController<machineObservationMapping>> logger) : base(context, logger)
        {
            db = context;
        }
        protected override IQueryable<machineObservationMapping> DbSet => db.machineObservationMapping.AsNoTracking().OrderBy(o => o.id);
    }
}
