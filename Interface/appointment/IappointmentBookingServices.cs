using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface.appointment
{
    public interface IappointmentBookingServices
    {
        Task<ServiceStatusResponseModel> GetAppointmentData(DateTime FromDate, DateTime Todate, int CentreID);
        Task<ServiceStatusResponseModel> CancelAppointment(int AppointmentId, int isCancel, int userid);
        Task<ServiceStatusResponseModel> AssignAppointment(int AppointmentId, int pheleboid, int userid);
        Task<ServiceStatusResponseModel> rescheduleAppointment(int AppointmentId, int userid);
    }
}
