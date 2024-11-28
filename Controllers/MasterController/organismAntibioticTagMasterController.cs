using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    }
}
 