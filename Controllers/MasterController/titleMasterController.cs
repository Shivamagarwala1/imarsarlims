using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class titleMasterController : BaseController<titleMaster>
    {
        private readonly ContextClass db;
        private readonly ItitleMasterServices _titleMasterServices;

        public titleMasterController(ContextClass context, ILogger<BaseController<titleMaster>> logger, ItitleMasterServices titleMasterServices) : base(context, logger)
        {
            db = context;
            this._titleMasterServices = titleMasterServices;
        }
        protected override IQueryable<titleMaster> DbSet => db.titleMaster.AsNoTracking().OrderBy(o => o.id);
        [HttpPost("SaveUpdateTitle")]
        public async Task<ServiceStatusResponseModel> SaveUpdateTitle(titleMaster Title)
        {
            try
            {
                var result = await _titleMasterServices.SaveUpdateTitle(Title);
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

        [HttpPost("UpdateTitleStatus")]
        public async Task<ServiceStatusResponseModel> UpdateTitleStatus(int id, byte status, int userId)
        {
            try
            {
                var result = await _titleMasterServices.UpdateTitleStatus(id, status, userId);
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
    }
}
