using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface ICentrePaymentServices
    {
        Task<ServiceStatusResponseModel> SubmitPayment(CentrePaymentRequestModel centrePayments);
        Task<ServiceStatusResponseModel> PaymentApproveReject(CentrePaymetVerificationRequestModel CentrePaymetVerificationRequest);
        Task<ServiceStatusResponseModel> LedgerStatus(List<int> CentreId);
        Task<ServiceStatusResponseModel> ClientLedgerStatus(List<int> CentreId, DateTime FromDate, DateTime ToDate);
        Task<ServiceStatusResponseModel> GetPatientBillDetail(string Workorderid);
        Task<ServiceStatusResponseModel> CancelPatientReciept(string Workorderid,int Userid);
        Task<ServiceStatusResponseModel> GetPatientpaymentDetail(string Workorderid);
        Task<ServiceStatusResponseModel> ChangePatientpaymentDetail(List<ChangePaymentMode> PaymentMode);
        Task<ServiceStatusResponseModel> GetRateList(int CentreId);
        byte[] GetRateListPdf(int centreid);
        Task<ServiceStatusResponseModel> TransferRateToRate(int FromRatetypeid, int ToRatetypeid);
        Task<ServiceStatusResponseModel> ClientDepositReport(List<int> centreid, DateTime FromDate, DateTime ToDate, string Paymenttype);
        Task<ServiceStatusResponseModel> GetWorkOrderdetailCentreChange(string WorkOrderid);
        Task<ServiceStatusResponseModel> GetWorkOrderNewRate(string WorkOrderid, int RatetypeId);



        Task<ServiceStatusResponseModel> ChangeBillingCentre(string WorkOrderId, int Centre, int RateType);
        Task<ServiceStatusResponseModel> GetPatientForSettelmet(int CentreId, DateTime FromDate, DateTime ToDate);
        Task<ServiceStatusResponseModel> UpdatePatientSettelment(List<BulkSettelmentRequest> SettelmentData);
        Task<ServiceStatusResponseModel> CentreRateChange(int Centre, DateTime FromDate, DateTime ToDate);
        Task<ServiceStatusResponseModel> LedgerStatement(int CentreId, DateTime FromDate, DateTime ToDate, string type);
       
    }
}
