using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface IcentreInvoiceServices
    {
        Task<ServiceStatusResponseModel> CreateInvoice(centreInvoiceRequestModel CentreInvoice);
        ServiceStatusResponseModel SearchInvoiceData(DateTime FromDate, DateTime Todate, List<int> CentreId);
    }
}
