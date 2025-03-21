using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface IdoctorShareMasterServices
    {
        Task<ServiceStatusResponseModel> GetDoctorShareData(int DoctorId, int DepartMentId,int CentreId,int type, string typeWise);
        Task<ServiceStatusResponseModel> SaveUpdateDoctorShareData(List<doctorShareMaster> shareData);
    }
}
