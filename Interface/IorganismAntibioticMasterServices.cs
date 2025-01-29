using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface IorganismAntibioticMasterServices
    {
        Task<ServiceStatusResponseModel> SaveUpdateOrganismAntibiotic(organismAntibioticMaster OrganismAntibiotic);
        Task<ServiceStatusResponseModel> UpdateOrganismAntibioticStatus(int id, byte status, int userId);
    }
}
