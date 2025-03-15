using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface.Store;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Model.Store;
using iMARSARLIMS.Response_Model;
using static Google.Cloud.Dialogflow.V2.Intent.Types.Message.Types.CarouselSelect.Types;

namespace iMARSARLIMS.Services.Store
{
    public class ItemMasterStoreServices : IItemMasterStoreServices
    {
        private readonly ContextClass db;
        public ItemMasterStoreServices(ContextClass context, ILogger<BaseController<ItemMasterStore>> logger)
        {
            db = context;
        }
        async Task<ServiceStatusResponseModel> IItemMasterStoreServices.SaveUpdateItemStore(ItemMasterStore item)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    if(item.itemId==0)
                    {
                        db.ItemMasterStore.Add(item);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Saved Successful"
                        };
                    }
                    else
                    {
                        db.ItemMasterStore.Update(item);
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

        async Task<ServiceStatusResponseModel> IItemMasterStoreServices.UpdateItemStatus(int id, byte status, int userId)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var data= db.ItemMasterStore.Where(i=>i.itemId==id).FirstOrDefault();
                    if (data != null)
                    {
                        data.isActive = status;
                        data.updateById= userId;
                        data.updateDateTime= DateTime.Now;
                        db.ItemMasterStore.Update(data);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Updated Successful"
                        };
                    }
                    else
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = " No item To Update "
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
