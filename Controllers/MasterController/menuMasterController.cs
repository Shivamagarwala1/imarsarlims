using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class menuMasterController : BaseController<menuMaster>
    {
        private readonly ContextClass db;
        private readonly IMenuMasterServices _MenuMasterServices;
        public menuMasterController(ContextClass context, ILogger<BaseController<menuMaster>> logger, IMenuMasterServices MenuMasterServices) : base(context, logger)
        {
            db = context;
            _MenuMasterServices = MenuMasterServices;
        }
        protected override IQueryable<menuMaster> DbSet => db.menuMaster.AsNoTracking().OrderBy(o => o.id);
        [HttpPost("SaveMenu")]
        public async Task<ServiceStatusResponseModel> SaveMenu(menuMaster MenuMaster)
        {
            if (MenuMaster == null)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = "Invalide Data"
                };
            }
            try
            {
                var result = await _MenuMasterServices.SaveMenu(MenuMaster);
                return result;
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        [HttpPost("UpdateMenuStatus")]
        public async Task<ServiceStatusResponseModel> UpdateMenuStatus(int menuId,bool Status)
        {
            try
            {
                var result = await _MenuMasterServices.UpdateMenuStatus(menuId, Status);
                return result;
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        [HttpGet("GetAllMenu")]
        public async Task<ActionResult<ServiceStatusResponseModel>> GetAllMenu(ODataQueryOptions<menuMaster> queryOptions)
        {
            try
            {
                var result = await _MenuMasterServices.GetAllMenu(queryOptions);
                return Ok(result); 
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

    }
}
