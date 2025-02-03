using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Services
{
    public class sampleRejectionReasonServices : IsampleRejectionReasonServices
    {
        private readonly ContextClass db;
        public sampleRejectionReasonServices(ContextClass context, ILogger<BaseController<sampleRejectionReason>> logger)
        {

            db = context;
        }
        async Task<ServiceStatusResponseModel> IsampleRejectionReasonServices.SaveUpdateRejectionReason(sampleRejectionReason RejectionReason)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    if (RejectionReason.id == 0)
                    {
                        db.sampleRejectionReason.Add(RejectionReason);
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
                        db.sampleRejectionReason.Update(RejectionReason);
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

        async Task<ServiceStatusResponseModel> IsampleRejectionReasonServices.UpdateRejectionReasonStatus(int id, byte status, int Userid)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var data = db.sampleRejectionReason.Where(d => d.id == id).FirstOrDefault();
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
                        data.updateById = Userid;
                        data.updateDateTime = DateTime.Now;
                        db.sampleRejectionReason.Update(data);
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
