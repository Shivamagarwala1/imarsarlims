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
    public class empMasterController : BaseController<empMaster>
    {
        private readonly ContextClass db;
        private readonly IempMasterServices _empMasterServices;


        public empMasterController(ContextClass context, ILogger<BaseController<empMaster>> logger, IempMasterServices empMasterServices) : base(context, logger)
        {
            db = context;
            this._empMasterServices = empMasterServices;
        }
        protected override IQueryable<empMaster> DbSet => db.empMaster.AsNoTracking().OrderBy(o => o.id);


        [HttpPost("Login")]
        public async Task<ActionResult<List<LoginResponseModel>>> EmpLogin(LoginRequestModel loginRequestModel)
        {
            try
            {
                var result = await _empMasterServices.EmpLogin(loginRequestModel);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("SaveEmployee")]
        public async Task<ActionResult<ServiceStatusResponseModel>> SaveEmployee(empMaster empmaster)
        {
            try
            {
                var result = await _empMasterServices.SaveEmployee(empmaster);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("UploadDocument")]
        public async Task<ActionResult<ServiceStatusResponseModel>> UploadDocument(IFormFile Document)
        {
            try
            {
                var result = await _empMasterServices.UploadDocument(Document);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
