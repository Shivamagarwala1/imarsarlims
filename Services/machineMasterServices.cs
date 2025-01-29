using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Services
{
    public class machineMasterServices : ImachineMasterServices
    {
        private readonly ContextClass db;
        public machineMasterServices(ContextClass context, ILogger<BaseController<machineMaster>> logger)
        {
            db = context;
        }

        public async Task<ServiceStatusResponseModel> SaveUpdateMapping(List<machineObservationMapping> MachineMappings)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var assayno = MachineMappings.Select(x => x.assay).FirstOrDefault();
                    var data= db.machineObservationMapping.Where(mo=>mo.assay==assayno).ToList();
                    db.machineObservationMapping.RemoveRange(data);
                    await db.SaveChangesAsync();
                    var newObservations = MachineMappings.Where(map => map.id == 0).Select(CreateObservationMapping).ToList();
                    if (newObservations.Any())
                    {
                        db.machineObservationMapping.AddRange(newObservations);
                    }
                    //var existingMappingsIds = MachineMappings.Where(item => item.id != 0).Select(item => item.id).ToList();
                    //var existingMappings = await db.machineObservationMapping
                    //    .Where(b => existingMappingsIds.Contains(b.id))
                    //    .ToListAsync();

                    //// Update existing mappings
                    //var updatedObservations = new List<machineObservationMapping>();
                    //foreach (var mapping in MachineMappings.Where(item => item.id != 0))
                    //{
                    //    var machineObservationMappingOld = existingMappings.FirstOrDefault(b => b.id == mapping.id);
                    //    if (machineObservationMappingOld != null)
                    //    {
                    //        updateObservationmapping(machineObservationMappingOld, mapping);
                    //        updatedObservations.Add(machineObservationMappingOld);
                    //    }
                    //}

                    //// Apply updates
                    //if (updatedObservations.Any())
                    //{
                    //    db.machineObservationMapping.UpdateRange(updatedObservations);
                    //}

                    // Save changes and commit the transaction
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "Mapped Successfully"
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

        private void updateObservationmapping(machineObservationMapping data, machineObservationMapping mapping)
        {
            data.machineId = mapping.machineId;
            data.assay = mapping.assay;
            data.labTestID = mapping.labTestID;
            data.isOrderable = mapping.isOrderable;
            data.formula = mapping.formula;
            data.roundUp = mapping.roundUp;
            data.multiplication = mapping.multiplication;
            data.suffix = mapping.suffix;
            data.isActive = mapping.isActive;
            data.updateDateTime = mapping.updateDateTime;
            data.updateById = mapping.updateById;
            
        }

        private machineObservationMapping CreateObservationMapping(machineObservationMapping mapping)
        {
            return new machineObservationMapping
            {
                machineId = mapping.machineId,
                assay = mapping.assay,
                labTestID = mapping.labTestID,
                isOrderable = mapping.isOrderable,
                formula = mapping.formula,
                roundUp = mapping.roundUp,
                multiplication = mapping.multiplication,
                suffix = mapping.suffix,
                isActive = mapping.isActive,
                createdById = mapping.createdById,
                createdDateTime = mapping.createdDateTime,
            };
        }

        async Task<ServiceStatusResponseModel> ImachineMasterServices.SaveMachineMaster(machineMaster Machine)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var count = db.machineMaster.Where(m => m.machineName == Machine.machineName).Count();
                    if (count > 0)
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "Machine Name Already Exist"
                        };
                    }
                    else
                    {
                        db.machineMaster.Add(Machine);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Saved Successful"
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
