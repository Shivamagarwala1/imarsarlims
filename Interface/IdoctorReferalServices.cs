using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface IdoctorReferalServices
    {
        Task<ServiceStatusResponseModel> SaveUpdateReferDoctor(doctorReferalMaster refDoc);
        Task<ServiceStatusResponseModel> ReferDoctorData();
        Task<ServiceStatusResponseModel> UpdateReferDoctorStatus(int DoctorId, byte status, int UserId);
    }
}
