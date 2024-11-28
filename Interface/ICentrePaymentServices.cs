using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface ICentrePaymentServices
    {
        Task<ServiceStatusResponseModel> SubmitPayment(CentrePaymentRequestModel centrePayments);
        Task<ServiceStatusResponseModel> PaymentApproveReject(CentrePaymetVerificationRequestModel CentrePaymetVerificationRequest);
    }
}
