using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface IdoctorReferalServices
    {
        Task<ServiceStatusResponseModel> SaveUpdateReferDoctor(doctorReferalMaster refDoc);
    }
}
