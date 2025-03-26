using iMARSARLIMS.Model.Sales;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface.Sales
{
    public interface ISalesEmployeeTaggingService
    {
        Task<ServiceStatusResponseModel> CreateSalestagging(List<SalesEmployeeTagging> SalesTagging);
        Task<ServiceStatusResponseModel> GetSalesTagging(int tagged);
        Task<ServiceStatusResponseModel> RemoveSalesTagging(int id);

    }
}
