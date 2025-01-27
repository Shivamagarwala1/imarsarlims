using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class helpMenumMasterController : BaseController<helpMenuMaster>
    {
        private readonly ContextClass db;
        private readonly IhelpMenuMasterServices _helpMenuMasterServices;

        public helpMenumMasterController(ContextClass context, ILogger<BaseController<helpMenuMaster>> logger, IhelpMenuMasterServices helpMenuMasterServices) : base(context, logger)
        {
            db = context;
            this._helpMenuMasterServices = helpMenuMasterServices;
        }
        protected override IQueryable<helpMenuMaster> DbSet => db.helpMenuMaster.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("SaveUpdateHelpMenu")]
        public async Task<ServiceStatusResponseModel> SaveUpdateHelpMenu(helpMenuMaster HelpMenu)
        {
            try
            {
                var result = await _helpMenuMasterServices.SaveUpdateHelpMenu(HelpMenu);
                return result;
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = ex.InnerException?.Message ?? "An error occurred."
                };
            }
        }

    }
}
