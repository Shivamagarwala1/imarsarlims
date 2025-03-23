using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface.SupportTicket;
using iMARSARLIMS.Model.SupportTicket;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;
using static Google.Cloud.Dialogflow.V2.MessageEntry.Types;

namespace iMARSARLIMS.Services.SupportTicket
{
    public class supportTicketServices : IsupportTicketServices
    {
        private readonly ContextClass db;
        public supportTicketServices(ContextClass context, ILogger<BaseController<supportTicket>> logger)
        {
            db = context;
        }

        async Task<ServiceStatusResponseModel> IsupportTicketServices.GetTicketDetails(DateTime FromDate, DateTime Todate, int Status, int assingedto, string Datetype, int roleid)
        {
            try
            {
                var Query = from st in db.supportTicket
                            join stt in db.SupportTicketType on st.ticketTypeId equals stt.id
                            select new
                            {
                                ticketId = st.id,
                                st.clientId,
                                st.clientName,
                                st.ticketTypeId,
                                stt.ticketType,
                                st.isAssigned,
                                st.isCompleted,
                                st.isReopen,
                                createdDate = st.CreateDate.ToString("yyyy-MMM-dd"),
                                AssigneDate = st.AssignedDate.HasValue ? st.AssignedDate.Value.ToString("yyyy-MMM-dd") : "",
                                DeliveryDate = st.Deliverydate.HasValue ? st.Deliverydate.Value.ToString("yyyy-MMM-dd") : "",
                                CompletedDate = st.CompletedDate.HasValue ? st.CompletedDate.Value.ToString("yyyy-MMM-dd") : "",
                                ReopenDate = st.ReopenDate.HasValue ? st.ReopenDate.Value.ToString("yyyy-MMM-dd") : "",
                                assinedToname = db.empMaster.Where(e => e.empId == st.assignedTo).Select(e => e.fName).FirstOrDefault(),
                                CompletedBY = db.empMaster.Where(e => e.empId == st.completedBy).Select(e => e.fName).FirstOrDefault(),
                                AssigneBy = db.empMaster.Where(e => e.empId == st.AssignedBy).Select(e => e.fName).FirstOrDefault(),
                                reopenBy = db.empMaster.Where(e => e.empId == st.ReopenBy).Select(e => e.fName).FirstOrDefault(),
                                st.ReopenReason,
                                st.ActionTaken,
                                st.task,
                                st.assignedTo,
                                st.isHold,
                                st.isRejected,
                                st.Document,st.roleId,
                                ticketstatus = st.isClosed == 1 ? "closed" : st.isCompleted == 1 ? "Completed":st.isHold==1?"Hold":st.isRejected==1?"Rejected":st.isAssigned==1?"Assigned":"New",
                                st.isClosed, CreateDateFilter= st.CreateDate,AssignedDatefilter= st.AssignedDate,DeliveryDatefilter= st.Deliverydate
                            };
                if (roleid>0)
                {
                    Query = Query.Where(q => q.roleId ==roleid);
                }
                if (Datetype== "CreateDate")
                {
                    Query = Query.Where(q => q.CreateDateFilter >= FromDate && q.CreateDateFilter<= Todate);
                }
                else if (Datetype == "AssignedDate")
                {
                    Query = Query.Where(q => q.AssignedDatefilter >= FromDate && q.AssignedDatefilter <= Todate);
                }
                else if (Datetype == "Deliverydate") 
                {
                    Query = Query.Where(q => q.DeliveryDatefilter >= FromDate && q.DeliveryDatefilter <= Todate);
                }
                if (Status==0)
                {
                    Query= Query.Where(q=> q.isAssigned==0 && q.isCompleted==0 );
                }
               else if (Status == 1)
                {
                    Query = Query.Where(q => q.isAssigned == 1 && q.isCompleted == 0);
                }
               else if (Status == 2)
                {
                    Query = Query.Where(q => q.isAssigned == 1 && q.isCompleted == 1);
                }
                else if (Status == 3)
                {
                    Query = Query.Where(q => q.isHold == 1);
                }
                else if (Status == 3)
                {
                    Query = Query.Where(q => q.isHold == 1);
                }
                else if (Status == 3)
                {
                    Query = Query.Where(q => q.isRejected == 1);
                }
                else 
                {
                    Query = Query.Where(q => q.isReopen == 1);
                }
                if (assingedto > 0)
                {
                    Query = Query.Where(q => q.assignedTo == assingedto);
                }
                var TicketData = await Query.ToListAsync();

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
                    await transaction.RollbackAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = ex.Message
                    };
                }
            }
        }

        async Task<ServiceStatusResponseModel> IsupportTicketServices.closeTicket(int ticketId, string closeRemark, int UserId)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var ticket = db.supportTicket.Where(t => t.id == ticketId).FirstOrDefault();
                    if (ticket != null)
                    {
                        ticket.isClosed = 1;
                        ticket.closedDate = DateTime.Now;
                        ticket.closedBy = UserId;
                        ticket.closedRemark= closeRemark;
                        db.supportTicket.Update(ticket);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "closed Successful"
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
                    await transaction.RollbackAsync();
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
                    await transaction.RollbackAsync();
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
                    await transaction.RollbackAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = ex.Message
                    };
                }
            }
        }

        async Task<ServiceStatusResponseModel> IsupportTicketServices.HoldTicket(int ticketId, string HoldReason, int UserId)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var ticket = db.supportTicket.Where(t => t.id == ticketId).FirstOrDefault();
                    if (ticket != null)
                    {
                        ticket.isHold = 1;
                        ticket.holdReason = HoldReason;
                        ticket.holdBy = UserId;
                        ticket.holdDate= DateTime.Now;
                        db.supportTicket.Update(ticket);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Hold Successful"
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
                    await transaction.RollbackAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = ex.Message
                    };
                }
            }
        }

        async Task<ServiceStatusResponseModel> IsupportTicketServices.RejectTicket(int ticketId, string rejectedReason, int UserId)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var ticket = db.supportTicket.Where(t => t.id == ticketId).FirstOrDefault();
                    if (ticket != null)
                    {
                        ticket.isRejected = 1;
                        ticket.rejectReason = rejectedReason;
                        ticket.rejectedBy = UserId;
                        ticket.rejectDate = DateTime.Now;
                        db.supportTicket.Update(ticket);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Rejected Successful"
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
                    await transaction.RollbackAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = ex.Message
                    };
                }
            }
        }

        async Task<ServiceStatusResponseModel> IsupportTicketServices.CompleteTicket(int ticketId, string actionTaken, int UserId)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var ticket = db.supportTicket.Where(t => t.id == ticketId).FirstOrDefault();
                    if (ticket != null)
                    {
                        ticket.isCompleted = 1;
                        ticket.ActionTaken = actionTaken;
                        ticket.completedBy = UserId;
                        ticket.CompletedDate = DateTime.Now;
                        db.supportTicket.Update(ticket);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Rejected Successful"
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
