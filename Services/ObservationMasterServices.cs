using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Services
{
    public class ObservationMasterServices : IObservationMasterServices
    {
        private readonly ContextClass db;

        public ObservationMasterServices(ContextClass context, ILogger<BaseController<itemObservationMaster>> logger)
        {
            db = context;
        }
        async Task<ServiceStatusResponseModel> IObservationMasterServices.SaveUpdateObservationMater(itemObservationMaster Observation)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    if(Observation.id==0)
                    {
                        var count=  db.itemObservationMaster.Where(i=> i.labObservationName==Observation.labObservationName).Count();
                        if (count == 0)
                        {
                            db.itemObservationMaster.Add(Observation);
                            await db.SaveChangesAsync();
                            await transaction.CommitAsync();
                            return new ServiceStatusResponseModel
                            {
                                Success = true,
                                Message = "Observation Saved Successful"
                            };
                        }
                        else
                        {
                            return new ServiceStatusResponseModel
                            {
                                Success = false,
                                Message = "Observation Name All Ready Exist"
                            };
                        }
                    }
                    else
                    {
                        var count = db.itemObservationMaster.Where(i => i.labObservationName == Observation.labObservationName && i.id != Observation.id).Count();
                        if(count == 0) 
                        {
                        db.itemObservationMaster.Update(Observation);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                            return new ServiceStatusResponseModel
                            {
                                Success = true,
                                Message = "Observation Updated Successful"
                            };
                        }
                        else
                        {
                            return new ServiceStatusResponseModel
                            {
                                Success = false,
                                Message = "Observation Name All Ready Exist"
                            };
                        }
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
