using iMARSARLIMS.Model.Store;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface.Store
{
    public interface IindentServices
    {
        Task<ServiceStatusResponseModel> CreateIndent(Indent indentdata);
        Task<ServiceStatusResponseModel> GetIndentDetails(int roleId, int empId, DateTime fromDate, DateTime todate, int UserId, int itemId);
        Task<ServiceStatusResponseModel> GetDetail(int indentId);
        Task<ServiceStatusResponseModel> RejectIndent(int indentId, int UserId, string rejectionReason);
        Task<ServiceStatusResponseModel> IssueIndent(List<indentIssueDetail> issueDetails);
        Task<ServiceStatusResponseModel> Approveindent(List<indentApprovatmodel> approvaldata);

    }
}
