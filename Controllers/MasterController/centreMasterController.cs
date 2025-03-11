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
    public class centreMasterController : BaseController<centreMaster>
    {
        private readonly ContextClass db;
        private readonly IcentreMasterServices _centreMasterServices;
        public centreMasterController(ContextClass context, ILogger<BaseController<centreMaster>> logger, IcentreMasterServices centreMasterServices) : base(context, logger)
        {
            db = context;
            this._centreMasterServices = centreMasterServices;
        }
        protected override IQueryable<centreMaster> DbSet => db.centreMaster.AsNoTracking().OrderBy(o => o.centreId);

        [HttpPost("SaveCentreDetail")]
        public async Task<ServiceStatusResponseModel> SaveCentreDetail(centreMaster centremaster)
        {
            try
            {
                var result = await _centreMasterServices.SaveCentreDetail(centremaster);
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

        [HttpPost("UpdateCentreStatus")]
        public async Task<ServiceStatusResponseModel> UpdateCentreStatus(int CentreId, byte status, int UserId)
        {
            try
            {
                var result = await _centreMasterServices.UpdateCentreStatus(CentreId, status, UserId);
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

        [HttpGet("GetParentCentre")]
        public async Task<ServiceStatusResponseModel> GetParentCentre()
        {
            try
            {
                var result = await _centreMasterServices.GetParentCentre();
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
        [HttpGet("GetCentreType")]
        public async Task<ServiceStatusResponseModel> GetCentreType(int billingtype)
        {
            try
            {
                var result = await _centreMasterServices.GetCentreType(billingtype);
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

        [HttpGet("GetProcesiongLab")]
        public async Task<ServiceStatusResponseModel> GetProcesiongLab()
        {
            try
            {
                var result = await _centreMasterServices.GetProcesiongLab();
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

        [HttpGet("GetMRPRateType")]
        public async Task<ServiceStatusResponseModel> GetMRPRateType()
        {
            try
            {
                var result = await _centreMasterServices.GetMRPRateType();
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
        [HttpGet("GetRateType")]
        public async Task<ServiceStatusResponseModel> GetRateType(int CentreType, int ParentCentre)
        {
            try
            {
                var result = await _centreMasterServices.GetRateType(CentreType, ParentCentre);
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

        [HttpGet("GetCentreData")]
        public async Task<ServiceStatusResponseModel> GetCentreData(int centreId)
        {
            try
            {
                var result = await _centreMasterServices.GetCentreData(centreId);
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

        [HttpPost("SaveLetterHead")]
        public async Task<ServiceStatusResponseModel> SaveLetterHead(ReportLetterHead LetterHead)
        {
            try
            {
                var result = await _centreMasterServices.SaveLetterHead(LetterHead);
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
        [HttpGet("GetLetterHeaddetails")]
        public async Task<ServiceStatusResponseModel> GetLetterHeaddetails(int CentreId)
        {
            try
            {
                var result = await _centreMasterServices.GetLetterHeaddetails(CentreId);
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

        [HttpGet("GetLetterHeaddetailall")]
        public async Task<ServiceStatusResponseModel> GetLetterHeaddetailall()
        {
            try
            {
                var result = await _centreMasterServices.GetLetterHeaddetailall();
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

        [HttpGet("GetRatetypeCentreWise")]
        public async Task<ServiceStatusResponseModel> GetRatetypeCentreWise(int CentreId)
        {
            try
            {
                var result = await _centreMasterServices.GetRatetypeCentreWise(CentreId);
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

        [HttpPost("DeleteLetterHeadDetail")]
        public async Task<ServiceStatusResponseModel> DeleteLetterHeadDetail(int CentreId)
        {
            try
            {
                var result = await _centreMasterServices.DeleteLetterHeadDetail(CentreId);
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
