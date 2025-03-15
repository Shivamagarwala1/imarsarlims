using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using OfficeOpenXml.Table.PivotTable;

namespace iMARSARLIMS.Interface
{
    public interface ItnxBookingServices
    {
        Task<ServiceStatusResponseModel> GetPatientData(patientDataRequestModel patientdata);
        string GetPatientDocumnet(string workOrderId);
        Task<ServiceStatusResponseModel> GetPaymentDetails(string workOrderId);
        Task<ServiceStatusResponseModel> SaveSettelmentDetail(List<settelmentRequestModel> settelments);
        Task<ServiceStatusResponseModel> GetHistoresult(int testid);
        Task<ServiceStatusResponseModel> GetMicroresult(int testid,int reportStatus);
        Task<ServiceStatusResponseModel> GetPatientDetail(string workorderId);
        Task<ServiceStatusResponseModel> GetDispatchData(DispatchDataRequestModel patientdata);
        Task<ServiceStatusResponseModel> GetTestInfo(string TestId);
        Task<ServiceStatusResponseModel> GetbarcodeChangedetail(string WorkOrderId);
        Task<ServiceStatusResponseModel> UpdateBarcode(List<barcodeChangeRequest> NewBarcodeData);
        Task<ServiceStatusResponseModel> TatReport(DateTime FromDate, DateTime ToDate, int centreId, int departmentId, int itemid, string TatType);
        byte[] TatReportExcel(DateTime FromDate, DateTime ToDate, int centreId, int departmentId, int itemid, string TatType);
        byte[] TatReportpdf(DateTime FromDate, DateTime ToDate, int centreId, int departmentId, int itemid, string TatType);
        Task<ServiceStatusResponseModel> GetMethodChangedetail(string WorkOrderId);
        Task<ServiceStatusResponseModel> UpdateMethod(List<methodChangeRequestModel> methoddata);
        byte[] PrintWorkSheet(string TestIds);
        Task<ServiceStatusResponseModel> GetWorkSheetData(WorkSheetRequestModel worksheetdata);
        Task<ServiceStatusResponseModel> GetSampleTypedetail(string WorkOrderId);
        Task<ServiceStatusResponseModel> UpdateSampleType(List<SampltypeChangeRequestModel> sampletypedata);
        Task<ServiceStatusResponseModel> MachineResult(MachineResultRequestModel machineResult);
        Task<ServiceStatusResponseModel> GetReportDateChangeData(string WorkOrderId);
        Task<ServiceStatusResponseModel> ReportDateChange(List<DateChangeRequestModel> DateData);
        Task<ServiceStatusResponseModel> SendWhatsapp(string workOrderId, int Userid);
        Task<ServiceStatusResponseModel> SendEmail(string workOrderId, int Userid);
    }
}
