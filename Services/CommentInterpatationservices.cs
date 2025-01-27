using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Services
{
    public class CommentInterpatationservices : ICommentInterpatationservices
    {
        private readonly ContextClass db;
        public CommentInterpatationservices(ContextClass context, ILogger<BaseController<observationReferenceRanges>> logger)
        {
            db = context;
        }
        async Task<ServiceStatusResponseModel> ICommentInterpatationservices.SaveInterpatation(itemInterpretation Interpretation)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    if (Interpretation == null)
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "Interpretation data incorrect"
                        };
                    }

                    string msg = "";
                    if (Interpretation.id == 0)
                    {
                        db.itemInterpretation.Add(Interpretation);
                        msg = "Saved successfully";
                    }
                    else
                    {
                        var data = await db.itemInterpretation.Where(i => i.id == Interpretation.id).FirstOrDefaultAsync();
                        if (data != null)
                        {
                            var logdata = createinterpatationlog(data);
                            db.itemInterpretationLog.Add(logdata);
                            UpdateinterpatationData(data, Interpretation);
                            db.itemInterpretation.Update(data);
                            msg = "Updated successfully";
                        }
                        else
                        {
                            return new ServiceStatusResponseModel
                            {
                                Success = false,
                                Message = "Wrong Interpretation ID"
                            };
                        }
                    }

                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = msg
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = "An error occurred: " + ex.Message
                    };
                }
            }
        }

        private itemInterpretationLog createinterpatationlog(itemInterpretation interpatationData)
        {
            return new itemInterpretationLog
            {
                id = 0,
                interpatationid = interpatationData.id,
                itemId = interpatationData.itemId,
                interpretation = interpatationData.interpretation,
                centreId = interpatationData.centreId,
                showinPackages = interpatationData.showinPackages,
                showInReport = interpatationData.showInReport,
                isActive = interpatationData.isActive,
                createdById = interpatationData.createdById,
                createdDateTime = interpatationData.createdDateTime
            };
        }

        private void UpdateinterpatationData(itemInterpretation interpatationData, itemInterpretation interpretationNew)
        {
            interpatationData.interpretation = interpretationNew.interpretation;
            interpatationData.showInReport = interpretationNew.showInReport;
            interpatationData.showinPackages = interpretationNew.showinPackages;
            interpatationData.centreId = interpretationNew.centreId;
            interpatationData.itemId = interpretationNew.itemId;
            interpatationData.updateById = interpretationNew.updateById;
            interpatationData.updateDateTime = interpretationNew.updateDateTime;
            interpatationData.isActive = interpretationNew.isActive;
        }

        async Task<ServiceStatusResponseModel> ICommentInterpatationservices.updateInterpatationStatus(int InterpatationId, byte Status, int UserId)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var data = db.itemInterpretation.Where(i => i.id == InterpatationId).FirstOrDefault();
                    if (data != null)
                    {
                        data.isActive = Status;
                        data.updateById = UserId;
                        data.updateDateTime = DateTime.Now;
                        db.itemInterpretation.Update(data);
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
                            Message = "Incorrect Interpatation Id"
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

        async Task<ServiceStatusResponseModel> ICommentInterpatationservices.SaveCommentMaster(itemCommentMaster Comment)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var msg = "";
                    if (Comment == null)
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "Comment data Incorrect"
                        };
                    }
                    else
                    {

                        if (Comment.id == 0)
                        {
                            db.itemCommentMaster.Add(Comment);
                            await db.SaveChangesAsync();
                            await transaction.CommitAsync();
                            msg = "Saved Successful";
                        }
                        else
                        {
                            db.itemCommentMaster.Update(Comment);
                            await db.SaveChangesAsync();
                            await transaction.CommitAsync();
                            msg = "Updated Successful";
                        }

                    }
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = msg
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

        async Task<ServiceStatusResponseModel> ICommentInterpatationservices.updateCommentStatus(int CommentId, byte Status, int UserId)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var data = db.itemCommentMaster.Where(i => i.id == CommentId).FirstOrDefault();
                    if (data != null)
                    {
                        data.isActive = Status;
                        data.updateById = UserId;
                        data.updateDateTime = DateTime.Now;
                        db.itemCommentMaster.Update(data);
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
                            Message = "Incorrect Comment Id"
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

        public async Task<ServiceStatusResponseModel> GetCommentData(int CentreID, string type, int testid)
        {
            if(type== "Item Wise")
            {
                var result= await (from ic in db.itemCommentMaster 
                                   join im in db.itemMaster on ic.itemId equals im.itemId
                                   join cm in db.centreMaster on ic.centreId equals cm.centreId
                                   where ic.itemId==testid && ic.centreId==CentreID
                                   select new
                                   {
                                       ic.id,
                                       ic.type,
                                       im.itemName,
                                       cm.companyName,
                                       ic.template,
                                       ic.templateName,
                                       ic.isActive
                                   }).ToListAsync();
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = result
                };
            }
            else
            {
                var result = await (from ic in db.itemCommentMaster
                                    join im in db.itemObservationMaster on ic.itemId equals im.id
                                    join cm in db.centreMaster on ic.centreId equals cm.centreId
                                    where ic.itemId == testid && ic.centreId==CentreID
                                    select new
                                    {
                                        ic.id,
                                        ic.type,
                                        itemName= im.labObservationName,
                                        cm.companyName,
                                        ic.template,
                                        ic.templateName,
                                        ic.isActive
                                    }).ToListAsync();
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = result
                };
            }
        }
    }
}
