using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface IdesignationMasterServices
    {
        Task<ServiceStatusResponseModel> SaveUpdateDesignation(designationMaster Designation);
        Task<ServiceStatusResponseModel> UpdateDesignationStatus(int id, byte status, int userId);
    }
}
