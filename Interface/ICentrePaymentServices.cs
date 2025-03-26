using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface ICentrePaymentServices
    {
        Task<ServiceStatusResponseModel> SubmitPayment(CentrePaymentRequestModel centrePayments);
        Task<ServiceStatusResponseModel> paymentRecieptUpload(IFormFile paymentReciept);
        Task<ServiceStatusResponseModel> PaymentApproveReject(CentrePaymetVerificationRequestModel CentrePaymetVerificationRequest);
        Task<ServiceStatusResponseModel> LedgerStatus(string CentreId);
        Task<ServiceStatusResponseModel> ClientLedgerStatus(List<int> CentreId, DateTime FromDate, DateTime ToDate);
        Task<ServiceStatusResponseModel> GetPatientBillDetail(string Workorderid);
        Task<ServiceStatusResponseModel> CancelPatientReciept(string Workorderid,int Userid);
        Task<ServiceStatusResponseModel> GetPatientpaymentDetail(string Workorderid);
        Task<ServiceStatusResponseModel> ChangePatientpaymentDetail(List<ChangePaymentMode> PaymentMode);
        Task<ServiceStatusResponseModel> GetRateList(int ratetypeID);
        byte[] GetRateListPdf(int ratetypeID);
        Task<ServiceStatusResponseModel> TransferRateToRate(int FromRatetypeid, int ToRatetypeid ,string type, double Percentage);
        Task<ServiceStatusResponseModel> ClientDepositReport(List<int> centreid, DateTime FromDate, DateTime ToDate, string Paymenttype,int status);
        Task<ServiceStatusResponseModel> ClientDeposit(int centreid,  string Paymenttype, int status);
        byte[] ClientDepositReportExcel(List<int> centreid, DateTime FromDate, DateTime ToDate, string Paymenttype, int status);
        byte[] ClientDepositReportPdf(List<int> centreid, DateTime FromDate, DateTime ToDate, string Paymenttype, int status);
        Task<ServiceStatusResponseModel> GetWorkOrderdetailCentreChange(string WorkOrderid);
        Task<ServiceStatusResponseModel> GetWorkOrderNewRate(string WorkOrderid, int RatetypeId);



        //Task<ServiceStatusResponseModel> ChangeBillingCentre(string WorkOrderId, int Centre, int RateType);
        //Task<ServiceStatusResponseModel> GetPatientForSettelmet(int CentreId, DateTime FromDate, DateTime ToDate);
        //Task<ServiceStatusResponseModel> UpdatePatientSettelment(List<BulkSettelmentRequest> SettelmentData);
        //Task<ServiceStatusResponseModel> CentreRateChange(int Centre, DateTime FromDate, DateTime ToDate);
        //Task<ServiceStatusResponseModel> LedgerStatement(int CentreId, DateTime FromDate, DateTime ToDate, string type);
       
    }
}
