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
    public class tat_masterController : BaseController<tat_master>
    {
        private readonly ContextClass db;
        private readonly ItatMasterServices _tatMasterServices;
        public tat_masterController(ContextClass context, ILogger<BaseController<tat_master>> logger, ItatMasterServices MenuMasterServices) : base(context, logger)
        {
            db = context;
            _tatMasterServices = MenuMasterServices;
        }
        protected override IQueryable<tat_master> DbSet => db.tat_master.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("SaveUpdateTatMaster")]
        public async Task<ServiceStatusResponseModel> SaveUpdateTatMaster(List<tat_master> Tatdata)
        {
            try
            {
                var result = await _tatMasterServices.SaveUpdateTatMaster(Tatdata);
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

        [HttpPost("GetTatMaster")]
        public async Task<ServiceStatusResponseModel> GetTatMaster(int centreId, int departmentId,List<int> ItemIds)
        {
            try
            {
                var result = await _tatMasterServices.GetTatMaster(centreId, departmentId,ItemIds);
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
