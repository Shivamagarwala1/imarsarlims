using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class itemMasterController : BaseController<itemMaster>
    {
        private readonly ContextClass db;
        private readonly IitemMasterServices _itemMasterServices;

        public itemMasterController(ContextClass context, ILogger<BaseController<itemMaster>> logger, IitemMasterServices itemMasterServices) : base(context, logger)
        {
            db = context;
            this._itemMasterServices = itemMasterServices;
        }
        protected override IQueryable<itemMaster> DbSet => db.itemMaster.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("SaveItemMaster")]
        public async Task<ServiceStatusResponseModel> SaveItemMaster(itemMaster itemmaster)
        {
            try
            {
                var result = await _itemMasterServices.SaveItemMaster(itemmaster);
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
