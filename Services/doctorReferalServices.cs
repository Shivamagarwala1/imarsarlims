using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace iMARSARLIMS.Services
{
    public class doctorReferalServices : IdoctorReferalServices
    {
        private readonly ContextClass db;
        public doctorReferalServices(ContextClass context, ILogger<BaseController<doctorReferalMaster>> logger)
        {
            db = context;
        }

        async Task<ServiceStatusResponseModel> IdoctorReferalServices.ReferDoctorData()
        {
            try
            {
                var doctorData = await (from dr in db.doctorReferalMaster
                                        join cm in db.centreMaster on dr.centreID equals cm.centreId
                                        select new
                                        {
                                            dr.doctorId,
                                            dr.centreID,
                                            dr.title,
                                            dr.degreeName,
                                            dr.doctorName,
                                            dr.doctorCode,
                                            dr.imaRegistartionNo,
                                            dr.address1,
                                            dr.address2,
                                            dr.mobileNo,
                                            dr.isActive
                                        }).ToListAsync();
                if (doctorData != null)
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Data = doctorData
                    };
                }
                else
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = "Data not found"
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

        async Task<ServiceStatusResponseModel> IdoctorReferalServices.UpdateReferDoctorStatus(int DoctorId, byte status, int UserId)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var doctorData= db.doctorReferalMaster.Where(d=> d.doctorId== DoctorId).FirstOrDefault();
                    if (doctorData != null)
                    {
                        doctorData.isActive = status;
                        doctorData.updateById = UserId;
                        doctorData.updateDateTime= DateTime.Now;
                        db.doctorReferalMaster.Update(doctorData);
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
                            Message = "Doctor Not Found"
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

        async Task<ServiceStatusResponseModel> IdoctorReferalServices.SaveUpdateReferDoctor(doctorReferalMaster refDoc)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var msg = "";
                    if(refDoc.doctorId== 0)
                    {
                        db.doctorReferalMaster.Add(refDoc);
                        msg = "Saved Successful"; 
                    }
                    else
                    {
                        db.doctorReferalMaster.Update(refDoc);
                        msg = "Updated Successful";
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
