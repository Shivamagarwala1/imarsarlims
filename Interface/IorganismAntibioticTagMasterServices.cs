using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;

namespace iMARSARLIMS.Interface
{
    public interface IorganismAntibioticTagMasterServices
    {
        Task<ServiceStatusResponseModel> OrganismAntibioticeMapping(List<organismAntibioticTagMaster> organismAntibioticTag);
    }
}
