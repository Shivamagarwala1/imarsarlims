using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Services
{
    public class helpMenuMasterServices : IhelpMenuMasterServices
    {
        private readonly ContextClass db;
        public helpMenuMasterServices(ContextClass context, ILogger<BaseController<cityMaster>> logger)
        {
            db = context;
        }

        async Task<ServiceStatusResponseModel> IhelpMenuMasterServices.RemoveHelpMenuMapping(int helpId, int itemId, int observationId, int userId)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var data= await db.helpMenuMapping.Where(h=> h.itemId== itemId && h.ObservationId== observationId && h.helpId== helpId).FirstOrDefaultAsync();
                    db.helpMenuMapping.Remove(data);
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "Removed Successful"
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = ex.Message,
                    };
                }
            }
        }

        async Task<ServiceStatusResponseModel> IhelpMenuMasterServices.SaveHelpMenuMapping(helpMenuMapping HelpMenu)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    db.helpMenuMapping.Add(HelpMenu);
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "Mapped Successful"
                    };
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

        async Task<ServiceStatusResponseModel> IhelpMenuMasterServices.SaveUpdateHelpMenu(helpMenuMaster HelpMenu)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var msg = "";
                    if (HelpMenu.id == 0)
                    {
                        var count = db.helpMenuMaster.Where(h => h.helpName == HelpMenu.helpName).Count();
                        if (count > 0)
                        {
                            return new ServiceStatusResponseModel
                            {
                                Success = false,
                                Message = "Help Menu Already Available"
                            };
                        }
                        await db.helpMenuMaster.AddAsync(HelpMenu);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        msg = "Saved Successful";
                    }
                    else
                    {
                        var data = db.helpMenuMaster.Where(h => h.id == HelpMenu.id).FirstOrDefault();
                        if (data != null)
                        {
                            data.helpName = HelpMenu.helpName;
                            db.helpMenuMaster.Update(data);
                            await db.SaveChangesAsync();
                            await transaction.CommitAsync();
                            msg = "Updated Successful";
                        }
                        else
                        {
                            return new ServiceStatusResponseModel
                            {
                                Success = false,
                                Message = "Please use correct id"
                            };
                        }
                    }
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = msg
                    };
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
