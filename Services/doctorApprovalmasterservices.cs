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
                        var count = db.doctorApprovalMaster.Where(d=> d.empId == doctorApproval.empId  &&  d.doctorId== doctorApproval.doctorId).Count();
                        if (count > 0)
                        {
                            return new ServiceStatusResponseModel
                            {
                                Success = false,
                                Message = "Appproval rights already Available"
                            };
                        }
                        db.doctorApprovalMaster.Add(doctorApproval);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        msg = "saved Successful";
                    }
                    else
                    {
                        var data = db.doctorApprovalMaster.Where(d => d.id == doctorApproval.id).FirstOrDefault();
                        if(data!=null)
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

        async Task<ServiceStatusResponseModel> IdoctorApprovalmasterservices.DoctorApprovalData()
        {
            var result= await (from da in db.doctorApprovalMaster
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
    }
}
