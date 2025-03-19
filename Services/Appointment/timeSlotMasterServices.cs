using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface.appointment;
using iMARSARLIMS.Model.Appointment;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Services.Appointment
{
    public class timeSlotMasterServices : ItimeSlotMasterServices
    {
        private readonly ContextClass db;
        public timeSlotMasterServices(ContextClass context, ILogger<BaseController<tnx_Booking>> logger)
        {
            db = context;
        }

        async Task<ServiceStatusResponseModel> ItimeSlotMasterServices.UpdateTimeSlotStatus(int id, byte status, int UserId)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var data = db.timeSlotMaster.Where(r => r.id == id).FirstOrDefault();
                    if (data == null)
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "Time slot Not Found"
                        };
                    }
                    else
                    {
                        data.isActive = status;
                        data.updateDateTime = DateTime.Now;
                        data.updateById = UserId;
                        db.timeSlotMaster.Update(data);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Updated Successful"
                        };
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = ex.Message
                    };
                }
            }
        }

        async Task<ServiceStatusResponseModel> ItimeSlotMasterServices.GetTimeSlotData()
        {
            try
            {
                var data = await (from ts in db.timeSlotMaster
                            join cm in db.centreMaster on ts.centreId equals cm.centreId
                            select new
                            {
                                ts.id,
                                cm.centrecode,
                                ts.centreId,
                                cm.companyName,
                                ts.timeslot,
                                ts.isActive
                            }).ToListAsync();
                if (data != null)
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Data = data
                    };
                }
                else
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = "No Data Found"
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

        async Task<ServiceStatusResponseModel> ItimeSlotMasterServices.SaveUpdateTimeSlot(List<timeSlotMaster> slot)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var msg = "";
                    foreach (var t in slot)
                    {
                        if (t.id == 0)
                        {
                            db.timeSlotMaster.Add(t);
                            await db.SaveChangesAsync();
                            msg = "Saved Successful";
                        }
                        else
                        {
                            db.timeSlotMaster.Update(t);
                            await db.SaveChangesAsync();
                            msg = "updated Successful";

                        }
                    }
                    await transaction.CommitAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = msg
                    };
                }
                
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
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
