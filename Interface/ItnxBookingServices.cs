using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface ItnxBookingServices
    {
        Task<ServiceStatusResponseModel> GetPatientData(patientDataRequestModel patientdata);
        string GetPatientDocumnet(string workOrderId);
    }
}
