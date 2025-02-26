using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.transactionController
{

    [Route("api/[controller]")]
    [ApiController]
    public class tnx_BookingController : BaseController<tnx_Booking>
    {
        private readonly ContextClass db;
        private readonly ItnxBookingServices _tnxBookingServices;

        public tnx_BookingController(ContextClass context, ILogger<BaseController<tnx_Booking>> logger, ItnxBookingServices tnxBookingServices) : base(context, logger)
        {
            db = context;
            this._tnxBookingServices = tnxBookingServices;
        }
        protected override IQueryable<tnx_Booking> DbSet => db.tnx_Booking.AsNoTracking().OrderBy(o => o.transactionId);

        [HttpPost("GetPatientData")]
        public async Task<ServiceStatusResponseModel> GetPatientData(patientDataRequestModel patientdata)
        {
            try
            {
                var result = await _tnxBookingServices.GetPatientData(patientdata);
                return result;
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                { 
                Success=false,
                Message= ex.Message
                };
            }
        }
    }
}
