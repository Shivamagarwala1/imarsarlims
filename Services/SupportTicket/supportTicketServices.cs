using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface.SupportTicket;
using iMARSARLIMS.Model.SupportTicket;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Services.SupportTicket
{
    public class supportTicketServices : IsupportTicketServices
    {
        private readonly ContextClass db;
        public supportTicketServices(ContextClass context, ILogger<BaseController<supportTicket>> logger)
        {
            db = context;
        }

        async Task<ServiceStatusResponseModel> IsupportTicketServices.GetTicketDetails()
        {
            try
            {
                var TicketData = await (from st in db.supportTicket
                                        join stt in db.SupportTicketType on st.ticketTypeId equals stt.id
                                        select new
                                        {
                                            st.clientId,
                                            st.clientName, st.ticketTypeId, stt.ticketType,
                                            st.isAssigned, st.isCompleted, st.isReopen,
                                            createdDate = st.CreateDate.ToString("yyyy-MMM-dd"),
                                            AssigneDate = st.AssignedDate.HasValue ? st.AssignedDate.Value.ToString("yyyy-MMM-dd") : "",
                                            DeliveryDate = st.Deliverydate.HasValue ? st.Deliverydate.Value.ToString("yyyy-MMM-dd") : "",
                                            CompletedDate = st.CompletedDate.HasValue ? st.CompletedDate.Value.ToString("yyyy-MMM-dd") : "",
                                            ReopenDate = st.ReopenDate.HasValue ? st.ReopenDate.Value.ToString("yyyy-MMM-dd") : "",
                                            assinedTo= db.empMaster.Where(e=>e.empId==st.assignedTo).Select(e=>e.fName).ToString(),
                                            CompletedBY = db.empMaster.Where(e => e.empId == st.completedBy).Select(e => e.fName).ToString(),
                                            AssigneBy = db.empMaster.Where(e => e.empId == st.AssignedBy).Select(e => e.fName).ToString(),
                                            reopenBy= db.empMaster.Where(e => e.empId == st.ReopenBy).Select(e => e.fName).ToString(),
                                            st.ReopenReason,
                                            st.ActionTaken,
                                            st.task
                                            
                                        }).ToListAsync();
                if (TicketData.Count > 0)
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Data = TicketData
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
            catch(Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        async Task<ServiceStatusResponseModel> IsupportTicketServices.AssignTicket(int ticketId, int AssigneTo, DateTime DeliveryDate, int UserId)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var ticket= db.supportTicket.Where(t=>t.id==ticketId).FirstOrDefault();
                    if (ticket != null)
                    {
                        ticket.isAssigned = 1;
                        ticket.assignedTo = AssigneTo;
                        ticket.Deliverydate = DeliveryDate;
                        ticket.AssignedBy = UserId;
                        db.supportTicket.Update(ticket);
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
                            Message = "Ticket Not Found"
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

        async Task<ServiceStatusResponseModel> IsupportTicketServices.closeTicket(int ticketId, string actionTaken, int UserId)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var ticket = db.supportTicket.Where(t => t.id == ticketId).FirstOrDefault();
                    if (ticket != null)
                    {
                        ticket.isCompleted = 1;
                        ticket.CompletedDate = DateTime.Now;
                        ticket.completedBy = UserId;
                        db.supportTicket.Update(ticket);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Completed Successful"
                        };
                    }
                    else
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "Ticket Not Found"
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

        async Task<ServiceStatusResponseModel> IsupportTicketServices.ReOpenTicket(int ticketId, string reOpenReason, int UserId)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var ticket = db.supportTicket.Where(t => t.id == ticketId).FirstOrDefault();
                    if (ticket != null)
                    {
                        ticket.isReopen = 1;
                        ticket.ReopenBy = UserId;
                        ticket.ReopenReason = reOpenReason;
                        ticket.ReopenDate = DateTime.Now;
                        db.supportTicket.Update(ticket);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Reopen Successful"
                        };
                    }
                    else
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "Ticket Not Found"
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

        async Task<ServiceStatusResponseModel> IsupportTicketServices.saveUpdateSupportTicket(supportTicket ticket)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    if (ticket.id == 0)
                    {
                        db.supportTicket.Add(ticket);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "saved Successful"
                        };
                    }
                    else
                    {
                        db.supportTicket.Update(ticket);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Update Successful"
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
    }
}
