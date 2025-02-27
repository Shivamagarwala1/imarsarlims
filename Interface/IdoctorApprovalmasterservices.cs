using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface IdoctorApprovalmasterservices
    {
        Task<ServiceStatusResponseModel> saveupdateDoctorApproval(doctorApprovalMaster doctorApproval);
        Task<ServiceStatusResponseModel> updateDoctorApprovalStatus(int id, byte status, int userid);
        Task<ServiceStatusResponseModel> DoctorApprovalData();
        Task<ServiceStatusResponseModel> Doctorcenterwise(int empid, int centreid);
    }
}
