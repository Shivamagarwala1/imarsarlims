using iMARSARLIMS.Interface.Store;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Model.Store;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.StoreController
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndentController : BaseController<Indent>
    {
        private readonly ContextClass db;
        private readonly IItemMasterStoreServices _itemMasterStoreServices;

        public IndentController(ContextClass context, ILogger<BaseController<Indent>> logger, IItemMasterStoreServices ItemMasterStoreServices) : base(context, logger)
        {
            db = context;
            this._itemMasterStoreServices = ItemMasterStoreServices;
        }
        protected override IQueryable<Indent> DbSet => db.Indent.AsNoTracking().OrderBy(o => o.indentId);

        [HttpPost("CreateIndent")]
        public async Task<ServiceStatusResponseModel> CreateIndent(Indent indentdata)
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


    }
}
