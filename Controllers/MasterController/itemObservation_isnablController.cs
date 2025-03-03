using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using iMARSARLIMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class itemObservation_isnablController : BaseController<itemObservation_isnabl>
    {
        private readonly ContextClass db;
        private readonly IitemObservation_isnablService _itemObservation_isnablService;
        public itemObservation_isnablController(ContextClass context, ILogger<BaseController<itemObservation_isnabl>> logger, IitemObservation_isnablService itemObservation_isnablService) : base(context, logger)
        {
            db = context;
            this._itemObservation_isnablService = itemObservation_isnablService;
        }
        protected override IQueryable<itemObservation_isnabl> DbSet => db.itemObservation_isnabl.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("SaveUpdateNabl")]
        public async Task<ServiceStatusResponseModel> SaveUpdateNabl(NablRequestModel Nabldata)
        {
            try
            {
                var result = await _itemObservation_isnablService.SaveUpdateNabl(Nabldata);
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
        [HttpPost("RemoveNabl")]
        public async Task<ServiceStatusResponseModel> RemoveNabl(int id)
        {
            try
            {
                var result = await _itemObservation_isnablService.RemoveNabl(id);
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
        [HttpPost("GetNablData")]
        public async Task<ServiceStatusResponseModel> GetNablData(int CentreId, int itemId)
        {
            try
            {
                var result = await _itemObservation_isnablService.GetNablData(CentreId,itemId);
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

        [HttpPost("UploadNablLogo")]
        public async Task<ServiceStatusResponseModel> UploadNablLogo(IFormFile NablLogo,int centreId)
        {
            try
            {
                var result = await _itemObservation_isnablService.UploadNablLogo(NablLogo, centreId);
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
