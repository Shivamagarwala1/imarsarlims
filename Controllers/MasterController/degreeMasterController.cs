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
    public class degreeMasterController : BaseController<degreeMaster>
    {
        private readonly ContextClass db;
        private readonly IdegreeMasterServices _degreeMasterServices;

        public degreeMasterController(ContextClass context, ILogger<BaseController<degreeMaster>> logger, IdegreeMasterServices degreeMasterServices) : base(context, logger)
        {
            db = context;
            this._degreeMasterServices = degreeMasterServices;
        }
        protected override IQueryable<degreeMaster> DbSet => db.degreeMaster.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("SaveUpdateDegree")]
        public async Task<ServiceStatusResponseModel> SaveUpdateDegree(degreeMaster Degree)
        {
            try
            {
                var result = await _degreeMasterServices.SaveUpdateDegree(Degree);
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

        [HttpPost("UpdateDegreeStatus")]
        public async Task<ServiceStatusResponseModel> UpdateDegreeStatus(int id, byte status,  int userId)
        {
            try
            {
                var result = await _degreeMasterServices.UpdateDegreeStatus(id, status,  userId);
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
