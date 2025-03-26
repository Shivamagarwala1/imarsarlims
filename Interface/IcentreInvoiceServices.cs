using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface IcentreInvoiceServices
    {
        Task<ServiceStatusResponseModel> CreateInvoice(centreInvoiceRequestModel CentreInvoice);
        Task<ServiceStatusResponseModel> SearchInvoiceData(DateTime FromDate, DateTime Todate, List<int> CentreId);
        Task<ServiceStatusResponseModel> GetLastInvoiceData(List<int> CentreId);
        byte[] PrintInvoice(string InvoiceNo);
        byte[] PrintInvoiceData(string InvoiceNo);
        byte[] PrintInvoiceDataExcel(string InvoiceNo);
    }
}
