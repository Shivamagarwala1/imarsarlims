using iMARSARLIMS.Interface;
using iMARSARLIMS.Interface.Store;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Model.Store;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Response_Model;
using iMARSARLIMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.StoreController
{
    [Route("api/[controller]")]
    [ApiController]
    public class itemMasterStoreController : BaseController<ItemMasterStore>
    {
        private readonly ContextClass db;
        private readonly IItemMasterStoreServices _itemMasterStoreServices;

        public itemMasterStoreController(ContextClass context, ILogger<BaseController<ItemMasterStore>> logger,  IItemMasterStoreServices ItemMasterStoreServices) : base(context, logger)
        {
            db = context;
            this._itemMasterStoreServices = ItemMasterStoreServices;
        }
        protected override IQueryable<ItemMasterStore> DbSet => db.ItemMasterStore.AsNoTracking().OrderBy(o => o.itemId);

        [HttpPost("SaveUpdateItemStore")]
        public async Task<ServiceStatusResponseModel> SaveUpdateItemStore(ItemMasterStore item)
        {
            try
            {
                var result = await _itemMasterStoreServices.SaveUpdateItemStore(item);
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

        [HttpPost("UpdateItemStatus")]
        public async Task<ServiceStatusResponseModel> UpdateItemStatus(int id, byte status, int userId)
        {
            try
            {
                var result = await _itemMasterStoreServices.UpdateItemStatus(id, status, userId);
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
