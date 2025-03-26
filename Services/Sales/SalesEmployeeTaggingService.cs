using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface.Sales;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Model.Sales;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Services.Sales
{
    public class SalesEmployeeTaggingService : ISalesEmployeeTaggingService
    {
        private readonly ContextClass db;

        public SalesEmployeeTaggingService(ContextClass context, ILogger<BaseController<SalesEmployeeTagging>> logger)
        {
            db = context;
        }

        public async Task<ServiceStatusResponseModel> CreateSalestagging(List<SalesEmployeeTagging> salesTagging)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var newTags = salesTagging.Where(x => x.id == 0).ToList();
                    var existingTags = salesTagging.Where(x => x.id != 0).ToList();

                    if (newTags.Any())
                        await db.SalesEmployeeTagging.AddRangeAsync(newTags);

                    if (existingTags.Any())
                        db.SalesEmployeeTagging.UpdateRange(existingTags);
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "Tagging Update Successful"
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

        async Task<ServiceStatusResponseModel> ISalesEmployeeTaggingService.RemoveSalesTagging(int id)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                   var data= db.SalesEmployeeTagging.Where(s=>s.id == id).FirstOrDefault();
                    if (data != null)
                    {
                        db.SalesEmployeeTagging.Remove(data);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();


                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Tagging Removed Successful"
                        };
                    }
                    else
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Tagging Not Found"
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

        async Task<ServiceStatusResponseModel> ISalesEmployeeTaggingService.GetSalesTagging(int tagged)
        {
            try
            {

                var taggingdata= await (from st in db.SalesEmployeeTagging
                                  join em in db.empMaster on st.SalesEmployeeId equals em.empId
                                  where st.TaggedToId == tagged
                                  select new
                                  {
                                      st.id,
                                      em.empId,
                                      EmployeeName= string.Concat(em.fName," ",em.lName)
                                  }).ToListAsync();
                if (taggingdata.Any())
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Data = taggingdata
                    };
                }
                else
                {
                    return new ServiceStatusResponseModel { Success = false, Message = "No Tagging Found" };
                }
            }
            catch (Exception ex)
            {
               return new ServiceStatusResponseModel { Success = false, Message=ex.Message };

            }
        }
    }
}
