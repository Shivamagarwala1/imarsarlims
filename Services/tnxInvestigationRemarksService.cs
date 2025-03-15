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
        async Task<ServiceStatusResponseModel> ItnxInvestigationRemarksService.AddSampleRemark(List<tnx_InvestigationRemarks> remarks)
        {
            if (remarks == null || remarks.Count == 0)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = "Invalid Data"
                };
            }

            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    foreach (var item in remarks)
                    {
                        if (item.id == 0)
                        {
                            db.tnx_InvestigationRemarks.Add(item);
                        }
                        else
                        { 
                            db.tnx_InvestigationRemarks.Update(item);
                        }
                    }
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "Saved Successful"
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
        async Task<ServiceStatusResponseModel> ItnxInvestigationRemarksService.GetSampleremark(int transacctionId, string WorkOrderId, int itemId)
        {
            try
            {
                var data = await (from sr in db.tnx_InvestigationRemarks
                                  join em in db.empMaster on sr.createdById equals em.empId
                                  where sr.transactionId == transacctionId && sr.itemId == itemId && sr.WorkOrderId == WorkOrderId
                                  select new
                                  {
                                      sr.id,
                                      sr.transactionId, sr.itemId,
                                      sr.invRemarks, sr.isActive, sr.itemName, sr.isInternal,
                                      remardkDate = sr.createdDateTime.ToString("yyyy-MMM-dd hh:mm tt"),
                                      remarkAdBy = string.Concat(em.fName, ' ', em.lName)
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
