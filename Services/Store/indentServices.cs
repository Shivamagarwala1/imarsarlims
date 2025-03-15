using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface.Store;
using iMARSARLIMS.Model.Store;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Services.Store
{
    public class indentServices : IindentServices
    {
        private readonly ContextClass db;
        public indentServices(ContextClass context, ILogger<BaseController<Indent>> logger)
        {
            db = context;
        }

        async Task<ServiceStatusResponseModel> IindentServices.CreateIndent(Indent indentdata)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    if (indentdata.indentId == 0)
                    {
                        var IndentDataCreate = CreateIndent(indentdata);
                        var indentDetail = db.Indent.Add(IndentDataCreate);
                        await db.SaveChangesAsync();
                        var indentId = indentDetail.Entity.indentId;
                        await SaveIndentDetail(indentdata.addIndentDetail, indentId);
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Saved Successful"
                        };
                    }
                    else
                    {
                        var indentOld = db.Indent.FirstOrDefault(em => em.indentId == indentdata.indentId);
                        if (indentOld != null)
                        {
                            UpdateIndent(indentOld, indentdata);
                        }
                        var indentDetail = db.Indent.Update(indentOld);
                        await db.SaveChangesAsync();
                        var Id = indentDetail.Entity.indentId;
                        await UpdateIndentDeatail(indentdata.addIndentDetail, Id);
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
                        Message = ex.InnerException?.Message ?? "An error occurred."
                    };

                }
            }

        }

        private Indent CreateIndent(Indent indent)
        {
            return new Indent
            {
                indentId = 0,
                roleId = indent.roleId,
                indentBy = indent.indentBy,
                indentById = indent.indentById,
                indentStatus = indent.indentStatus,
                isrejected = indent.isrejected,
                rejectedBy = indent.rejectedBy,
                RejectDatetime = indent.RejectDatetime,
                isActive = indent.isActive,
                createdById = indent.createdById,
                createdDateTime = DateTime.Now,
                CentreId=indent.CentreId
            };
        }
        private void UpdateIndent(Indent oldindent, Indent indent)
        {
            oldindent.roleId = indent.roleId;
            oldindent.indentBy = indent.indentBy;
            oldindent.indentById = indent.indentById;
            oldindent.indentStatus = indent.indentStatus;
            oldindent.isrejected = indent.isrejected;
            oldindent.rejectedBy = indent.rejectedBy;
            oldindent.RejectDatetime = indent.RejectDatetime;
            oldindent.isActive = indent.isActive;
            oldindent.updateById = indent.updateById;
            oldindent.updateDateTime = DateTime.Now;
            oldindent.CentreId= indent.CentreId;

        }
        private async Task<ServiceStatusResponseModel> UpdateIndentDeatail(IEnumerable<indentDetail> details, int indentId)
        {
            
            if (details != null)
            {
             var indentdetailsave =   details.Where(d=>d.id== 0).ToList();
                var indentDetaillist = indentdetailsave.Select(detailitem => CreateIndentDetail(detailitem, indentId)).ToList();
                if (indentDetaillist.Any())
                {
                    db.indentDetail.AddRange(indentDetaillist);
                    await db.SaveChangesAsync();
                }
                var indentdetailUpdate = details.Where(d => d.id >0).ToList();
                db.indentDetail.UpdateRange(indentdetailUpdate);
                await db.SaveChangesAsync();

            }

            return new ServiceStatusResponseModel
            {
                Success = true,
                Data = ""
            };
        }

        private async Task<ServiceStatusResponseModel> SaveIndentDetail(IEnumerable<indentDetail> detail, int indentid)
        {
            if (detail != null)
            {
                var indentDetaillist = detail.Select(detailitem => CreateIndentDetail(detailitem, indentid)).ToList();
                if (indentDetaillist.Any())
                {
                    db.indentDetail.AddRange(indentDetaillist);
                    await db.SaveChangesAsync();
                }
            }
            return new ServiceStatusResponseModel
            {
                Success = true,
                Data = detail
            };

        }

        private indentDetail CreateIndentDetail(indentDetail detail, int indentid)
        {
            return new indentDetail
            {
                id = 0,
                indentId = indentid,
                itemId = detail.itemId,
                itemName = detail.itemName,
                Quantity = detail.Quantity,
                IssuedQuantity = detail.IssuedQuantity,
                isActive = detail.isActive,
                createdById = detail.createdById,
                createdDateTime = DateTime.Now,
            };
    }

        async Task<ServiceStatusResponseModel> IindentServices.GetIndentDetails(int roleId, int empId, DateTime fromDate, DateTime todate, int UserId)
        {
            try
            {
                var query = from ind in db.Indent
                            join id in db.indentDetail on ind.indentId equals id.indentId
                                  join ld in db.roleMaster on ind.roleId equals ld.id
                                  join em in db.empMaster on ind.indentById equals em.empId
                                  join cm in db.centreMaster on ind.CentreId equals cm.centreId
                            where ind.createdDateTime>= fromDate && ind.createdDateTime<= todate
                                  select new
                                  {
                                      ind.indentId,
                                      ind.indentById,
                                      CreatedBy = string.Concat(em.fName, " ", em.lName),
                                      RoleName = ld.roleName,
                                      ind.roleId,
                                      centrename= cm.companyName,
                                      id.itemName, id.Quantity, id.ApprovedQuantity, id.IssuedQuantity,
                                      ind.createdDateTime,
                                      rejected = ind.isrejected,
                                      rejectedText = ind.isrejected==1?"rejeccted":"",
                                      status= ind.indentStatus,
                                      issueRight= (db.empMaster.Where(e=>e.empId== UserId).Select(e=> e.indentIssue).FirstOrDefault()),
                                      ApproveRight = (db.empMaster.Where(e => e.empId == UserId).Select(e => e.IndentApprove).FirstOrDefault()),
                                  };
                if(roleId > 0) 
                    {
                        query= query.Where(q => q.roleId == roleId);
                    }
                    if (empId > 0)
                    {
                        query = query.Where(q => q.indentById == empId);
                    }
                    query = query.OrderBy(q => q.indentId);

                    var data= await query.ToListAsync();

                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }

        async Task<ServiceStatusResponseModel> IindentServices.GetDetail(int indentId)
        {
            try
            {
                var data = await (from ind in db.Indent
                            join ld in db.roleMaster on ind.roleId equals ld.id
                            join idd in db.indentDetail on ind.indentId equals idd.indentId
                            join em in db.empMaster on ind.indentById equals em.empId
                            where ind.indentId == indentId
                            select new
                            {
                                ind.roleId,
                                ind.createdDateTime,
                                ind.indentById,
                                RoleName = ld.roleName,
                                CreatedBy = string.Concat(em.fName, " ", em.lName),
                                ind.indentId,
                                rejected = ind.isrejected,
                                status = ind.indentStatus,
                                idd.itemId,
                                idd.itemName,
                                idd.Quantity,
                                idd.ApprovedQuantity,
                                idd.IssuedQuantity,
                                pendingissue=(idd.ApprovedQuantity-idd.IssuedQuantity)
                            }).ToListAsync();
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }

        async Task<ServiceStatusResponseModel> IindentServices.RejectIndent(int indentId, int UserId, string rejectionReason)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var data= db.Indent.Where(i=>i.indentId == indentId).FirstOrDefault();
                    if(data == null)
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "No data found"
                        };
                    }
                    else
                    {
                        data.RejectDatetime = DateTime.Now;
                        data.rejectedBy = UserId;
                        data.isrejected = 1;
                        data.indentStatus = -1;
                        db.Indent.Update(data);
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
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = ex.Message,
                    };
                }
            }
        }

        async Task<ServiceStatusResponseModel> IindentServices.IssueIndent(List<indentIssueDetail> issueDetails)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    db.indentIssueDetail.AddRange(issueDetails);
                    await db.SaveChangesAsync();
                    foreach (indentIssueDetail detail in issueDetails)
                    {
                        var data= db.indentDetail.Where(i=>i.indentId == detail.indentId && i.itemId== detail.itemId).FirstOrDefault();
                        data.IssuedQuantity = data.IssuedQuantity + detail.IssuedQuantity;
                        db.indentDetail.Update(data);
                        await db.SaveChangesAsync();
                    }
                    await transaction.CommitAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "Issued Successful"
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = ex.Message,

                    };
                }
            }
        }

        async Task<ServiceStatusResponseModel> IindentServices.Approveindent(List<indentApprovatmodel> approvaldata)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var indentid = approvaldata.Select(i => i.indentId).FirstOrDefault();
                    var approvBy= approvaldata.Select(i => i.userid).FirstOrDefault();
                    var data = db.Indent.Where(i=> i.indentId == indentid).FirstOrDefault();
                    data.indentStatus = 1;
                    data.approveByid = approvBy;
                    data.ApproveDatetime = DateTime.Now;
                    db.Indent.Update(data);
                    await db.SaveChangesAsync();
                    foreach (var indent in approvaldata)
                    {
                        var indentitem = db.indentDetail.Where(i=> i.indentId==indentid && i.itemId== indent.itemid).FirstOrDefault();
                        if (indentitem != null)
                        {
                            indentitem.ApprovedQuantity = indent.ApprovedQuantity;
                            db.indentDetail.Update(indentitem);
                            await db.SaveChangesAsync();
                        }
                    }
                    await transaction.CommitAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "Approved Successfull"
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
    }

}

