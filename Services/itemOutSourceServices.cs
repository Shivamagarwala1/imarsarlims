using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Services
{
    public class itemOutSourceServices : IitemOutSourceServices
    {
        private readonly ContextClass db;
        public itemOutSourceServices(ContextClass context, ILogger<BaseController<item_outsourcemaster>> logger)
        {
            db = context;
        }
        async Task<ServiceStatusResponseModel> IitemOutSourceServices.GetOutSourceMapping(int BookingCentre, int OutSourceLab, int DeptId)
        {
            try
            {
                var data = await (from io in db.item_outsourcemaster
                                  join im in db.itemMaster on io.itemId equals im.itemId
                                  join cm in db.centreMaster on io.bookingCentreId equals cm.centreId
                                  join ol in db.outSourcelabmaster on io.LabId equals ol.id
                                  join ld in db.labDepartment on io.departmentId equals ld.id
                                  where io.departmentId == DeptId && io.bookingCentreId == BookingCentre
                                  && io.LabId == OutSourceLab
                                  select new
                                  {
                                      io.id,
                                      io.bookingCentreId,
                                      BookingCentre = cm.companyName,
                                      io.LabId,
                                      ProcessingCentre = ol.labName,
                                      io.departmentId,
                                      ld.deptName,
                                      io.itemId,
                                      im.itemName,
                                      io.isActive
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

        async Task<ServiceStatusResponseModel> IitemOutSourceServices.RemoveOutSourceMapping(int id)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var data = db.item_outsourcemaster.Where(i => i.id == id).FirstOrDefault();
                    if (data != null)
                    {
                        db.item_outsourcemaster.Remove(data);
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

        async Task<ServiceStatusResponseModel> IitemOutSourceServices.SaveOutSourceMapping(List<item_outsourcemaster> OutSourceMapping)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                if (OutSourceMapping == null)
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = "Data not available"
                    };
                }
                try
                {
                    foreach (var item in OutSourceMapping)
                    {
                        var count = db.item_outsourcemaster.Where(i => i.itemId == item.itemId && i.bookingCentreId == item.bookingCentreId && i.LabId == item.LabId).Count();
                        if (count == 0)
                        {
                            var OutHouseMpping = CreateOutHouseMapping(item);
                            db.item_outsourcemaster.AddRange(OutHouseMpping);
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


        private item_outsourcemaster CreateOutHouseMapping(item_outsourcemaster item)
        {
            return new item_outsourcemaster
            {
                id = 0,
                LabId = item.LabId,
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
