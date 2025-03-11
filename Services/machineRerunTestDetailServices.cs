using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Services
{
    public class machineRerunTestDetailServices: ImachineRerunTestDetailServices
    {
        private readonly ContextClass db;
        public machineRerunTestDetailServices(ContextClass context, ILogger<BaseController<empMaster>> logger)
        {
            db = context;
        }

        async Task<ServiceStatusResponseModel> ImachineRerunTestDetailServices.SaveRerun(List<machineRerunTestDetail> RerunDetail)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    db.machineRerunTestDetail.AddRange(RerunDetail);
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "Saved Successfull"
                    };
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
