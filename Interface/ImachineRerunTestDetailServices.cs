using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface ImachineRerunTestDetailServices
    {
        Task<ServiceStatusResponseModel> SaveRerun(List<machineRerunTestDetail> RerunDetail);
    }
}
