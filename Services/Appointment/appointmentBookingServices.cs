using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface.appointment;
using iMARSARLIMS.Model.Appointment;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Services.Appointment
{
    public class appointmentBookingServices : IappointmentBookingServices
    {
        private readonly ContextClass db;
        public appointmentBookingServices(ContextClass context, ILogger<BaseController<bank_master>> logger)
        {
            db = context;
        }

        async Task<ServiceStatusResponseModel> IappointmentBookingServices.rescheduleAppointment(int AppointmentId, int userid, DateTime RescheduleDate, string rescheduleReson)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var data = db.appointmentBooking.Where(a => a.appointmentId == AppointmentId).FirstOrDefault();
                    if (data != null)
                    {
                        data.rescheduleBy = userid;
                        data.rescheduleDate = DateTime.Now;
                        data.AppointmentScheduledOn = RescheduleDate;
                        data.reschedulreason = rescheduleReson;
                        data.Status = 2;
                        db.appointmentBooking.Update(data);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Reschedule Successful"
                        };
                    }
                    else
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "Appointment Not found"
                        };
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

        async Task<ServiceStatusResponseModel> IappointmentBookingServices.AssignAppointment(int AppointmentId, int pheleboid, int userid)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var data = db.appointmentBooking.Where(a => a.appointmentId == AppointmentId).FirstOrDefault();
                    if (data != null)
                    {
                        data.assignedBy = userid;
                        data.AssignedPhlebo = pheleboid;
                        data.AssignedDate = DateTime.Now;
                        data.Status = 1;
                        db.appointmentBooking.Update(data);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Assigned Successful"
                        };
                    }
                    else
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "Appointment Not found"
                        };
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

        async Task<ServiceStatusResponseModel> IappointmentBookingServices.CancelAppointment(int AppointmentId, int isCancel, int userid, string Reason)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var data = db.appointmentBooking.Where(a => a.appointmentId == AppointmentId).FirstOrDefault();
                    if (data != null)
                    {
                        data.assignedBy = userid;
                        data.CancelDate = DateTime.Now;
                        data.Status = -1;
                        data.cancelreason = Reason;
                        db.appointmentBooking.Update(data);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Cancel Successful"
                        };
                    }
                    else
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "Appointment Not found"
                        };
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

        async Task<ServiceStatusResponseModel> IappointmentBookingServices.GetAppointmentData(DateTime FromDate, DateTime Todate, int CentreID,int status)
        {
            try
            {
                var startDate = FromDate.Date;
                var endDate = Todate.Date.AddDays(1).AddTicks(-1);
                var Query = from tb in db.tnx_Booking
                            join ap in db.appointmentBooking on tb.transactionId equals ap.transactionId
                            join tbp in db.tnx_BookingPatient on tb.patientId equals tbp.patientId
                            join tbi in db.tnx_BookingItem on tb.workOrderId equals tbi.workOrderId
                            join im in db.itemMaster on tbi.itemId equals im.itemId
                            where tb.bookingDate >= startDate && tb.bookingDate <= endDate
                            select new
                            {
                                AppointmentId = ap.appointmentId,
                                PatientName = tb.name,
                                tb.workOrderId,
                                tb.centreId,
                                tb.patientId,
                                tb.mobileNo,
                                tbp.pinCode,
                                tb.grossAmount,
                                tb.discount,
                                tb.netAmount,
                                tb.paidAmount, dueAmount= tb.netAmount-tb.paidAmount,
                                tb.rateId,
                                SceduleDate = ap.AppointmentScheduledOn.ToString("yyyy-MMM-dd hh:mm tt"),
                                BookingDate = tb.bookingDate.ToString("yyyy-MMM-dd hh:mm tt"),
                                Age = string.Concat(tb.ageYear, " Y ", tb.ageMonth, " M ", tb.ageDay, " D/", tb.gender),
                                Address = tbp.address,
                                tbi.investigationName,
                                tbi.isSampleCollected,
                                im.defaultsampletype,
                                TestId= tbi.id,tbi.barcodeNo,
                                Status1= ap.Status,
                                Assinedphelebo = db.empMaster.Where(e => e.empId == ap.AssignedPhlebo).Select(e => e.fName).FirstOrDefault(),
                                sampletypedata = (db.itemSampleTypeMapping.Where(i => i.itemId == im.itemId).Select(i => new { i.sampleTypeId, i.sampleTypeName })).ToList(),
                                status = ap.Status == 1 ? "Asigned" : ap.Status == 2 ? "Rescheduled" : ap.Status == 0 ? "New Appointment" : "Cancel"
                            };
                if (CentreID > 0)
                {
                    Query = Query.Where(q => q.centreId == CentreID);
                }
                if (status >= 0 && status<5)
                {
                    Query = Query.Where(q => q.Status1 == status);
                }
                
                
                var data = await Query.ToListAsync();
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = data
                };
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

        async Task<ServiceStatusResponseModel> IappointmentBookingServices.GetPhelebo(int pincode)
        {
            try
            {
                var phelebodata = await (from rm in db.RouteMapping
                                         join rt in db.routeMaster on rm.routeId equals rt.id
                                         join em in db.empMaster on rm.pheleboId equals em.empId

                                         select new
                                         {
                                             rm.pheleboId,
                                             pheleboname = string.Concat(em.fName, " ", em.lName),
                                             rt.pincode,
                                             status = (rt.pincode == pincode) ? "OnLocation" : "OutSideLocation"
                                         }).ToListAsync();

                                         var groupedData = phelebodata
    .GroupBy(p => new { p.pheleboId, p.pheleboname, p.status })
    .Select(g => new
    {
        g.Key.pheleboId,
        g.Key.pheleboname,
        g.Key.status
    })
    .ToList();
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = groupedData
                };
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

        async Task<ServiceStatusResponseModel> IappointmentBookingServices.UpdateSamplestatus(List<appointmentSamplesStatusModel> sampleStatus)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    foreach (var sample in sampleStatus)
                    {
                        var data= db.tnx_BookingItem.Where(bi=>bi.id== sample.testid).FirstOrDefault();
                        if(data != null)
                        {
                            data.isSampleCollected = sample.iscamplecollected;
                            data.sampleCollectionDate=DateTime.Now;
                            data.sampleCollectedID = sample.collectedBy;
                            data.sampleReceivedBY = sample.collectedBy.ToString();
                            data.sampleReceiveDate = DateTime.Now;
                            data.barcodeNo = sample.barcodeno;
                            db.tnx_BookingItem.Update(data);
                            await db.SaveChangesAsync();
                        }
                        
                    }
                    await transaction.CommitAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "updated Successful"
                    };
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
}
