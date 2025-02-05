using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Services
{
    
    public class labDepartmentServices : IlabDepartmentServices
    {
        private readonly ContextClass db;
        public labDepartmentServices(ContextClass context, ILogger<BaseController<labDepartment>> logger)
        {

            db = context;
        }

        async Task<ServiceStatusResponseModel> IlabDepartmentServices.UpdateLabDepartmentStatus(int id, byte status, int Userid)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var data = db.labDepartment.Where(d => d.id == id).FirstOrDefault();
                    if (data == null)
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "No Data Found to Update"
                        };
                    }
                    else
                    {
                        data.isActive = status;
                        data.updateById = Userid;
                        data.updateDateTime = DateTime.Now;
                        db.labDepartment.Update(data);
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
                    await transaction.RollbackAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = ex.Message
                    };
                }
            }
        }

        async Task<ServiceStatusResponseModel> IlabDepartmentServices.SaveUpdateLabDepartment(labDepartment Department)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    if(Department.id==0)
                    {
                        db.labDepartment.Add(Department);
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
                        db.labDepartment.Update(Department);
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
                    await transaction.RollbackAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = ex.Message
                    };
                }
            }
        }

        async Task<ServiceStatusResponseModel> IlabDepartmentServices.UpdateDepartmentOrder(List<DepartmentOrderModel> DepartmentOrder, string type)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var Ids = DepartmentOrder.Select(d => d.id).ToList();
                    if (type == "Department")
                    {
                        var departmentsToUpdate = await db.labDepartment
                                                          .Where(d => Ids.Contains(d.id))
                                                          .ToListAsync();

                        var departmentOrderDict = DepartmentOrder.ToDictionary(d => d.id, d => d.order);

                        foreach (var department in departmentsToUpdate)
                        {
                            if (departmentOrderDict.ContainsKey(department.id))
                            {
                                department.printSequence = departmentOrderDict[department.id];
                            }
                        }
                        db.labDepartment.UpdateRange(departmentsToUpdate);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();

                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Department Order Update Successful"
                        };
                    }
                    else
                    {
                        var departmentsToUpdate = await db.itemMaster
                                                          .Where(d => Ids.Contains(d.itemId))
                                                          .ToListAsync();

                        var departmentOrderDict = DepartmentOrder.ToDictionary(d => d.id, d => d.order);

                        foreach (var department in departmentsToUpdate)
                        {
                            if (departmentOrderDict.ContainsKey(department.itemId))
                            {
                                department.displaySequence = departmentOrderDict[department.itemId];
                            }
                        }
                        db.itemMaster.UpdateRange(departmentsToUpdate);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();

                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Investigation Order Update Successful"
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

    }
}
