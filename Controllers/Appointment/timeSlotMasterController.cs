using iMARSARLIMS.Interface.appointment;
using iMARSARLIMS.Model.Appointment;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.Appointment
{
    [Route("api/[controller]")]
    [ApiController]
    public class timeSlotMasterController : BaseController<timeSlotMaster>
    {
        private readonly ContextClass db;
        private readonly ItimeSlotMasterServices _timeSlotMasterServices;

        public timeSlotMasterController(ContextClass context, ILogger<BaseController<timeSlotMaster>> logger, ItimeSlotMasterServices timeSlotMasterServices) : base(context, logger)
        {
            db = context;
            this._timeSlotMasterServices = timeSlotMasterServices;
        }
        protected override IQueryable<timeSlotMaster> DbSet => db.timeSlotMaster.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("SaveUpdateTimeSlot")]
        public async Task<ServiceStatusResponseModel> SaveUpdateTimeSlot(List<timeSlotMaster> slot)
        {
            try
            {
                var result = await _timeSlotMasterServices.SaveUpdateTimeSlot(slot);
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
        [HttpGet("GetTimeSlotData")]
        public async Task<ServiceStatusResponseModel> GetTimeSlotData()
        {
            try
            {
                var result = await _timeSlotMasterServices.GetTimeSlotData();
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
        [HttpPost("UpdateTimeSlotStatus")]
        public async Task<ServiceStatusResponseModel> UpdateTimeSlotStatus(int id, byte status, int UserId)
        {
            try
            {
                var result = await _timeSlotMasterServices.UpdateTimeSlotStatus(id,status,UserId);
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
