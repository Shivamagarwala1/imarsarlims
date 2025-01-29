using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class organismAntibioticMasterController : BaseController<organismAntibioticMaster>
    {
        private readonly ContextClass db;
        private readonly IorganismAntibioticMasterServices _organismAntibioticMasterServices;

        public organismAntibioticMasterController(ContextClass context, ILogger<BaseController<organismAntibioticMaster>> logger, IorganismAntibioticMasterServices organismAntibioticMasterServices) : base(context, logger)
        {
            db = context;
            this._organismAntibioticMasterServices= organismAntibioticMasterServices;
        }
        protected override IQueryable<organismAntibioticMaster> DbSet => db.organismAntibioticMaster.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("SaveUpdateOrganismAntibiotic")]
        public async Task<ServiceStatusResponseModel> SaveUpdateOrganismAntibiotic(organismAntibioticMaster OrganismAntibiotic)
        {
            try
            {
                var result = await _organismAntibioticMasterServices.SaveUpdateOrganismAntibiotic(OrganismAntibiotic);
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

        [HttpPost("UpdateOrganismAntibioticStatus")]
        public async Task<ServiceStatusResponseModel> UpdateOrganismAntibioticStatus(int id, byte status,int userId)
        {
            try
            {
                var result = await _organismAntibioticMasterServices.UpdateOrganismAntibioticStatus(id,status,userId);
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
