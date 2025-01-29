using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using static Google.Cloud.Dialogflow.V2.MessageEntry.Types;

namespace iMARSARLIMS.Services
{
    [Route("api/[controller]")]
    [ApiController]
    public class roleMenuAccessServices : IroleMenuAccessServices
    {
        private readonly ContextClass db;
        public roleMenuAccessServices(ContextClass context, ILogger<BaseController<tnx_BookingPatient>> logger)
        {
            db = context;
        }

        async Task<ServiceStatusResponseModel> IroleMenuAccessServices.EmpPageAccessRemove(int Id)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var menuAccess = await db.roleMenuAccess.Where(r => r.id == Id).FirstOrDefaultAsync();
                    if (menuAccess == null)
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "Menu access not found"
                        };
                    }

                    db.roleMenuAccess.Remove(menuAccess);
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "Access Removed Successfully"
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(); 
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = $"Error: {ex.Message}"
                    };
                }
            }
        }


        async Task<ServiceStatusResponseModel> IroleMenuAccessServices.GetAllRoleMenuAcess(ODataQueryOptions<roleMenuAccess> queryOptions)
        {
            try
            {
                var rawData = db.roleMenuAccess.AsQueryable();

                var filteredData = (IQueryable<roleMenuAccess>)queryOptions.Filter
                    .ApplyTo(rawData, new ODataQuerySettings
                    {
                        HandleNullPropagation = HandleNullPropagationOption.False
                    });

                var totalCount = await filteredData.CountAsync();

                var paginatedData = queryOptions.ApplyTo(filteredData, new ODataQuerySettings
                {
                    HandleNullPropagation = HandleNullPropagationOption.False
                }).Cast<roleMenuAccess>();

                var AccessData = await (from pd in paginatedData
                                        join em in db.empMaster on pd.employeeId equals em.empId
                                        join mm in db.menuMaster on pd.menuId equals mm.id
                                        join mm1 in db.menuMaster on pd.subMenuId equals mm1.id
                                        join rm in db.roleMaster on pd.roleId equals rm.id
                                        select new
                                        {
                                            pd.id,
                                            Name = em.fName + " " + em.lName,
                                            MenuMame = mm.menuName,
                                            SubMenuName = mm1.menuName,
                                            rm.roleName
                                        }).ToListAsync();

                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = AccessData,
                    Count = totalCount
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

        async Task<ServiceStatusResponseModel> IroleMenuAccessServices.SaveRoleMenuAccess(RoleMenuAccessRequestModel roleMenu)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    if (roleMenu != null)
                    {
                        // Ensure employeeId and subMenuId are not null or empty
                        if (!string.IsNullOrEmpty(roleMenu.subMenuId) && !string.IsNullOrEmpty(roleMenu.employeeId))
                        {
                            var subMenuIds = roleMenu.subMenuId.Split(',').Select(id => id.Trim()).ToList();
                            var employeeIds = roleMenu.employeeId.Split(',').Select(id => id.Trim()).ToList();

                            foreach (var subMenuId in subMenuIds)
                            {
                                foreach (var employeeId in employeeIds)
                                {
                                    var count= db.roleMenuAccess.Where(rm=>rm.subMenuId== Convert.ToInt32(subMenuId) && rm.employeeId== Convert.ToInt32(employeeId) && rm.roleId == roleMenu.roleId).Count();
                                    if (count == 0)
                                    {
                                        var roleMenuAccess = new roleMenuAccess
                                        {
                                            roleId = roleMenu.roleId,
                                            menuId = roleMenu.menuId,
                                            subMenuId = Convert.ToInt32(subMenuId), // Correctly assigning the subMenuId
                                            employeeId = Convert.ToInt32(employeeId),
                                            isActive = true
                                        };
                                        db.roleMenuAccess.Add(roleMenuAccess);
                                    }
                                }
                            }

                            await db.SaveChangesAsync(); // Save changes before committing the transaction
                            await transaction.CommitAsync();

                            return new ServiceStatusResponseModel
                            {
                                Success = true,
                                Message = "Saved Successfully"
                            };
                        }
                        else
                        {
                            return new ServiceStatusResponseModel
                            {
                                Success = false,
                                Message = "No SubMenu or Employee IDs found to save."
                            };
                        }
                    }
                    else
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "No Data Found to Save"
                        };
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = ex.InnerException?.Message ?? "An error occurred."
                    };
                }
            }
        }
    }
}
