using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;

namespace iMARSARLIMS.Interface
{
    public interface Itnx_BookingPatientServices
    {
        Task<string> Getworkorderid(int centreId, string type);
        Task<ServiceStatusResponseModel> SavePatientRegistration(tnx_BookingPatient tnxBookingPatient);
        byte[] GetPatientReceipt(string workorderid);
    }
}
