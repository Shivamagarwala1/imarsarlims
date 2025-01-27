using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using iMARSARLIMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class itemTemplateController : BaseController<itemTemplate>
    {
        private readonly ContextClass db;
        private readonly IitemTemplateServices _itemTemplateServices;

        public itemTemplateController(ContextClass context, ILogger<BaseController<itemTemplate>> logger, IitemTemplateServices itemTemplateServices) : base(context, logger)
        {
            db = context;
            this._itemTemplateServices = itemTemplateServices;
        }
        protected override IQueryable<itemTemplate> DbSet => db.itemTemplate.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("SaveUpdateTemplate")]
        public async Task<ServiceStatusResponseModel> SaveUpdateTemplate(itemTemplate Template)
        {
            try
            {
                var result = await _itemTemplateServices.SaveUpdateTemplate(Template);
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

        [HttpPost("UpdateTemplateStatus")]
        public async Task<ServiceStatusResponseModel> UpdateTemplateStatus(int id,byte status, int Userid)
        {
            try
            {
                var result = await _itemTemplateServices.UpdateTemplateStatus(id,status, Userid);
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

        [HttpPost("GetTemplateData")]
        public async Task<ServiceStatusResponseModel> GetTemplateData(int CentreID, int testid)
        {
            try
            {
                var result = await _itemTemplateServices.GetTemplateData(CentreID, testid);
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
