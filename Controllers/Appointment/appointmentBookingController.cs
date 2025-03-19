using iMARSARLIMS.Interface.appointment;
using iMARSARLIMS.Model.Appointment;
using iMARSARLIMS.Response_Model;
using iMARSARLIMS.Services.Appointment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.Appointment
{
    [Route("api/[controller]")]
    [ApiController]
    public class appointmentBookingController : BaseController<appointmentBooking>
    {
        private readonly ContextClass db;
        private readonly IappointmentBookingServices _appointmentBookingServices;
        public appointmentBookingController(ContextClass context, ILogger<BaseController<appointmentBooking>> logger, IappointmentBookingServices appointmentBookingServices) : base(context, logger)
        {
            db = context;
            this._appointmentBookingServices = appointmentBookingServices;
        }
        protected override IQueryable<appointmentBooking> DbSet => db.appointmentBooking.AsNoTracking().OrderBy(o => o.appointmentId);


        [HttpGet("GetAppointmentData")]
        public async Task<ServiceStatusResponseModel> GetAppointmentData(DateTime FromDate, DateTime Todate, int CentreID)
        {
            try
            {
                var result = await _appointmentBookingServices.GetAppointmentData(FromDate,Todate,CentreID);
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
        [HttpPost("CancelAppointment")]
        public async Task<ServiceStatusResponseModel> CancelAppointment(int AppointmentId, int isCancel, int userid)
        {
            try
            {
                var result = await _appointmentBookingServices.CancelAppointment(AppointmentId,isCancel,userid);
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
        [HttpPost("rescheduleAppointment")]
        public async Task<ServiceStatusResponseModel> rescheduleAppointment(int AppointmentId,  int userid)
        {
            try
            {
                var result = await _appointmentBookingServices.rescheduleAppointment(AppointmentId,  userid);
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

        [HttpPost("AssignAppointment")]
        public async Task<ServiceStatusResponseModel> AssignAppointment(int AppointmentId, int pheleboid, int userid)
        {
            try
            {
                var result = await _appointmentBookingServices.CancelAppointment(AppointmentId, pheleboid, userid);
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
