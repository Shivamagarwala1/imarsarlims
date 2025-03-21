using iMARSARLIMS.Interface.appointment;
using iMARSARLIMS.Interface.SupportTicket;
using iMARSARLIMS.Model.Appointment;
using iMARSARLIMS.Model.SupportTicket;
using iMARSARLIMS.Response_Model;
using iMARSARLIMS.Services.Appointment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace iMARSARLIMS.Controllers.SupportTicket
{
    [Route("api/[controller]")]
    [ApiController]
    public class supportTicketController : BaseController<supportTicket>
    {
        private readonly ContextClass db;
        private readonly IsupportTicketServices _supportTicketServices;
        public supportTicketController(ContextClass context, ILogger<BaseController<supportTicket>> logger, IsupportTicketServices supportTicketServices) : base(context, logger)
        {
            db = context;
            _supportTicketServices = supportTicketServices;
        }
        protected override IQueryable<supportTicket> DbSet => db.supportTicket.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("saveUpdateSupportTicket")]
        public async Task<ServiceStatusResponseModel> saveUpdateSupportTicket(supportTicket ticket)
        {
            try
            {
                var result = await _supportTicketServices.saveUpdateSupportTicket(ticket);
                return result;
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

        [HttpPost("AssignTicket")]
        public async Task<ServiceStatusResponseModel> AssignTicket(int ticketId, int AssigneTo, DateTime DeliveryDate, int UserId)
        {
            try
            {
                var result = await _supportTicketServices.AssignTicket(ticketId, AssigneTo, DeliveryDate, UserId);
                return result;
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
        [HttpPost("closeTicket")]
        public async Task<ServiceStatusResponseModel> closeTicket(int ticketId, string actionTaken, int UserId)
        {
            try
            {
                var result = await _supportTicketServices.closeTicket(ticketId, actionTaken,  UserId);
                return result;
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
        [HttpPost("ReOpenTicket")]
        public async Task<ServiceStatusResponseModel> ReOpenTicket(int ticketId, string reOpenReason, int UserId)
        {
            try
            {
                var result = await _supportTicketServices.ReOpenTicket(ticketId, reOpenReason, UserId);
                return result;
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

        [HttpGet("GetTicketDetails")]
        public async Task<ServiceStatusResponseModel> GetTicketDetails()
        {
            try
            {
                var result = await _supportTicketServices.GetTicketDetails();
                return result;
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
