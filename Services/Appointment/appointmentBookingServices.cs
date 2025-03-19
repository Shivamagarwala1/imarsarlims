using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface.appointment;
using iMARSARLIMS.Model.Appointment;
using iMARSARLIMS.Model.Master;
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

        async Task<ServiceStatusResponseModel> IappointmentBookingServices.rescheduleAppointment(int AppointmentId, int userid)
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

        async Task<ServiceStatusResponseModel> IappointmentBookingServices.CancelAppointment(int AppointmentId, int isCancel, int userid)
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

        async Task<ServiceStatusResponseModel> IappointmentBookingServices.GetAppointmentData(DateTime FromDate, DateTime Todate, int CentreID)
        {
            try
            {
                var Query = from tb in db.tnx_Booking
                           join ap in db.appointmentBooking on tb.transactionId equals ap.transactionId
                           join tbp in db.tnx_BookingPatient on tb.patientId equals tbp.patientId
                           join tbi in db.tnx_BookingItem on tb.workOrderId equals tbi.workOrderId
                           where tb.bookingDate>= FromDate && tb.bookingDate<= Todate
                           select new
                           {
                               AppointmentId = ap.appointmentId,
                               PatientName = tb.name,
                               tb.workOrderId,
                               tb.centreId,
                               tb.patientId,
                               SceduleDate = ap.AppointmentScheduledOn.ToString("yyyy-MMM-dd hh:mm tt"),
                               BookingDate = tb.bookingDate.ToString("yyyy-MMM-dd hh:mm tt"),
                               Age = string.Concat(tb.ageYear, " Y ", tb.ageMonth, " M ", tb.ageDay, " D/", tb.gender),
                               Address = tbp.address,
                               tbi.investigationName,
                               status = ap.Status == 1 ? "Asigned" : ap.Status == 2 ? "Rescheduled" : ap.Status == 0 ? "New Appointment" : "Cancel"
                           };
                if(CentreID>0)
                {
                    Query = Query.Where(q=> q.centreId==CentreID);
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
    }
}
