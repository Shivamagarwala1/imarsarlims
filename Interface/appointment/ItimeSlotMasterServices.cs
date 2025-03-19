using iMARSARLIMS.Model.Appointment;
using iMARSARLIMS.Response_Model;
using System.Threading.Tasks;

namespace iMARSARLIMS.Interface.appointment
{
    public interface ItimeSlotMasterServices
    {
        Task<ServiceStatusResponseModel> SaveUpdateTimeSlot(List<timeSlotMaster> slot);
        Task<ServiceStatusResponseModel> GetTimeSlotData();
        Task<ServiceStatusResponseModel> UpdateTimeSlotStatus(int id, byte status, int UserId);
    }
}
