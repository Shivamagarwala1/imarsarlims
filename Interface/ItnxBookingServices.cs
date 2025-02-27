using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface ItnxBookingServices
    {
        Task<ServiceStatusResponseModel> GetPatientData(patientDataRequestModel patientdata);
        string GetPatientDocumnet(string workOrderId);
        Task<ServiceStatusResponseModel> GetPaymentDetails(string workOrderId);
        Task<ServiceStatusResponseModel> SaveSettelmentDetail(List<settelmentRequestModel> settelments);
        Task<ServiceStatusResponseModel> GetHistoresult(int testid);
        Task<ServiceStatusResponseModel> GetMicroresult(int testid);
    }
}
