using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface.appointment;
using iMARSARLIMS.Model.Appointment;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace iMARSARLIMS.Services.Appointment
{
    public class routeMasterServices : IrouteMasterServices
    {
        private readonly ContextClass db;
        public routeMasterServices(ContextClass context, ILogger<BaseController<tnx_Booking>> logger)
        {
            db = context;
        }

        async Task<ServiceStatusResponseModel> IrouteMasterServices.GetRouteMapping(int PheleboId)
        {
            try
            {
                var data = await (from rm in db.RouteMapping
                                  join rt in db.routeMaster on rm.routeId equals rt.id
                                  join em in db.empMaster on rm.pheleboId equals em.empId
                                  where rm.pheleboId == PheleboId
                                  select new
                                  {
                                      rm.id,
                                      rt.routeName,
                                      rm.routeId,
                                      rm.pheleboId,
                                      rt.pincode,
                                      Phelebo= string.Concat(em.fName," ",em.lName),
                                      rm.isActive

                                  }).ToListAsync();
                if (data != null)
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Data = data
                    };
                }
                else
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = "No Mapping found"
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

        async Task<ServiceStatusResponseModel> IrouteMasterServices.SaveUpdateRouteMapping(List<RouteMapping> Route)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var msg = "";
                    foreach (var routeMapping in Route)
                    {
                        if (routeMapping.id == 0)
                        {
                            db.RouteMapping.Add(routeMapping);
                            await db.SaveChangesAsync();
                            msg = "Saved Succcessful";
                           
                        }
                        else
                        {
                            db.RouteMapping.Update(routeMapping);
                            await db.SaveChangesAsync();
                            msg = "updated Succcessful";

                        }
                    }
                    await transaction.CommitAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "Updated Succcessful"
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

        async Task<ServiceStatusResponseModel> IrouteMasterServices.UpdateRouteMappingStatus(int id, byte status, int Userid)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var data = db.RouteMapping.Where(r => r.id == id).FirstOrDefault();
                    if (data == null)
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "Route Mapping Not Found"
                        };
                    }
                    else
                    {
                        data.isActive = status;
                        data.updateDateTime = DateTime.Now;
                        data.updateById = Userid;
                        db.RouteMapping.Update(data);
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

        async Task<ServiceStatusResponseModel> IrouteMasterServices.SaveUpdateRoute(routeMaster Route)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    if (Route.id == 0)
                    {
                        db.routeMaster.Add(Route);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Saved Succcessful"
                        };
                    }
                    else
                    {

                        db.routeMaster.Update(Route);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Updated Succcessful"
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

        async Task<ServiceStatusResponseModel> IrouteMasterServices.UpdateRouteStatus(int id, byte status, int Userid)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var data = db.routeMaster.Where(r => r.id == id).FirstOrDefault();
                    if (data == null)
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "Route Not Found"
                        };
                    }
                    else
                    {
                        data.isActive = status;
                        data.updateDateTime = DateTime.Now;
                        data.updateById = Userid;
                        db.routeMaster.Update(data);
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
    }
}
