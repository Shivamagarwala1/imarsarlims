using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Account;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Services
{
    public class tnxBookingServices : ItnxBookingServices
    {
        private readonly ContextClass db;
        public tnxBookingServices(ContextClass context, ILogger<BaseController<tnx_Booking>> logger)
        {
            db = context;
        }
        async Task<ServiceStatusResponseModel> ItnxBookingServices.GetPatientData(patientDataRequestModel patientdata)
        {
            try
            {
                var patientData = (from tb in db.tnx_Booking
                                   join dr in db.doctorReferalMaster on tb.refID1 equals dr.doctorId
                                   join cm in db.centreMaster on tb.centreId equals cm.centreId
                                   join em in db.empMaster on tb.createdById equals em.empId
                                   join rt in db.rateTypeMaster on tb.rateId equals rt.rateTypeId
                                   select new
                                   {
                                       tb.bookingDate,
                                       tb.transactionId,
                                       tb.name,
                                       Age = string.Concat(tb.ageYear, " Y ", tb.ageMonth, " M ", tb.ageDay, " D/", tb.gender),
                                       tb.mobileNo,
                                       RefDoctor = dr.doctorName,
                                       CentreName = cm.companyName,
                                       ratetype = rt.rateType,
                                       RegBy = string.Concat(em.fName, " ", em.lName),
                                       tb.grossAmount,
                                       tb.discount,
                                       tb.netAmount,
                                       tb.paidAmount,
                                       DueAmt = tb.netAmount - tb.paidAmount
                                   }).ToListAsync();
                if (patientData != null)
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Data = patientData
                    };
                }
                else
                {
                    return new ServiceStatusResponseModel { Success = false, Message = "No Data Found" };
                }
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}

