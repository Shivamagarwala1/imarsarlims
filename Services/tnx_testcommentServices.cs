using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Services
{
    public class tnx_testcommentServices: Itnx_testcommentServices
    {
        private readonly ContextClass db;
      public tnx_testcommentServices(ContextClass context, ILogger<BaseController<tnx_testcomment>> logger)
        {
            db = context;
        }

        async Task<ServiceStatusResponseModel> Itnx_testcommentServices.SaveTestComment(tnx_testcomment CommentData)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    if (CommentData.id == 0)
                    {
                        db.tnx_testcomment.Add(CommentData);
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
                        db.tnx_testcomment.Update(CommentData);
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
