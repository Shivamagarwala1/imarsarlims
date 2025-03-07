using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface Itnx_OutsourceDetailServices
    {
        Task<ServiceStatusResponseModel> GetOutsourceData(DateTime FromDate, DateTime Todate, string SearchValue);
        Task<ServiceStatusResponseModel> SaveUpdateOutsourceData(List<tnx_OutsourceDetail> outSourcedata);
        byte[] GetOutsourceReportExcel(DateTime FromDate, DateTime Todate);
    }
}
