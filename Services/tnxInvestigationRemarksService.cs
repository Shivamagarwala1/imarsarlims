using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Services
{
    public class tnxInvestigationRemarksService : ItnxInvestigationRemarksService
    {
        private readonly ContextClass db;
        public tnxInvestigationRemarksService(ContextClass context, ILogger<BaseController<labDepartment>> logger)
        {
            db = context;
        }
        async Task<ServiceStatusResponseModel> ItnxInvestigationRemarksService.AddSampleRemark(tnx_InvestigationRemarks remark)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    if (remark == null)
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Invalid Data"
                        };
                    }
                    else
                    {
                        if (remark.id == 0)
                        {
                            db.tnx_InvestigationRemarks.Add(remark);
                            await db.SaveChangesAsync();
                            await transaction.CommitAsync();
                            return new ServiceStatusResponseModel
                            {
                               Success = true,
                               Message= "Saved Successful"
                            };
                        }
                        else
                        {
                            db.tnx_InvestigationRemarks.Update(remark);
                            await db.SaveChangesAsync();
                            await transaction.CommitAsync();
                            return new ServiceStatusResponseModel
                            {
                                Success = true,
                                Message = "Updated Successful"
                            };
                        }
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
        async Task<ServiceStatusResponseModel> ItnxInvestigationRemarksService.GetSampleremark(int transacctionId, string WorkOrderId, int itemId)
        {
            try
            {
                var data = await (from sr in db.tnx_InvestigationRemarks
                                  where sr.transactionId == transacctionId && sr.itemId == itemId && sr.WorkOrderId == WorkOrderId
                                  select new
                                  {
                                      sr.id,
                                      sr.transactionId, sr.itemId,
                                      sr.invRemarks, sr.isActive, sr.itemName,sr.isInternal
                                  }).ToListAsync();
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = data
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
