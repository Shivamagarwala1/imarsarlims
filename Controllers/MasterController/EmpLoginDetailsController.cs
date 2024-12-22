using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpLoginDetailsController : BaseController<empLoginDetails>
    {
        private readonly ContextClass db;
        private readonly IEmpLoginDetailServices _EmpLoginDetailServices;

        public EmpLoginDetailsController(ContextClass context, ILogger<BaseController<empLoginDetails>> logger, IEmpLoginDetailServices EmpLoginDetailServices) : base(context, logger)
        {
            db = context;
            this._EmpLoginDetailServices = EmpLoginDetailServices;
        }
        protected override IQueryable<empLoginDetails> DbSet => db.empLoginDetails.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("SaveLoginDetails")]
        public async Task<ServiceStatusResponseModel> SaveLoginDetails(empLoginDetails emploginDetails)
        {
            try
            {
              var result = await _EmpLoginDetailServices.SaveLoginDetails(emploginDetails);
              return result;
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Message = ex.Message,
                };
            }
        }

    }

    
}
