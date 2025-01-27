using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Services
{
    public class observationReferenceRangesServices : IobservationReferenceRangesServices
    {
        private readonly ContextClass db;
        public observationReferenceRangesServices(ContextClass context, ILogger<BaseController<observationReferenceRanges>> logger)
        {
            db = context;
        }
        async Task<ServiceStatusResponseModel> IobservationReferenceRangesServices.SaveUpdateReferenceRange(List<observationReferenceRanges> ObservationReferenceRanges)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                if (ObservationReferenceRanges == null)
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = "Data not available"
                    };
                }
                try
                {
                    ObservationReferenceRanges.ToList().ForEach(refrencerange =>
                    {
                        if (refrencerange.id == 0)
                        {
                            var ObservationRangesMpping = ObservationRangesData(refrencerange);
                            db.observationReferenceRanges.Add(ObservationRangesMpping);
                        }
                        else
                        {
                            var oldRanges = db.observationReferenceRanges.FirstOrDefault(o => o.id == refrencerange.id);
                            if (oldRanges != null)
                            {
                                updateRefrenceRange(refrencerange, oldRanges);
                                db.observationReferenceRanges.Update(oldRanges);
                            }
                        }
                    });
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "Saved SuccessFul"
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
        }
        private observationReferenceRanges ObservationRangesData(observationReferenceRanges ObservationReferenceRanges)
        {
            return new observationReferenceRanges
            {
                id = ObservationReferenceRanges.id,
                observationId = ObservationReferenceRanges.observationId,
                gender = ObservationReferenceRanges.gender,
                genderTextValue = ObservationReferenceRanges.genderTextValue,
                fromAge = ObservationReferenceRanges.fromAge,
                toAge = ObservationReferenceRanges.toAge,
                minValue = ObservationReferenceRanges.minValue,
                maxValue = ObservationReferenceRanges.maxValue,
                unit = ObservationReferenceRanges.unit,
                reportReading = ObservationReferenceRanges.reportReading,
                defaultValue = ObservationReferenceRanges.defaultValue,
                minCritical = ObservationReferenceRanges.minCritical,
                maxCritical = ObservationReferenceRanges.maxCritical,
                minAutoApprovalValue = ObservationReferenceRanges.minAutoApprovalValue,
                maxAutoApprovalValue = ObservationReferenceRanges.maxAutoApprovalValue,
                centreId = ObservationReferenceRanges.centreId,
                machineID = ObservationReferenceRanges.machineID,
                isActive = ObservationReferenceRanges.isActive,
                createdById = ObservationReferenceRanges.createdById,
                createdDateTime = ObservationReferenceRanges.createdDateTime,
            };
        }

        private void updateRefrenceRange(observationReferenceRanges ObservationReferenceRanges, observationReferenceRanges oldranges)
        {
            oldranges.observationId = ObservationReferenceRanges.observationId;
            oldranges.gender = ObservationReferenceRanges.gender;
            oldranges.genderTextValue = ObservationReferenceRanges.genderTextValue;
            oldranges.fromAge = ObservationReferenceRanges.fromAge;
            oldranges.toAge = ObservationReferenceRanges.toAge;
            oldranges.minValue = ObservationReferenceRanges.minValue;
            oldranges.maxValue = ObservationReferenceRanges.maxValue;
            oldranges.unit = ObservationReferenceRanges.unit;
            oldranges.reportReading = ObservationReferenceRanges.reportReading;
            oldranges.defaultValue = ObservationReferenceRanges.defaultValue;
            oldranges.minCritical = ObservationReferenceRanges.minCritical;
            oldranges.maxCritical = ObservationReferenceRanges.maxCritical;
            oldranges.minAutoApprovalValue = ObservationReferenceRanges.minAutoApprovalValue;
            oldranges.maxAutoApprovalValue = ObservationReferenceRanges.maxAutoApprovalValue;
            oldranges.centreId = ObservationReferenceRanges.centreId;
            oldranges.machineID = ObservationReferenceRanges.machineID;
            oldranges.isActive = ObservationReferenceRanges.isActive;
            oldranges.updateById = ObservationReferenceRanges.updateById;
            oldranges.updateDateTime = ObservationReferenceRanges.updateDateTime;

        }

        async Task<ServiceStatusResponseModel> IobservationReferenceRangesServices.DeleteReferenceRange(int Id)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var result = await db.observationReferenceRanges.Where(o => o.id == Id).FirstOrDefaultAsync();
                    if (result != null)
                    {
                        db.observationReferenceRanges.Remove(result);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Deleted Successful"
                        };
                    }
                    else
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "Please use correct ID"
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
