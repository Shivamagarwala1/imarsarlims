using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Services
{
    public class labUniversalMasterServices : IlabUniversalMasterServices
    {
        private readonly ContextClass db;
        public labUniversalMasterServices(ContextClass context, ILogger<BaseController<outSourcelabmaster>> logger)
        {

            db = context;

        }

        async Task<ServiceStatusResponseModel> IlabUniversalMasterServices.SaveUpdateSampleRerunReason(SampleRerunReason SampleReason)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    if (SampleReason.id == 0)
                    {
                        db.SampleRerunReason.Add(SampleReason);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Saved Successful"
                        };
                    }
                    else
                    {
                        db.SampleRerunReason.Update(SampleReason);
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
                    await transaction.RollbackAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = ex.Message
                    };
                }
            }
        }

        async Task<ServiceStatusResponseModel> IlabUniversalMasterServices.SaveUpdateTestMethod(TestMethodMaster testMethod)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    if (testMethod.id == 0)
                    {
                        db.TestMethodMaster.Add(testMethod);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Saved Successful"
                        };
                    }
                    else
                    {
                        db.TestMethodMaster.Update(testMethod);
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
                    await transaction.RollbackAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = ex.Message
                    };
                }
            }
        }

        async Task<ServiceStatusResponseModel> IlabUniversalMasterServices.UpdateSampleReasonStatus(int id, byte status, int userId)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var data = db.SampleRerunReason.Where(o => o.id == id).FirstOrDefault();
                    if (data != null)
                    {
                        data.isActive = status;
                        data.updateById = userId;
                        data.updateDateTime = DateTime.Now;
                        db.SampleRerunReason.Update(data);
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
                            Message = "No Data Found To Update"
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

        async Task<ServiceStatusResponseModel> IlabUniversalMasterServices.UpdateTestMethodStatus(int id, byte status, int userId)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var data = db.TestMethodMaster.Where(o => o.id == id).FirstOrDefault();
                    if (data != null)
                    {
                        data.isActive = status;
                        data.updateById = userId;
                        data.updateDateTime = DateTime.Now;
                        db.TestMethodMaster.Update(data);
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
                            Message = "No Data Found To Update"
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

        async Task<ServiceStatusResponseModel> IlabUniversalMasterServices.SaveUpdateOutsourceLab(outSourcelabmaster OutsourceLab)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    if (OutsourceLab.id == 0)
                    {
                        db.outSourcelabmaster.Add(OutsourceLab);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Saved Successful"
                        };
                    }
                    else
                    {
                        db.outSourcelabmaster.Update(OutsourceLab);
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
                    await transaction.RollbackAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = ex.Message
                    };
                }
            }
        }

        async Task<ServiceStatusResponseModel> IlabUniversalMasterServices.SaveUpdateSampletype(sampletype_master SampleType)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    if (SampleType.id == 0)
                    {
                        db.sampletype_master.Add(SampleType);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Saved Successful"
                        };
                    }
                    else
                    {
                        db.sampletype_master.Update(SampleType);
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

        async Task<ServiceStatusResponseModel> IlabUniversalMasterServices.UpdateOutsourceLabStatus(int id, byte status, int userId)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var data = db.outSourcelabmaster.Where(o => o.id == id).FirstOrDefault();
                    if (data != null)
                    {
                        data.isActive = status;
                        data.updateById = userId;
                        data.updateDateTime = DateTime.Now;
                        db.outSourcelabmaster.Update(data);
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
                            Message = "No Data Found To Update"
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

        async Task<ServiceStatusResponseModel> IlabUniversalMasterServices.UpdateSampleTypeStatus(int id, byte status, int userId)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var data = db.sampletype_master.Where(o => o.id == id).FirstOrDefault();
                    if (data != null)
                    {
                        data.isActive = status;
                        data.updateById = userId;
                        data.updateDateTime = DateTime.Now;
                        db.sampletype_master.Update(data);
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
                            Message = "No Data Found To Update"
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

        async Task<ServiceStatusResponseModel> IlabUniversalMasterServices.UpdateFooterTextStatus(int id, byte status, int userId)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var data = db.labReportFooterText.Where(o => o.id == id).FirstOrDefault();
                    if (data != null)
                    {
                        data.isActive = status;
                        data.updateById = userId;
                        data.updateDateTime = DateTime.Now;
                        db.labReportFooterText.Update(data);
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
                            Message = "No Data Found To Update"
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

        async Task<ServiceStatusResponseModel> IlabUniversalMasterServices.SaveUpdateFooterText(labReportFooterText FooterText)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    if (FooterText.id == 0)
                    {
                        var data = db.labReportFooterText.Where(l=>l.centreId== FooterText.centreId).FirstOrDefault();
                        if (data != null)
                        {
                            return new ServiceStatusResponseModel
                            {
                                Success = false,
                                Message = "Footer Text Already Exist"
                            };
                        }
                        else
                        {
                            db.labReportFooterText.Add(FooterText);
                            await db.SaveChangesAsync();
                            await transaction.CommitAsync();
                            return new ServiceStatusResponseModel
                            {
                                Success = true,
                                Message = "Saved Successful"
                            };
                        }
                    }
                    else
                    {
                        db.labReportFooterText.Update(FooterText);
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

        async Task<ServiceStatusResponseModel> IlabUniversalMasterServices.GetFooterText()
        {
            try
            {
                var data = await (from ft in db.labReportFooterText
                                  join cm in db.centreMaster on ft.centreId equals cm.centreId
                                  select new
                                  {
                                      ft.id,
                                      ft.centreId,
                                      CentreName=cm.companyName,
                                      ft.footerText,
                                      ft.isActive
                                  }).ToListAsync();
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data=data
                };
            }
            catch(Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        async Task<ServiceStatusResponseModel> IlabUniversalMasterServices.UpdateSampleRemarkStatus(int id, byte status, int userId)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var data = db.SampleremarkMaster.Where(o => o.id == id).FirstOrDefault();
                    if (data != null)
                    {
                        data.isActive = status;
                        data.updateById = userId;
                        data.updateDateTime = DateTime.Now;
                        db.SampleremarkMaster.Update(data);
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
                            Message = "No Data Found To Update"
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

        async Task<ServiceStatusResponseModel> IlabUniversalMasterServices.SaveUpdatesampleRemark(SampleremarkMaster SampleRemark)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    if (SampleRemark.id == 0)
                    {
                        db.SampleremarkMaster.Add(SampleRemark);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Saved Successful"
                        };
                    }
                    else
                    {
                        db.SampleremarkMaster.Update(SampleRemark);
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

