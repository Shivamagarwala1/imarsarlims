using System.Transactions;
using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Account;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Services
{
    public class LocationsServices : ILocationsServices
    {
        private readonly ContextClass db;
        public LocationsServices(ContextClass context, ILogger<BaseController<cityMaster>> logger)
        {
            db = context;
        }
        async Task<ServiceStatusResponseModel> ILocationsServices.SaveCity(cityMaster City)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var count = db.cityMaster.Where(c => c.cityName == City.cityName).Count();
                    if (count > 0)
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "City already Exist"
                        };
                    }
                    else
                    {
                        db.cityMaster.Add(City);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "City Created"
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

        async Task<ServiceStatusResponseModel> ILocationsServices.SaveDistrict(districtMaster District)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var count = db.districtMaster.Where(c => c.district == District.district).Count();
                    if (count > 0)
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "District already Exist"
                        };
                    }
                    else
                    {
                        db.districtMaster.Add(District);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "District Created"
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

        async Task<ServiceStatusResponseModel> ILocationsServices.SaveState(stateMaster State)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var count = db.stateMaster.Where(c => c.state == State.state).Count();
                    if (count > 0)
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "State already Exist"
                        };
                    }
                    else
                    {
                        db.stateMaster.Add(State);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "State Created"
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
    }
}
