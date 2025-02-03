using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class designationMasterController : BaseController<designationMaster>
    {
        private readonly ContextClass db;
        private readonly IdesignationMasterServices _designationMasterServices;

        public designationMasterController(ContextClass context, ILogger<BaseController<designationMaster>> logger, IdesignationMasterServices designationMasterServices) : base(context, logger)
        {
            db = context;
            this._designationMasterServices = designationMasterServices;
        }
        protected override IQueryable<designationMaster> DbSet => db.designationMaster.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("SaveUpdateDesignation")]
        public async Task<ServiceStatusResponseModel> SaveUpdateDesignation(designationMaster Designation)
        {
            try
            {
                var result = await _designationMasterServices.SaveUpdateDesignation(Designation);
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

        [HttpPost("UpdateDesignationStatus")]
        public async Task<ServiceStatusResponseModel> UpdateDesignationStatus(int id, byte status, int userId)
        {
            try
            {
                var result = await _designationMasterServices.UpdateDesignationStatus(id, status, userId);
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
