using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;

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

        async Task<ServiceStatusResponseModel> IroleMenuAccessServices.GetMenuList(menuAccess MenuAccess)
        {
            var employee = await (from rma in db.roleMenuAccess
                                  join mm in db.menuMaster on rma.menuId equals mm.id
                                  join mm1 in db.menuMaster on rma.subMenuId equals mm1.id
                                  where rma.employeeId == MenuAccess.employeeId && rma.roleId == MenuAccess.roleId
                                  orderby mm1.displaySequence
                                  select new
                                  {
                                      rma.roleId,
                                      MainMenu = mm.dispalyName,
                                      SubMenu = mm1.dispalyName,
                                      mm1.navigationUrl
                                  }).ToListAsync();

            if (employee != null)
            {
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = employee
                };
            }

            return new ServiceStatusResponseModel();
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
