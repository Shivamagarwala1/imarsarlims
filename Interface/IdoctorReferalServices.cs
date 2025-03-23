using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface IdoctorReferalServices
    {
        Task<ServiceStatusResponseModel> SaveUpdateReferDoctor(doctorReferalMaster refDoc);
        Task<ServiceStatusResponseModel> ReferDoctorData();
        Task<ServiceStatusResponseModel> UpdateReferDoctorStatus(int DoctorId, byte status, int UserId);
        Task<ServiceStatusResponseModel> DoctorBussinessReport(string DoctorId, string centreID, DateTime FromDate, DateTime ToDate);
        Task<ServiceStatusResponseModel> DoctorBussinessReportSummury(string DoctorId, string centreID, DateTime FromDate, DateTime ToDate);
        byte[] DoctorBussinessReportPdf(string DoctorId, string centreID, DateTime FromDate, DateTime ToDate);
        byte[] DoctorBussinessReportSummuryPdf(string DoctorId, string centreID, DateTime FromDate, DateTime ToDate);
        byte[] DoctorBussinessReportExcel(string DoctorId, string centreID, DateTime FromDate, DateTime ToDate);
        byte[] DoctorBussinessReportSummuryExcel(string DoctorId, string centreID, DateTime FromDate, DateTime ToDate);
    }
}
