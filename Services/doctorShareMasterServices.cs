using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Services
{
    public class doctorShareMasterServices : IdoctorShareMasterServices
    {
        private readonly ContextClass db;
        public doctorShareMasterServices(ContextClass context, ILogger<BaseController<bank_master>> logger)
        {
            db = context;
        }


        public async Task<ServiceStatusResponseModel> GetDoctorShareData(int doctorId, int departmentId, int centreId, int type, string typeWise)
        {
            try
            {
                object data;

                if (typeWise == "item")
                {
                    data = await (from im in db.itemMaster
                                  join dsm in db.doctorShareMaster
                                  on im.itemId equals dsm.ItemID into dsmGroup
                                  from dsm in dsmGroup.DefaultIfEmpty()
                                  where im.deptId == departmentId && (dsm == null || (dsm.Doctorid == doctorId && dsm.Centreid == centreId)&& (dsm == null || dsm.type==type))
                                  select new
                                  {
                                      id = (dsm != null) ? dsm.id : 0,
                                      im.itemId,
                                      im.itemName,
                                      im.deptId,
                                      deptname = "",
                                      absorbedBy = (dsm != null) ? dsm.absorbedBy : 0,
                                      percentage = (dsm != null) ? dsm.percentage : 0,
                                      Amount = (dsm != null) ? dsm.Amount : 0
                                  }).ToListAsync();
                }
                else
                {
                    data = await (from ld in db.labDepartment
                                  join dsm in db.doctorShareMaster
                                  on ld.id equals dsm.deptid into dsmGroup
                                  from dsm in dsmGroup.Where(x => (x.Centreid == null || x.Centreid == centreId) && (x.Doctorid == null || x.Doctorid == doctorId) && x.type==type).DefaultIfEmpty()
                                  select new
                                  {
                                      id = (dsm != null) ? dsm.id : 0,
                                      itemid = 0,
                                      itemName = "",
                                      DeptId = ld.id,
                                      ld.deptName,
                                      absorbedBy = (dsm != null) ? dsm.absorbedBy : 0,
                                      percentage = (dsm != null) ? dsm.percentage : 0,
                                      Amount = (dsm != null) ? dsm.Amount : 0
                                  }).ToListAsync();
                }

                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                // Optional: Log the exception
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = $"Error retrieving doctor share data: {ex.Message}"
                };
            }
        }

        async Task<ServiceStatusResponseModel> IdoctorShareMasterServices.SaveUpdateDoctorShareData(List<doctorShareMaster> shareData)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var newItems = shareData.Where(item => item.id == 0).Select(item => CreateShareData(item)).ToList();
                    db.doctorShareMaster.AddRange(newItems);
                    var existingIds = shareData.Where(item => item.id != 0).Select(item => item.id).ToList();
                    var existingItems = await db.doctorShareMaster.Where(t => existingIds.Contains(t.id)).ToListAsync();

                    foreach (var item in shareData.Where(item => item.id != 0))
                    {
                        var existingItem = existingItems.FirstOrDefault(t => t.id == item.id);
                        if (existingItem != null)
                        {
                            updateShareData(existingItem, item);
                        }
                    }

                    db.doctorShareMaster.UpdateRange(existingItems);

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
        private doctorShareMaster CreateShareData(doctorShareMaster newdata)
        {
            return new doctorShareMaster
            {
                id = newdata.id,
                Doctorid = newdata.Doctorid,
                deptid = newdata.deptid,
                ItemID = newdata.ItemID,
                Centreid = newdata.Centreid,
                percentage = newdata.percentage,
                Amount = newdata.Amount,
                absorbedBy = newdata.absorbedBy,
                type = newdata.type,
                CreatedBYID = newdata.CreatedBYID,
                createdbyName = newdata.createdbyName,
                createdDate = newdata.createdDate
            };
        }

        private void updateShareData(doctorShareMaster olddata, doctorShareMaster newdata)
        {
            olddata.Doctorid = newdata.Doctorid;
            olddata.deptid = newdata.deptid;
            olddata.ItemID = newdata.ItemID;
            olddata.Centreid = newdata.Centreid;
            olddata.percentage = newdata.percentage;
            olddata.Amount = newdata.Amount;
            olddata.type = newdata.type;
            olddata.absorbedBy = newdata.absorbedBy;
            olddata.CreatedBYID = newdata.CreatedBYID;
            olddata.createdbyName = newdata.createdbyName;
            olddata.createdDate = newdata.createdDate;
        }

    }
}
