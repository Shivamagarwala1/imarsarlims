using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface IEmpLoginDetailServices
    {
        Task<ServiceStatusResponseModel> SaveLoginDetails(empLoginDetails empLoginDetail);
     //   Task<List<ServiceStatusResponseModel>> UpdateLoginDetails(empLoginDetails EmpLoginDetail);
    }
}
