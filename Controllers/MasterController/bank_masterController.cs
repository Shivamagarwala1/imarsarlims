using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using iMARSARLIMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class bank_masterController : BaseController<bank_master>
    {
        private readonly ContextClass db;
        private readonly Ibank_masterServices _bank_masterServices;
        public bank_masterController(ContextClass context, ILogger<BaseController<bank_master>> logger, Ibank_masterServices bank_masterServices) : base(context, logger)
        {
            db = context;
            this._bank_masterServices = bank_masterServices;
        }
        protected override IQueryable<bank_master> DbSet => db.bank_master.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("SaveUpdateBankMaster")]
        public async Task<ServiceStatusResponseModel> SaveUpdateBankMaster(bank_master Bank)
        {
            try
            {
                var result = await _bank_masterServices.SaveUpdateBankMaster(Bank);
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

        [HttpPost("UpdateBankStatus")]
        public async Task<ServiceStatusResponseModel> UpdateBankStatus(int id, byte status, int userId)
        {
            try
            {
                var result = await _bank_masterServices.UpdateBankStatus(id, status, userId);
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
