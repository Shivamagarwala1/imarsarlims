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
    public class doctorShareMasterController : BaseController<doctorShareMaster>
    {
        private readonly ContextClass db;
        private readonly IdoctorShareMasterServices _doctorShareMasterServices;

        public doctorShareMasterController(ContextClass context, ILogger<BaseController<doctorShareMaster>> logger, IdoctorShareMasterServices doctorShareMasterServices) : base(context, logger)
        {
            db = context;
            this._doctorShareMasterServices = doctorShareMasterServices;
        }
        protected override IQueryable<doctorShareMaster> DbSet => db.doctorShareMaster.AsNoTracking().OrderBy(o => o.id);

        [HttpGet("GetDoctorShareData")]
        public async Task<ServiceStatusResponseModel> GetDoctorShareData(int DoctorId, int DepartMentId,int CentreId,int type,string typeWise)
        {
            try
            {
                var result = await _doctorShareMasterServices.GetDoctorShareData(DoctorId,DepartMentId, CentreId, type,typeWise);
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
        [HttpPost("SaveUpdateDoctorShareData")]
        public async Task<ServiceStatusResponseModel> SaveUpdateDoctorShareData(List<doctorShareMaster> shareData)
        {
            try
            {
                var result = await _doctorShareMasterServices.SaveUpdateDoctorShareData(shareData);
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
