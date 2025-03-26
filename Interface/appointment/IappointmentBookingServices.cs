using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface.appointment
{
    public interface IappointmentBookingServices
    {
        Task<ServiceStatusResponseModel> GetAppointmentData(DateTime FromDate, DateTime Todate, int CentreID, int status);
        Task<ServiceStatusResponseModel> CancelAppointment(int AppointmentId, int isCancel, int userid,string Reason);
        Task<ServiceStatusResponseModel> AssignAppointment(int AppointmentId, int pheleboid, int userid);
        Task<ServiceStatusResponseModel> rescheduleAppointment(int AppointmentId, int userid,DateTime RescheduleDate, string rescheduleReson);
        Task<ServiceStatusResponseModel> GetPhelebo(int pincode);
        Task<ServiceStatusResponseModel> UpdateSamplestatus(List<appointmentSamplesStatusModel> sampleStatus);
    }
}
