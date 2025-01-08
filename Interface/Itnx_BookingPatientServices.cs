using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface Itnx_BookingPatientServices
    {
        byte[] GetPatientReceipt(string workorderid);
        byte[] GetPatientMRPBill(string workorderid);
        Task<string> Getworkorderid(int centreId, string type);
        Task<ServiceStatusResponseModel> SavePatientRegistration(tnx_BookingPatient tnxBookingPatient);
    }
}
