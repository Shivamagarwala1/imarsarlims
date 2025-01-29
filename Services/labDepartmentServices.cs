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

        async Task<ServiceStatusResponseModel> IlabDepartmentServices.UpdateDepartmentOrder(List<DepartmentOrderModel> DepartmentOrder)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var departmentIds = DepartmentOrder.Select(d => d.deptId).ToList();

                    var departmentsToUpdate = await db.labDepartment
                                                      .Where(d => departmentIds.Contains(d.id))
                                                      .ToListAsync();

                    var departmentOrderDict = DepartmentOrder.ToDictionary(d => d.deptId, d => d.order);

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
                        Message = "Update Successful"
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

    }
}
