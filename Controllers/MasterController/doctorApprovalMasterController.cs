using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class doctorApprovalMasterController : BaseController<doctorApprovalMaster>
    {
        private readonly ContextClass db;
        private readonly IdoctorApprovalmasterservices _doctorApprovalmasterservices;
        public doctorApprovalMasterController(ContextClass context, ILogger<BaseController<doctorApprovalMaster>> logger, IdoctorApprovalmasterservices doctorApprovalmasterservices) : base(context, logger)
        {
            db = context;
            this._doctorApprovalmasterservices = doctorApprovalmasterservices;
        }
        protected override IQueryable<doctorApprovalMaster> DbSet => db.doctorApprovalMaster.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("saveupdateDoctorApproval")]
        public async Task<ServiceStatusResponseModel> saveupdateDoctorApproval(doctorApprovalMaster doctorApproval)
        {
            try
            {
                var result = await _doctorApprovalmasterservices.saveupdateDoctorApproval(doctorApproval);
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

        [HttpPost("updateDoctorApprovalStatus")]
        public async Task<ServiceStatusResponseModel> updateDoctorApprovalStatus(int id,byte status,int userid)
        {
            try
            {
                var result = await _doctorApprovalmasterservices.updateDoctorApprovalStatus(id,status,userid);
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

        [HttpGet("DoctorApprovalData")]
        public async Task<ServiceStatusResponseModel> DoctorApprovalData()
        {
            try
            {
                var result = await _doctorApprovalmasterservices.DoctorApprovalData();
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
