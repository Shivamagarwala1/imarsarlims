using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Transaction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace iMARSARLIMS.Controllers.TransactionController
{
    [Route("api/[controller]")]
    [ApiController]
    public class tnx_BookingPatientController : BaseController<tnx_BookingPatient>
    {
        private readonly ContextClass db;
        private readonly Itnx_BookingPatientServices _tnx_BookingPatientServices;

        public tnx_BookingPatientController(ContextClass context, ILogger<BaseController<tnx_BookingPatient>> logger, Itnx_BookingPatientServices tnx_BookingPatientServices) : base(context, logger)
        {
            db = context;
            this._tnx_BookingPatientServices = tnx_BookingPatientServices;
        }
        protected override IQueryable<tnx_BookingPatient> DbSet => db.tnx_BookingPatient.AsNoTracking().OrderBy(o => o.patientId);

        [HttpGet("Getworkorderid")]
        public async Task<ActionResult<string>> Getworkorderid(int centreId, string type)
        {
            try
            {
                var empId = Convert.ToInt32(Encoding.UTF8.GetString(Convert.FromBase64String(HttpContext.Request.Headers["empId"])));
                var roleId = Convert.ToInt32(Encoding.UTF8.GetString(Convert.FromBase64String(HttpContext.Request.Headers["roleId"])));
                var CentreId = Convert.ToInt32(Encoding.UTF8.GetString(Convert.FromBase64String(HttpContext.Request.Headers["centreId"])));
                var employeeCount = await (from em in db.empMaster
                                           join emr in db.empRoleAccess on em.id equals emr.empId
                                           join emc in db.empCenterAccess on em.id equals emc.empId
                                           where em.id == empId && emr.roleId == roleId && emc.centreId == CentreId
                                           select em).CountAsync();

                var result = "";
                if (employeeCount > 0)
                {
                    result = await _tnx_BookingPatientServices.Getworkorderid(centreId, type);
                    return Ok(result);
                }
                else
                {
                    result = "Validation Failed";
                }

               return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("SavePatientRegistration")]
        public async Task<ActionResult> SavePatientRegistration(tnx_BookingPatient tnxBookingPatient)
        {
            try
            {
                var result = await _tnx_BookingPatientServices.SavePatientRegistration(tnxBookingPatient);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
