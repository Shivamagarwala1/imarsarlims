using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Response_Model;
using System.Net.Mail;

namespace iMARSARLIMS.Services
{
    public class tnxInvestigationAttachmentService : ItnxInvestigationAttachmentService
    {
        private readonly ContextClass db;
        public tnxInvestigationAttachmentService(ContextClass context, ILogger<BaseController<tnx_InvestigationAttchment>> logger)
        {
            db = context;
        }

        async Task<ServiceStatusResponseModel> ItnxInvestigationAttachmentService.AddReport(tnx_InvestigationAddReport Report)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {

                    if (Report.id == 0)
                    {
                        db.tnx_InvestigationAddReport.Add(Report);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Saved Successfull"
                        };
                    }
                    else
                    {
                        db.tnx_InvestigationAddReport.Update(Report);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Updated Successfull"
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

        async Task<ServiceStatusResponseModel> ItnxInvestigationAttachmentService.AddAttchment(tnx_InvestigationAttchment attchment)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {

                    if(attchment.id==0)
                    {
                        db.tnx_InvestigationAttchment.Add(attchment);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Saved Successfull"
                        };
                    }
                    else
                    {
                        db.tnx_InvestigationAttchment.Update(attchment);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Updated Successfull"
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
