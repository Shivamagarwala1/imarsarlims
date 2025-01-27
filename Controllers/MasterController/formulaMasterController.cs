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
    public class formulaMasterController : BaseController<formulaMaster>
    {
        private readonly ContextClass db;
        private readonly IFormulaMasterServices _FormulaMasterServices;

        public formulaMasterController(ContextClass context, ILogger<BaseController<formulaMaster>> logger, IFormulaMasterServices FormulaMasterServices) : base(context, logger)
        {
            db = context;
            this._FormulaMasterServices = FormulaMasterServices;
        }
        protected override IQueryable<formulaMaster> DbSet => db.formulaMaster.AsNoTracking().OrderBy(o => o.id);
        [HttpPost("SaveUpdateFormula")]
        public async Task<ServiceStatusResponseModel> SaveUpdateFormula(formulaMaster Formula)
        {
            try
            {
                var result = await _FormulaMasterServices.SaveUpdateFormula(Formula);
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
