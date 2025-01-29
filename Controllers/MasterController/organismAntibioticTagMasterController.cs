using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class organismAntibioticTagMasterController : BaseController<organismAntibioticTagMaster>
    {
        private readonly ContextClass db;
        private readonly IorganismAntibioticTagMasterServices _organismAntibioticTagMasterServices;

        public organismAntibioticTagMasterController(ContextClass context, ILogger<BaseController<organismAntibioticTagMaster>> logger, IorganismAntibioticTagMasterServices organismAntibioticTagMasterServices) : base(context, logger)
        {
            db = context;
            this._organismAntibioticTagMasterServices = organismAntibioticTagMasterServices;
        }
        protected override IQueryable<organismAntibioticTagMaster> DbSet => db.organismAntibioticTagMaster.AsNoTracking().OrderBy(o => o.id);


        [HttpPost("OrganismAntibioticeMapping")]
        public async Task<ServiceStatusResponseModel> OrganismAntibioticeMapping(List<organismAntibioticTagMaster> organismAntibioticTag)
        {
            try
            {
                var result = await _organismAntibioticTagMasterServices.OrganismAntibioticeMapping(organismAntibioticTag);
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

        [HttpGet("GetOrganismAntibioticeMapping")]
        public async Task<ServiceStatusResponseModel> GetOrganismAntibioticeMapping(int OrganismId)
        {
            try
            {
                var data= (from oam in db.organismAntibioticMaster
                                        join oat in db.organismAntibioticTagMaster
                                        on oam.id equals oat.antibiticId into oatJoin
                                        from oat in oatJoin.DefaultIfEmpty()
                                        where oam.microType == 2 && (oat == null || oat.organismId == OrganismId)
                                        select new
                                        {
                                            oam.id,
                                            Antibiotic = oam.organismAntibiotic,
                                            mapped = oat != null && oat.organismId == OrganismId ? "1" : "0"
                                        }).ToList();
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = data
                };
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
 