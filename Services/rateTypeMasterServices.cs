using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MySqlX.XDevAPI.CRUD;
using static Org.BouncyCastle.Utilities.Test.FixedSecureRandom;

namespace iMARSARLIMS.Services
{
    public class rateTypeMasterServices : IrateTypeMasterServices
    {
        private readonly ContextClass db;
        public rateTypeMasterServices(ContextClass context, ILogger<BaseController<rateTypeMaster>> logger)
        {

            db = context;
        }

        async Task<ServiceStatusResponseModel> IrateTypeMasterServices.GetRatetypeTagging()
        {
            try
            {
                var result = await (from rt in db.rateTypeMaster
                                    join rtt in db.rateTypeTagging on rt.id equals rtt.rateTypeId into rttGroup
                                    from rtt in rttGroup.DefaultIfEmpty()
                                    join cm in db.centreMaster on rtt.centreId equals cm.centreId into cmGroup
                                    from cm in cmGroup.DefaultIfEmpty()
                                    select new
                                    {
                                        rt.id,
                                        rt.rateName,
                                        companyName = cm != null ? cm.companyName : "",  // Default to "Unknown" if cm is null
                                        centreId = cm != null ? cm.centreId : (int?)null  // Default to null if cm is null

                                    }).ToListAsync();
                var resultGroupBy = result.GroupBy(r => r.id).Select(g => new
                                    {
                                       
                                        Id = g.FirstOrDefault()?.id, // Get the first item's id from the group
                                        RateName = g.FirstOrDefault()?.rateName, // Get the first machineName
                                        CentreName = string.Join(", ", g.Select(r => r.companyName)), // Join TestName values
                                        CentreId= string.Join(", ", g.Select(r => r.centreId))
                }).ToList();
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = resultGroupBy
                };
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }
        async Task<ServiceStatusResponseModel> IrateTypeMasterServices.SaveUpdateRateType(int rateTypeId, string rateTypeName, string CentreId, int userId)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var rateId = 0;
                    if (rateTypeId == 0)
                    {
                        var count = db.rateTypeMaster.Where(r => r.rateType == rateTypeName).Count();
                        if (count > 0)
                        {
                            return new ServiceStatusResponseModel
                            {
                                Success = false,
                                Message = " RateType Already exist"
                            };
                        }
                        var rateType = createRateType(rateTypeId, rateTypeName, userId);
                        var rateTypeData = db.rateTypeMaster.Add(rateType);
                        await db.SaveChangesAsync();
                        rateId = rateTypeData.Entity.id;
                    }
                    else
                    {
                        rateId = rateTypeId;
                        var count = db.rateTypeMaster.Where(r => r.rateType == rateTypeName && r.id == rateTypeId).Count();
                        if (count > 0)
                        {
                            return new ServiceStatusResponseModel
                            {
                                Success = false,
                                Message = " RateType Already exist"
                            };
                        }
                        var oldratetype = db.rateTypeMaster.Where(r => r.id == rateTypeId).FirstOrDefault();
                        if (oldratetype != null)
                        {
                            oldratetype.rateType = rateTypeName;
                            oldratetype.rateName = rateTypeName;
                            db.rateTypeMaster.Update(oldratetype);
                            await db.SaveChangesAsync();
                        }
                    }

                    var centreList = CentreId.Split(',').Select(c => int.Parse(c.Trim())).ToList();
                    var tasks = centreList.Select(centre =>
                        Task.Run(async () =>
                        {
                            var data = CreateRateTypeTagging(centre, rateId, userId);
                            db.rateTypeTagging.Add(data);
                        })).ToList();

                    await Task.WhenAll(tasks);
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
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = ex.Message
                    };
                }
            }
        }
        private rateTypeTagging CreateRateTypeTagging(int centreid, int rateTypeId,int userId)
        {
            return new rateTypeTagging
            {
                id=0,
                centreId = centreid,
                rateTypeId = rateTypeId,
                isActive= 1,
                createdById = userId,
                createdDateTime = DateTime.Now
            };
        }
        private rateTypeMaster createRateType(int rateTypeId, string rateTypeName, int userId)
        {
            return new rateTypeMaster
            {
                id = rateTypeId,
                rateName = rateTypeName,
                rateTypeId = rateTypeId,
                rateType = rateTypeName,
                isActive = 1,
                createdById = userId,
                createdDateTime = DateTime.Now,
            };
        }
    }
}
