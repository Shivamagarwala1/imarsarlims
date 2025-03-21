using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class doctorReferalMasterController : BaseController<doctorReferalMaster>
    {
        private readonly ContextClass db;
        private readonly IdoctorReferalServices _doctorReferalServices;

        public doctorReferalMasterController(ContextClass context, ILogger<BaseController<doctorReferalMaster>> logger, IdoctorReferalServices doctorReferalServices) : base(context, logger)
        {
            db = context;
            this._doctorReferalServices = doctorReferalServices;
        }
        protected override IQueryable<doctorReferalMaster> DbSet => db.doctorReferalMaster.AsNoTracking().OrderBy(o => o.doctorId);

        [HttpPost("SaveUpdateReferDoctor")]
        public async Task<ServiceStatusResponseModel> SaveUpdateReferDoctor(doctorReferalMaster refDoc)
        {
            try
            {
                var result = await _doctorReferalServices.SaveUpdateReferDoctor(refDoc);
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

        [HttpPost("UpdateReferDoctorStatus")]
        public async Task<ServiceStatusResponseModel> UpdateReferDoctorStatus(int DoctorId, byte status, int UserId)
        {
            try
            {
                var result = await _doctorReferalServices.UpdateReferDoctorStatus(DoctorId, status,UserId);
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

        [HttpGet("ReferDoctorData")]
        public async Task<ServiceStatusResponseModel> ReferDoctorData()
        {
            try
            {
                var result = await _doctorReferalServices.ReferDoctorData();
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
