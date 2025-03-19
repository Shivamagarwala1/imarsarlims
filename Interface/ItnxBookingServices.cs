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
        Task<ServiceStatusResponseModel> SendWhatsapp(string workOrderId, int Userid,string MobileNo,int header);
        Task<ServiceStatusResponseModel> WhatsappNo(string workOrderId);
        Task<ServiceStatusResponseModel> SendEmail(string workOrderId, int Userid,string EmailId,int header);
        Task<ServiceStatusResponseModel> SendEmailId(string workOrderId);

        byte[] CollectionReport(collectionReportRequestModel collectionData);
        Task<ServiceStatusResponseModel> CollectionReportData(collectionReportRequestModel collectionData);

        byte[] CollectionReportSummury(collectionReportRequestModel collectionData);

        byte[] CollectionReportExcel(collectionReportRequestModel collectionData);
        Task<ServiceStatusResponseModel> CollectionReportDataSummury(collectionReportRequestModel collectionData);

        byte[] CollectionReportExcelSummury(collectionReportRequestModel collectionData);
        byte[] DiscountReport(collectionReportRequestModel collectionData);
        Task<ServiceStatusResponseModel> DiscountReportData(collectionReportRequestModel collectionData);
        byte[] DiscountReportExcel(collectionReportRequestModel collectionData);

        byte[] DiscountReportSummury(collectionReportRequestModel collectionData);
        Task<ServiceStatusResponseModel> DiscountReportDataSummury(collectionReportRequestModel collectionData);
        byte[] DiscountReportExcelSummury(collectionReportRequestModel collectionData);
        Task<ServiceStatusResponseModel> DiscountAfterBill(DicountAfterBillRequestModel DiscountData);
        Task<ServiceStatusResponseModel> patientDataDiscount(string workorderId);
        Task<ServiceStatusResponseModel> TestRefund(testRefundModel RefundData);
    }
}
