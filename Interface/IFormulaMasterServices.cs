using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface IFormulaMasterServices
    {
        Task<ServiceStatusResponseModel> SaveUpdateFormula(formulaMaster Formula);
    }
}
