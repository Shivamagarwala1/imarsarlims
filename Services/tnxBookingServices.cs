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

        public string GetPatientDocumnet(string workOrderId)
        {
            try
            {
                var documentData = db.tnx_Booking
                                     .Where(b => b.workOrderId == workOrderId)
                                     .Select(b => b.uploadDocument)
                                     .FirstOrDefault();

                return documentData;
                
            }
            catch
            {
                return "";
            }
        }


        async Task<ServiceStatusResponseModel> ItnxBookingServices.GetPatientData(patientDataRequestModel patientdata)
        {
            try
            {
                    var query =from tb in db.tnx_Booking
                                         join dr in db.doctorReferalMaster on tb.refID1 equals dr.doctorId
                                         join cm in db.centreMaster on tb.centreId equals cm.centreId
                                         join em in db.empMaster on tb.createdById equals em.empId
                                         join rt in db.rateTypeMaster on tb.rateId equals rt.rateTypeId
                                         select new
                                         {
                                             bookingDate= tb.bookingDate.ToString("yyyy-MMM-dd hh:mm tt"),
                                             tb.transactionId,
                                             datefilter= tb.bookingDate,
                                             tb.workOrderId,
                                             tb.name,
                                             Age = string.Concat(tb.ageYear, " Y ", tb.ageMonth, " M ", tb.ageDay, " D/", tb.gender),
                                             tb.mobileNo,
                                             RefDoctor = dr.doctorName,
                                             CentreName = cm.centrecode,
                                             ratetype = rt.rateType,
                                             RegBy = string.Concat(em.fName, " ", em.lName),
                                             tb.grossAmount,
                                             tb.discount,
                                             tb.netAmount,
                                             tb.paidAmount,
                                             tb.createdById,
                                             tb.centreId,
                                             tb.paymentMode,
                                             documnet= !string.IsNullOrEmpty(tb.uploadDocument) ? 1 : 0,
                                             DueAmt = tb.netAmount - tb.paidAmount
                                         };

                query = query.Where(q => q.datefilter >= patientdata.FromDate && q.datefilter <= patientdata.ToDate);


                if (!string.IsNullOrEmpty(patientdata.SearchValue))
                {
                    query = query.Where(q => q.workOrderId == patientdata.SearchValue || q.name== patientdata.SearchValue);
                }
                if (patientdata.UserId!=0)
                {
                    query = query.Where(q => q.createdById == patientdata.UserId );
                }
                if (patientdata.CentreIds.Count > 0)
                {
                    query = query.Where(q => patientdata.CentreIds.Contains(q.centreId));
                }
                if (patientdata.status!= "")
                {
                    if (patientdata.status == "Credit")
                        query = query.Where(q => q.paymentMode == "Credit");
                    else if (patientdata.status == "PartialPaid")
                        query = query.Where(q => q.netAmount!= q.paidAmount);
                    else if (patientdata.status == "Paid")
                        query = query.Where(q => q.netAmount == q.paidAmount);
                    else 
                        query = query.Where(q => q.paidAmount==0);
                }
                query = query.OrderBy(q=> q.workOrderId);

                var patientData = await query.ToListAsync();
                if (patientData != null && patientData.Any())
                {
                    return new ServiceStatusResponseModel { Success = true,Data = patientData};
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

