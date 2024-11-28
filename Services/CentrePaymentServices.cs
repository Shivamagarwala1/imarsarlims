using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Account;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Mysqlx.Crud;
using System.Reflection.Metadata;

namespace iMARSARLIMS.Services
{
    public class CentrePaymentServices : ICentrePaymentServices
    {
        private readonly ContextClass db;
        public CentrePaymentServices(ContextClass context, ILogger<BaseController<CentrePayment>> logger)
        {
            db = context;
        }
        async Task<ServiceStatusResponseModel> ICentrePaymentServices.SubmitPayment(CentrePaymentRequestModel centrePayments)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {

                try
                {
                    if (centrePayments.apprvoedByID == 0)
                    {
                        var DocumentName = "";
                        if (centrePayments.paymentRecieptFile != null)
                        {
                            var DocName = UploadPaymentReciept(centrePayments.paymentRecieptFile).ToString();
                            if (DocName.Split("#")[1] == "1")
                            {
                                DocumentName = DocName.Split("#")[1];
                            }
                            else
                            {
                                return new ServiceStatusResponseModel
                                {
                                    Success = false,
                                    Message = DocName.Split("#")[1]
                                };
                            }
                        }
                        var centerPaymentData = CreatePaymentData(centrePayments, DocumentName);
                        db.CentrePayment.Add(centerPaymentData);
                        await db.SaveChangesAsync();
                    }
                    await transaction.CommitAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "Payment saved Successful"
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

        public async Task<string> UploadPaymentReciept(IFormFile file)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (extension != ".pdf" && extension != ".jpg" && extension != ".jpeg" && extension != ".png")
            {
                return "0#No valid file extension found. Valid file extensions are (.jpg, .pdf, .png)";
                // testing git 
            }

            try
            {
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedDocuments");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                string fileName = Guid.NewGuid().ToString() + extension;
                string filePath = Path.Combine(uploadPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return "1#" + fileName;
            }
            catch (Exception ex)
            {
                return "0#Error in attachment upload: " + ex.Message;
            }
        }

        public CentrePayment CreatePaymentData(CentrePaymentRequestModel centerpaymentmodel, string fileName)
        {
            return new CentrePayment
            {
                id = centerpaymentmodel.id,
                centreId = centerpaymentmodel.centreId,
                paymentDate = centerpaymentmodel.paymentDate,
                paymentMode = centerpaymentmodel.paymentMode,
                advancePaymentAmt = centerpaymentmodel.advancePaymentAmt,
                bank = centerpaymentmodel.bank,
                tnxNo = centerpaymentmodel.tnxNo,
                tnxDate = centerpaymentmodel.tnxDate,
                remarks = centerpaymentmodel.remarks,
                createdBy = centerpaymentmodel.createdBy,
                createdDate = centerpaymentmodel.createdDate,
                paymentType = centerpaymentmodel.paymentType,
                documentName = fileName

            };

        }

        async Task<ServiceStatusResponseModel> ICentrePaymentServices.PaymentApproveReject(CentrePaymetVerificationRequestModel CentrePaymetVerificationRequest)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {

                try
                {
                    if (CentrePaymetVerificationRequest.id != 0)
                    {
                        if (CentrePaymetVerificationRequest.approved == -1 && CentrePaymetVerificationRequest.rejectRemarks == "")
                        {
                            return new ServiceStatusResponseModel
                            {
                                Success = true,
                                Message = "Please Entre Rejection remark"
                            };
                        }
                        else
                        {
                            var paymetnData = db.CentrePayment.Where(c => c.id == CentrePaymetVerificationRequest.id).FirstOrDefault();
                            UpdatePaymentStaus(paymetnData, CentrePaymetVerificationRequest);
                            db.CentrePayment.UpdateRange(paymetnData);
                            await db.SaveChangesAsync();
                            await transaction.CommitAsync();
                            return new ServiceStatusResponseModel
                            {
                                Success = true,
                                Message = "Payment saved Successful"
                            };
                        }
                    }
                    else
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Please Entre Valid Payment Id"
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

        private void UpdatePaymentStaus(CentrePayment paymentdata, CentrePaymetVerificationRequestModel verificationRequestModel)
        {
            paymentdata.updateDate = DateTime.Now;
            paymentdata.approved= verificationRequestModel.approved;
            paymentdata.apprvoedByID = verificationRequestModel.apprvoedByID;
            paymentdata.updateByID = verificationRequestModel.updateByID;
        }
    }
}
