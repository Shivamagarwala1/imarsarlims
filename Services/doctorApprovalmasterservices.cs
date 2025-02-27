using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Services
{
    public class doctorApprovalmasterservices : IdoctorApprovalmasterservices
    {
        private readonly ContextClass db;
        public doctorApprovalmasterservices(ContextClass context, ILogger<BaseController<doctorApprovalMaster>> logger)
        {
            db = context;
        }

        async Task<ServiceStatusResponseModel> IdoctorApprovalmasterservices.updateDoctorApprovalStatus(int id, byte status, int userid)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var msg = "";
                    var data = db.doctorApprovalMaster.Where(d => d.id == id).FirstOrDefault();
                    if (data != null)
                    {

                        data.isActive = status;
                        data.updateById = userid;
                        data.updateDateTime = DateTime.Now;

                        db.doctorApprovalMaster.Update(data);
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
                            Message = "Wrong Id,Please use correct Id"
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

        async Task<ServiceStatusResponseModel> IdoctorApprovalmasterservices.saveupdateDoctorApproval(doctorApprovalMaster doctorApproval)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var msg = "";
                    if (doctorApproval.id == 0)
                    {
                        var count = db.doctorApprovalMaster.Where(d => d.empId == doctorApproval.empId && d.doctorId == doctorApproval.doctorId && d.isActive==1).Count();
                        if (count > 0)
                        {
                            return new ServiceStatusResponseModel
                            {
                                Success = false,
                                Message = "Appproval rights already Available"
                            };
                        }
                        var DoctorApprovalData = db.doctorApprovalMaster.Add(doctorApproval);
                        await db.SaveChangesAsync();
                        var approvalId = DoctorApprovalData.Entity.id;
                        var signdata = createSignData(approvalId, doctorApproval.empId, doctorApproval.doctorId, doctorApproval.signature,(int)doctorApproval.createdById);
                        db.DoctorApprovalSign.Add(signdata);
                        await transaction.CommitAsync();
                        msg = "saved Successful";
                    }
                    else
                    {
                        var data = db.doctorApprovalMaster.Where(d => d.id == doctorApproval.id).FirstOrDefault();
                        if (data != null)
                        {
                            data.centreId = doctorApproval.centreId;
                            data.doctorId = doctorApproval.id;
                            data.empId = doctorApproval.empId;
                            data.empName = doctorApproval.empName;
                            data.doctorName = doctorApproval.doctorName;
                            data.signature = doctorApproval.signature;
                            data.hold = doctorApproval.hold;
                            data.approve = doctorApproval.approve;
                            data.notApprove = doctorApproval.notApprove;
                            data.unHold = doctorApproval.unHold;
                            data.updateById = doctorApproval.updateById;
                            data.updateDateTime = doctorApproval.updateDateTime;

                            db.doctorApprovalMaster.Update(data);
                            await db.SaveChangesAsync();
                            var signdata = createSignData(doctorApproval.id, doctorApproval.empId, doctorApproval.doctorId, doctorApproval.signature, (int)doctorApproval.updateById);
                            db.DoctorApprovalSign.Add(signdata);
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
        private DoctorApprovalSign createSignData(int approvalid, int empid, int doctorid, string sign,int createdby)
        {
            return new DoctorApprovalSign
            {
                id=0,
                DoctorId=doctorid,
                DoctorSign=sign,
                empid=empid,
                isActive=1,
                createdById=createdby,
                createdDateTime= DateTime.Now
            };
        }
        async Task<ServiceStatusResponseModel> IdoctorApprovalmasterservices.DoctorApprovalData()
        {
            var result = await (from da in db.doctorApprovalMaster
                                join cm in db.centreMaster on da.centreId equals cm.centreId
                                select new
                                {
                                    da.id,
                                    da.centreId,
                                    da.empId,
                                    da.empName,
                                    da.doctorName,
                                    da.signature,
                                    da.approve,
                                    da.notApprove,
                                    da.hold,
                                    da.unHold,
                                    da.doctorId,
                                    da.isActive,
                                    cm.companyName
                                }).ToListAsync();

            return new ServiceStatusResponseModel
            {
                Success = true,
                Data = result
            };
        }

        async Task<ServiceStatusResponseModel> IdoctorApprovalmasterservices.Doctorcenterwise(int empid, int centreid)
        {
            try
            {
                var result = await (from da in db.doctorApprovalMaster
                                    join cm in db.centreMaster on da.centreId equals cm.centreId
                                    where da.empId == empid && da.centreId == centreid && da.isActive == 1
                                    select new
                                    {
                                        da.id,
                                        da.doctorId,
                                        da.doctorName,
                                        da.signature,
                                        da.approve,
                                        da.notApprove,
                                        da.hold,
                                        da.unHold
                                    }).ToListAsync();
                if (result != null)
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Data = result
                    };
                }
                else
                {

                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = "No Approval Right",
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
}
