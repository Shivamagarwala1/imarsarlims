using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Services
{
    public class tatMasterServices : ItatMasterServices
    {
        private readonly ContextClass db;

        public tatMasterServices(ContextClass context, ILogger<BaseController<tat_master>> logger)
        {
            db = context;
        }
        async Task<ServiceStatusResponseModel> ItatMasterServices.GetTatMaster(int centreId, int departmentId, List<int> ItemIds)
        {
            try
            {
                var data = (from im in db.itemMaster
                            join tm in db.tat_master on im.itemId equals tm.itemid into tmGroup
                            from tm in tmGroup.DefaultIfEmpty()
                            where im.deptId == departmentId && ItemIds.Contains(im.itemId) && (tm.centreid== null || tm.centreid == centreId)
                            select new
                            {
                                id = tm != null ? tm.id : 0,
                                im.itemId,
                                im.itemName,
                                StartTime = tm != null && tm.StartTime.HasValue ? tm.StartTime.Value.ToString(@"hh\:mm\:ss") : string.Empty,
                                EndTime = tm != null && tm.EndTime.HasValue ? tm.EndTime.Value.ToString(@"hh\:mm\:ss") : string.Empty,
                                Mins = tm != null ? tm.Mins : 0,
                                Days = tm != null ? tm.Days : 0,
                                Sun = tm != null ? tm.Sun : 0,
                                Mon = tm != null ? tm.Mon : 0,
                                Tue = tm != null ? tm.Tue : 0,
                                Wed = tm != null ? tm.Wed : 0,
                                Thu = tm != null ? tm.Thu : 0,
                                Fri = tm != null ? tm.Fri : 0,
                                Sat = tm != null ? tm.Sat : 0,
                                Regcoll = tm != null ? tm.Regcoll : 0,
                                Collrecv = tm != null ? tm.collrecv : 0,
                                TATType = tm != null ? tm.TATType : "Days"
                            }).ToList();
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

        async Task<ServiceStatusResponseModel> ItatMasterServices.SaveUpdateTatMaster(List<tat_master> Tatdata)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var newItems = Tatdata.Where(item => item.id == 0).Select(item => createTatData(item)).ToList();
                    db.tat_master.AddRange(newItems);
                   var existingIds = Tatdata.Where(item => item.id != 0).Select(item => item.id).ToList();
                    var existingItems = await db.tat_master.Where(t => existingIds.Contains(t.id)).ToListAsync();

                    foreach (var item in Tatdata.Where(item => item.id != 0))
                    {
                        var existingItem = existingItems.FirstOrDefault(t => t.id == item.id);
                        if (existingItem != null)
                        {
                            updateTatData(existingItem, item);
                        }
                    }

                   db.tat_master.UpdateRange(existingItems);

                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "Data saved/updated successfully."
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(); // Rollback in case of an error
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = ex.Message
                    };
                }
            }
        }

        private tat_master createTatData(tat_master item)
        {
            return new tat_master
            {
                id = item.id,
                centreid = item.centreid,
                Deptid = item.Deptid,
                itemid = item.itemid,
                StartTime = item.StartTime,
                EndTime = item.EndTime,
                Mins = item.Mins,
                Days = item.Days,
                Sun = item.Sun,
                Mon = item.Mon,
                Tue = item.Tue,
                Wed = item.Wed,
                Thu = item.Thu,
                Fri = item.Fri,
                Sat = item.Sat,
                Regcoll = item.Regcoll,
                collrecv = item.collrecv,
                CreatedOn = item.CreatedOn,
                Createdby = item.Createdby,
                CreatedByName = item.CreatedByName,
                TATType = item.TATType,
            };
        }

        private void updateTatData(tat_master olddata, tat_master newdata)
        {
            olddata.centreid = newdata.centreid;
            olddata.Deptid = newdata.Deptid;
            olddata.itemid = newdata.itemid;
            olddata.StartTime = newdata.StartTime;
            olddata.EndTime = newdata.EndTime;
            olddata.Mins = newdata.Mins;
            olddata.Days = newdata.Days;
            olddata.Sun = newdata.Sun;
            olddata.Mon = newdata.Mon;
            olddata.Tue = newdata.Tue;
            olddata.Wed = newdata.Wed;
            olddata.Thu = newdata.Thu;
            olddata.Fri = newdata.Fri;
            olddata.Sat = newdata.Sat;
            olddata.Regcoll = newdata.Regcoll;
            olddata.collrecv = newdata.collrecv;
            olddata.CreatedOn = newdata.CreatedOn;
            olddata.Createdby = newdata.Createdby;
            olddata.CreatedByName = newdata.CreatedByName;
            olddata.TATType = newdata.TATType;
        }

    }
}
