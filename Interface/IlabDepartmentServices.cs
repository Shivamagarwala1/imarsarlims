using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface IlabDepartmentServices
    {
        Task<ServiceStatusResponseModel> SaveUpdateLabDepartment(labDepartment Department);
        Task<ServiceStatusResponseModel> UpdateDepartmentOrder(List<DepartmentOrderModel> DepartmentOrder);
    }
}
