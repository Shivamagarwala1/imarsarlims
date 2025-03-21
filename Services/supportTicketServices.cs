using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Services
{
    public class supportTicketServices: IsupportTicketServices
    {
        private readonly ContextClass db;
        public supportTicketServices(ContextClass context, ILogger<BaseController<supportTicket>> logger)
        {
            db = context;
        }

        async Task<ServiceStatusResponseModel> IsupportTicketServices.saveUpdateSupportTicket(supportTicket ticket)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    if(ticket.id==0)
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
