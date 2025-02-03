using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Services
{
    public class titleMasterServices : ItitleMasterServices
    {
        private readonly ContextClass db;
        public titleMasterServices(ContextClass context, ILogger<BaseController<titleMaster>> logger)
        {

            db = context;
        }
        async Task<ServiceStatusResponseModel> ItitleMasterServices.SaveUpdateTitle(titleMaster Title)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    if (Title.id == 0)
                    {
                        db.titleMaster.Add(Title);
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
                        db.titleMaster.Update(Title);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Updated Successful"
                        };
                    }
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

        async Task<ServiceStatusResponseModel> ItitleMasterServices.UpdateTitleStatus(int id, byte status, int userId)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var data = db.titleMaster.Where(d => d.id == id).FirstOrDefault();
                    if (data == null)
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "No Data Found to Update"
                        };
                    }
                    else
                    {
                        data.isActive = status;
                        data.updateById = userId;
                        data.updateDateTime = DateTime.Now;
                        db.titleMaster.Update(data);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Updated Successful"
                        };
                    }

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
