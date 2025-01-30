using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Services
{
    public class itemOutHouseServices : IitemOutHouseServices
    {
        private readonly ContextClass db;
        public itemOutHouseServices(ContextClass context, ILogger<BaseController<item_OutHouseMaster>> logger)
        {
            db = context;
        }
        async Task<ServiceStatusResponseModel> IitemOutHouseServices.GetOutHouseMapping(int BookingCentre, int ProcessingCentre, int DeptId)
        {
            try
            {
                var data = await (from io in db.item_OutHouseMaster
                            join im in db.itemMaster on io.itemId equals im.itemId
                            join cm in db.centreMaster on io.bookingCentreId equals cm.centreId
                            join cm1 in db.centreMaster on io.ProcessingLabId equals cm1.centreId
                            join ld in db.labDepartment on io.departmentId equals ld.id
                            where io.departmentId == DeptId && io.bookingCentreId == BookingCentre
                            && io.ProcessingLabId == ProcessingCentre
                            select new
                            {io.id, io.bookingCentreId,
                            BookingCentre=cm.companyName,
                            io.ProcessingLabId,
                            ProcessingCentre=cm1.companyName,
                            io.departmentId,
                            ld.deptName,
                            io.itemId,
                            im.itemName,io.isActive
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

        async Task<ServiceStatusResponseModel> IitemOutHouseServices.RemoveOutHouseMapping(int id)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var data = db.item_OutHouseMaster.Where(i => i.id == id).FirstOrDefault();
                    if (data != null)
                    {
                        db.item_OutHouseMaster.Remove(data);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Mapping Removed Successful"
                        };
                    }
                    else
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "Please use correct Id"
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

        async Task<ServiceStatusResponseModel> IitemOutHouseServices.SaveOutHouseMapping(List<item_OutHouseMaster> OutHouseMapping)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                if (OutHouseMapping == null)
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = "Data not available"
                    };
                }
                try
                {
                    foreach (var item in OutHouseMapping)
                    {
                        var count = db.item_OutHouseMaster.Where(i => i.itemId == item.itemId && i.bookingCentreId == item.bookingCentreId && i.ProcessingLabId == item.ProcessingLabId).Count();
                        if (count == 0)
                        {
                            var OutHouseMpping = CreateOutHouseMapping(item);
                            db.item_OutHouseMaster.AddRange(OutHouseMpping);
                            await db.SaveChangesAsync();
                        }
                    }
                    await transaction.CommitAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "Saved SuccessFul"
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


        private item_OutHouseMaster CreateOutHouseMapping(item_OutHouseMaster item)
        {
            return new item_OutHouseMaster
            {
                id = 0,
                ProcessingLabId = item.ProcessingLabId,
                bookingCentreId = item.bookingCentreId,
                itemId = item.itemId,
                departmentId = item.departmentId,
                isActive = item.isActive,
                createdById = item.createdById,
                createdDateTime = item.createdDateTime,
            };
        }
    }
}
