using iMARSARLIMS.Interface;
using iMARSARLIMS.Interface.appointment;
using iMARSARLIMS.Model.Appointment;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using iMARSARLIMS.Services.Appointment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
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
            this._supportTicketServices = supportTicketServices;
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
    }
}
