using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Services
{
    public class organismAntibioticMasterServices : IorganismAntibioticMasterServices
    {
        private readonly ContextClass db;
        public organismAntibioticMasterServices(ContextClass context, ILogger<BaseController<organismAntibioticMaster>> logger)
        {
            db = context;
        }
        async Task<ServiceStatusResponseModel> IorganismAntibioticMasterServices.SaveUpdateOrganismAntibiotic(organismAntibioticMaster OrganismAntibiotic)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    if (OrganismAntibiotic.id == 0)
                    {
                        db.organismAntibioticMaster.Add(OrganismAntibiotic);
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
                        var count = db.organismAntibioticMaster.Where(o => o.id == OrganismAntibiotic.id).Count();
                        if (count > 0)
                        {
                            db.organismAntibioticMaster.Update(OrganismAntibiotic);
                            await db.SaveChangesAsync();
                            await transaction.CommitAsync();
                            return new ServiceStatusResponseModel
                            {
                                Success = true,
                                Message = "Updated Succcessful"
                            };
                        }
                        else
                        {
                            return new ServiceStatusResponseModel
                            {
                                Success = false,
                                Message = "Data not fount on Id"
                            };
                        }
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

        async Task<ServiceStatusResponseModel> IorganismAntibioticMasterServices.UpdateOrganismAntibioticStatus(int id, byte status, int userId)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var data = db.organismAntibioticMaster.Where(o => o.id == id).FirstOrDefault();
                    if (data != null)
                    {
                        data.isActive = status;
                        data.updateById = userId;
                        data.updateDateTime = DateTime.Now;
                        db.organismAntibioticMaster.Update(data);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Updated Successful"
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
