using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Account;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using MySqlX.XDevAPI.Common;
using iMARSARLIMS.Model.Master;
using OfficeOpenXml;
using static Google.Cloud.Dialogflow.V2.Intent.Types.Message.Types.CarouselSelect.Types;
namespace iMARSARLIMS.Services
{
    public class CentrePaymentServices : ICentrePaymentServices
    {
        private readonly ContextClass db;
        private readonly MySql_Procedure_Services _MySql_Procedure_Services;
        public CentrePaymentServices(ContextClass context, ILogger<BaseController<CentrePayment>> logger, MySql_Procedure_Services mySql_Procedure_Services)
        {
            db = context;
            this._MySql_Procedure_Services = mySql_Procedure_Services;
        }
        async Task<ServiceStatusResponseModel> ICentrePaymentServices.SubmitPayment(CentrePaymentRequestModel centrePayments)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {

                try
                {
                    if (centrePayments.apprvoedByID == 0)
                    {

                        var centerPaymentData = CreatePaymentData(centrePayments);
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
                // testing git 1 
            }

            try
            {
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedPAymentReceipt");
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

        public CentrePayment CreatePaymentData(CentrePaymentRequestModel centerpaymentmodel)
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
                documentName = centerpaymentmodel.fileName,
                ChequeNo = centerpaymentmodel.ChequeNo,
                ChequeDate= centerpaymentmodel.ChequeDate

            };

        }

        async Task<ServiceStatusResponseModel> ICentrePaymentServices.PaymentApproveReject(CentrePaymetVerificationRequestModel CentrePaymetVerificationRequest)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    if (CentrePaymetVerificationRequest.id == 0)
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "Please Enter a Valid Payment Id"
                        };
                    }

                    if (CentrePaymetVerificationRequest.approved == -1 && string.IsNullOrWhiteSpace(CentrePaymetVerificationRequest.rejectRemarks))
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "Please Enter Rejection Remarks"
                        };
                    }

                    var paymentData = await db.CentrePayment.Where(c => c.id == CentrePaymetVerificationRequest.id).ToListAsync();

                    if (paymentData == null)
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "Payment record not found."
                        };
                    }

                    UpdatePaymentStaus(paymentData.First(), CentrePaymetVerificationRequest);
                    db.CentrePayment.Update(paymentData.First());
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "Payment saved successfully."
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = $"An error occurred: {ex.Message}"
                    };
                }
            }
        }

        private void UpdatePaymentStaus(CentrePayment paymentdata, CentrePaymetVerificationRequestModel verificationRequestModel)
        {
            paymentdata.updateDate = DateTime.Now;
            paymentdata.approved = verificationRequestModel.approved;
            paymentdata.apprvoedByID = verificationRequestModel.apprvoedByID;
            paymentdata.updateByID = verificationRequestModel.updateByID;
        }

async Task<ServiceStatusResponseModel> ICentrePaymentServices.LedgerStatus(string CentreId)
        {
            try
        {
                var result = _MySql_Procedure_Services.LedgerStatus(CentreId);

                return new ServiceStatusResponseModel
            {
                Success = true,
                Data = result
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
    public async Task<ServiceStatusResponseModel> ClientLedgerStatus(List<int> CentreId, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                var centres = await db.centreMaster
                    .Where(cm => CentreId.Contains(cm.centreId))
                    .ToListAsync(); 

                var bookings = await db.tnx_Booking
                    .Where(ip => CentreId.Contains(ip.centreId))
                    .ToListAsync(); 

                var payments = await db.CentrePayment
                    .Where(ip => CentreId.Contains(ip.centreId))
                    .ToListAsync(); 

                var result = centres.Select(cm => new
                {
                    cm.centreId,
                    cm.centrecode,
                    cm.centretype,
                    LastinvoiceAmount = (decimal?)null,
                    OpeningAmt = bookings
                                  .Where(ip => ip.centreId == cm.centreId && ip.bookingDate < FromDate)
                                  .Sum(ip => (decimal?)ip.netAmount) ?? 0,
                    DepositeAmt = payments
                                  .Where(ip => ip.centreId == cm.centreId && ip.paymentDate < FromDate)
                                  .Sum(ip => (decimal?)ip.advancePaymentAmt) ?? 0,
                    ClosingAmt = (decimal?)null,
                    CreditLimit = cm.creditLimt,
                    CurrentMonthBussiness = bookings
                                            .Where(ip => ip.centreId == cm.centreId &&
                                                         ip.bookingDate >= FromDate && ip.bookingDate <= ToDate)
                                            .Sum(ip => (decimal?)ip.netAmount) ?? 0,
                    CurrentmonthDeposit = payments
                                          .Where(ip => ip.centreId == cm.centreId &&
                                                       ip.paymentDate >= FromDate && ip.paymentDate <= ToDate)
                                          .Sum(ip => (decimal?)ip.advancePaymentAmt) ?? 0,
                    CurrentDate = DateTime.UtcNow,
                    YesterDayBussiness = bookings
                                         .Where(ip => ip.centreId == cm.centreId &&
                                                      ip.bookingDate.Date == DateTime.UtcNow.AddDays(-1).Date)
                                         .Sum(ip => (decimal?)ip.netAmount) ?? 0,
                    CreditDays = cm.creditPeridos,
                    Status = cm.isLock == 1 ? "Locked" : "UnLocked",
                    OpenBy = cm.unlockBy!="0"? db.empMaster.Where(e=>e.empId.ToString()== cm.unlockBy).Select(e=> string.Concat(e.fName," ",e.lName)).FirstOrDefault():"",
                    OpenDate = cm.unlockDate.HasValue ? cm.unlockDate.Value.ToString("yyyy-MMM-dd hh:mm tt") : "",
                    LockDate = cm.LockDate.HasValue ? cm.LockDate.Value.ToString("yyyy-MMM-dd hh:mm tt") : ""
                }).ToList();

                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = result
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

        async Task<ServiceStatusResponseModel> ICentrePaymentServices.GetPatientBillDetail(string Workorderid)
        {
            try
            {
                var result = await (from bp in db.tnx_Booking
                                        // join bi in db.tnx_BookingItem on bp.workOrderId equals bi.workOrderId
                                    where bp.workOrderId == Workorderid && bp.isActive == 1
                                    select new
                                    {
                                        bp.name,
                                        Age = string.Concat(bp.ageYear, " Y ", bp.ageMonth, " M ", bp.ageDay, " D"),
                                        bp.mrp,
                                        bp.discount,
                                        bp.netAmount
                                    }).ToListAsync();
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = result
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
        async Task<ServiceStatusResponseModel> ICentrePaymentServices.CancelPatientReciept(string Workorderid, int Userid)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var result = db.tnx_BookingItem.Where(b => b.workOrderId == Workorderid).ToList();
                    foreach (var item in result)
                    {
                        item.isRemoveItem = 1;
                        item.updateById = Userid;
                        item.updateDateTime = DateTime.Now;
                    }
                    db.tnx_BookingItem.UpdateRange(result);
                    await db.SaveChangesAsync();

                    var resultbi = db.tnx_Booking.Where(b => b.workOrderId == Workorderid).FirstOrDefault();
                    if (resultbi != null)
                    {
                        resultbi.isActive = 0;
                        resultbi.updateById = Userid;
                        resultbi.updateDateTime = DateTime.Now;
                        db.tnx_Booking.Update(resultbi);
                        await db.SaveChangesAsync();
                    }
                    await transaction.CommitAsync();

                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "Cancel successfull"
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

        async Task<ServiceStatusResponseModel> ICentrePaymentServices.GetPatientpaymentDetail(string Workorderid)
        {
            try
            {
                var patientdetail = await (from tb in db.tnx_Booking
                                           join cm in db.centreMaster on tb.centreId equals cm.centreId
                                           join tm in db.titleMaster on tb.title_id equals tm.id
                                           where tb.workOrderId == Workorderid
                                           select new
                                           {
                                               tm.title,
                                               tb.name,
                                               tb.gender,
                                               Age = string.Concat(tb.ageYear, " Y ", tb.ageMonth, " M ", tb.ageDay, " D"),
                                               tb.mobileNo,
                                               tb.workOrderId,
                                               Centrename = cm.companyName
                                           }).ToListAsync();

                var Paymentdetail = await (from rd in db.tnx_ReceiptDetails
                                           where rd.workOrderId == Workorderid
                                           select new
                                           {
                                               rd.id,
                                               rd.workOrderId,
                                               rd.receivedAmt,
                                               rd.paymentModeId
                                           }).ToListAsync();

                var result = new { patientdetail, Paymentdetail };
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = result
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

        async Task<ServiceStatusResponseModel> ICentrePaymentServices.ChangePatientpaymentDetail(List<ChangePaymentMode> PaymentMode)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    foreach (var changePaymentMode in PaymentMode)
                    {
                        if (changePaymentMode.id == 0)
                        {
                            var data = db.tnx_ReceiptDetails.Where(r => r.id == changePaymentMode.id).FirstOrDefault();
                            data.isCancel = changePaymentMode.isCancel;
                            data.canceledBy = changePaymentMode.canceledBy;
                            data.cancelReason = changePaymentMode.cancelReason;
                            data.cancelDate = changePaymentMode.cancelDate;
                            db.tnx_ReceiptDetails.Update(data);
                            await db.SaveChangesAsync();
                            await transaction.CommitAsync();

                        }
                        else
                        {
                            var paymodedata = CreatePaymentData(changePaymentMode);
                            db.tnx_ReceiptDetails.Add(paymodedata);
                            await db.SaveChangesAsync();
                            await transaction.CommitAsync();
                        }

                    }
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "Payment Mode Updated"
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
        private tnx_ReceiptDetails CreatePaymentData(ChangePaymentMode changePaymentMode)
        {
            return new tnx_ReceiptDetails
            {
                id = changePaymentMode.id,
                transactionId = changePaymentMode.transactionId,
                transactionType = changePaymentMode.transactionType,
                workOrderId = changePaymentMode.workOrderId,
                receiptNo = changePaymentMode.receiptNo,
                receivedAmt = changePaymentMode.receivedAmt,
                cashAmt = changePaymentMode.cashAmt,
                creditCardAmt = changePaymentMode.creditCardAmt,
                creditCardNo = changePaymentMode.creditCardNo,
                chequeAmt = changePaymentMode.chequeAmt,
                chequeNo = changePaymentMode.chequeNo,
                onlinewalletAmt = changePaymentMode.onlinewalletAmt,
                walletno = changePaymentMode.walletno,
                NEFTamt = changePaymentMode.NEFTamt,
                BankName = changePaymentMode.BankName,
                paymentModeId = changePaymentMode.paymentModeId,
                isCancel = changePaymentMode.isCancel,
                cancelDate = changePaymentMode.cancelDate,
                canceledBy = changePaymentMode.canceledBy,
                cancelReason = changePaymentMode.cancelReason,
                bookingCentreId = changePaymentMode.bookingCentreId,
                settlementCentreID = changePaymentMode.settlementCentreID,
                receivedBy = changePaymentMode.receivedBy,
                receivedID = changePaymentMode.receivedID,
            };
        }

        async Task<ServiceStatusResponseModel> ICentrePaymentServices.GetRateList(int ratetypeID)
        {
            try
            {
                var result = await (from rtm in db.rateTypeMaster
                                    join rttm in db.rateTypeTagging on rtm.rateTypeId equals rttm.rateTypeId
                                    join rtrl in db.rateTypeWiseRateList on rtm.rateTypeId equals rtrl.rateTypeId
                                    join im in db.itemMaster on rtrl.itemid equals im.itemId
                                    where rtm.isActive == 1 && rttm.rateTypeId == ratetypeID
                                    orderby im.deptId, rtrl.itemid
                                    select new
                                    {
                                        ItemName = im.itemName,
                                        MRP = rtrl.mrp,
                                        Rate = rtrl.rate
                                    }).ToListAsync();
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = result
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

        byte[] ICentrePaymentServices.GetRateListPdf(int ratetypeID)
        {
            var result = (from rtm in db.rateTypeMaster
                          join rtrl in db.rateTypeWiseRateList on rtm.rateTypeId equals rtrl.rateTypeId
                          join im in db.itemMaster on rtrl.itemid equals im.itemId
                          where rtm.isActive == 1 && rtrl.rateTypeId == ratetypeID
                          orderby im.deptId, rtrl.itemid
                          select new
                          {
                              ItemName = im.itemName,
                              MRP = rtrl.mrp,
                              Rate = rtrl.rate,
                             rtm.rateName
                            
                          }).ToList();

            if (result.Count > 0)
            {
                QuestPDF.Settings.License = LicenseType.Community;

                var document = Document.Create(container =>
                {

                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);

                        page.MarginTop(2.5f, Unit.Centimetre);
                        page.MarginLeft(0.5f, Unit.Centimetre);
                        page.MarginRight(0.5f, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.Foreground();
                        page.DefaultTextStyle(x => x.FontFamily("TimesNewRoman"));
                        page.DefaultTextStyle(x => x.FontSize(10));
                        page.Header()
                        .Column(column =>
                        {
                            column.Item().Text(result[0].rateName).AlignCenter().Bold();

                        });

                        page.Content()
                        .Column(column =>
                        {
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(1.0f, Unit.Centimetre);
                                    columns.RelativeColumn();
                                    columns.ConstantColumn(2.0f, Unit.Centimetre);
                                    columns.ConstantColumn(2.0f, Unit.Centimetre);
                                });
                               table.Header(header =>
                                {
                                    string[] headers = { "#", "Test Name", "Mrp", "Rate" };
                                    foreach (var title in headers)
                                    {
                                        header.Cell().BorderBottom(0.5f, Unit.Point).BorderTop(0.5f, Unit.Point)
                                            .Text(title)
                                            .Style(TextStyle.Default.FontSize(10).Bold());
                                    }
                                });
                                int rowNumber = 1;
                                foreach (var item in result)
                                {
                                    table.Cell().Height(0.5f, Unit.Centimetre).Text(""+ rowNumber).Style(TextStyle.Default.FontSize(10));
                                    table.Cell().Height(0.5f, Unit.Centimetre).Text(item.ItemName).Style(TextStyle.Default.FontSize(10));
                                    table.Cell().Height(0.5f, Unit.Centimetre).Text(item.MRP.ToString()).Style(TextStyle.Default.FontSize(10));
                                    table.Cell().Height(0.5f, Unit.Centimetre).Text(item.Rate.ToString()).Style(TextStyle.Default.FontSize(10));
                                    rowNumber++;
                                }
                            });
                        });

                        page.Footer().Height(1.5f, Unit.Centimetre)
                           .Column(column =>
                           {
                               column.Item().Table(table =>
                               {
                                   table.ColumnsDefinition(columns =>
                                   {
                                       columns.RelativeColumn();
                                       columns.RelativeColumn();
                                       columns.RelativeColumn();
                                   });
                                   table.Cell().AlignLeft().Text("");
                                   table.Cell().AlignLeft().Text("");
                                   table.Cell().AlignRight().AlignBottom().Text(text =>
                                   {
                                       text.DefaultTextStyle(x => x.FontSize(8));
                                       text.CurrentPageNumber();
                                       text.Span(" of ");
                                       text.TotalPages();
                                   });
                               });
                           });
                    });

                });
                byte[] pdfBytes = document.GeneratePdf();
                return pdfBytes;
            }
            else
            {
                byte[] pdfbyte = [];
                return pdfbyte;
            }
        }

        async Task<ServiceStatusResponseModel> ICentrePaymentServices.TransferRateToRate(int FromRatetypeid, int ToRatetypeid , string type, double Percentage)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var itemids= db.rateTypeWiseRateList.Where(r => r.rateTypeId == FromRatetypeid).Select(r=>r.itemid).ToList();
                    var data = db.rateTypeWiseRateList.Where(r => r.rateTypeId == ToRatetypeid && itemids.Contains(r.itemid)).ToList();
                    db.rateTypeWiseRateList.RemoveRange(data);
                    await db.SaveChangesAsync();
                    var torateList = db.rateTypeWiseRateList.Where(r => r.rateTypeId == FromRatetypeid).ToList();
                    torateList.ForEach(rate => rate.id = 0);
                    torateList.ForEach(rate => rate.rateTypeId = ToRatetypeid);
                   
                    if (type=="Plus")
                    {
                        torateList.ForEach(rate => rate.rate = Math.Round(rate.rate+ rate.rate*Percentage/100));
                    }
                    else
                    {
                        torateList.ForEach(rate => rate.rate = Math.Round(rate.rate + rate.rate * Percentage / 100));
                    }

                    
                    db.rateTypeWiseRateList.AddRange(torateList);
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "Transfer successful"
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

        async Task<ServiceStatusResponseModel> ICentrePaymentServices.ClientDepositReport(List<int> Centreids, DateTime FromDate, DateTime ToDate, string Paymenttype,int status)
        {
            try
            {
                var Query = from cm in db.centreMaster
                              join cp in db.CentrePayment on cm.centreId equals cp.centreId
                              where Centreids.Contains(cm.centreId) && cp.createdDate >= FromDate && cp.createdDate <= ToDate
                              orderby cm.centreId
                              select new
                              {
                                  cp.id,
                                  CentreName = cm.companyName,
                                  cp.paymentDate,
                                  cp.advancePaymentAmt,
                                  cp.paymentType,
                                  cp.approved,
                                  remarks= string.IsNullOrEmpty(cp.remarks)?"": cp.remarks,
                                  cp.paymentMode,
                                  documentName = string.IsNullOrEmpty(cp.documentName) ? "" : cp.documentName,
                                  bank = string.IsNullOrEmpty(cp.bank) ? "" : cp.bank,
                                  ChequeNo = string.IsNullOrEmpty(cp.ChequeNo) ? "" : cp.ChequeNo,
                                  status = cp.approved == 1 ? "Payment Rcv." : cp.approved == -1 ? "Payment Reject" : "Payment Pending",
                                  ConfirmedBy = cp.apprvoedByID != null
                                      ? db.empMaster.Where(e => e.empId == cp.apprvoedByID)
                                          .Select(e => string.Concat(e.fName ?? "", " ", e.lName ?? "")).FirstOrDefault() : "",
                                  rejectRemarks= string.IsNullOrEmpty(cp.rejectRemarks)?"": cp.rejectRemarks
                              };
                if(Paymenttype!="0")
                {
                    Query= Query.Where(q=>q.paymentType.ToString()==Paymenttype);
                }
                if (status != 2)
                {
                    Query = Query.Where(q => q.approved == status);
                }

                var result = await Query.ToListAsync();

                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = result
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

        async Task<ServiceStatusResponseModel> ICentrePaymentServices.ClientDeposit(int Centreid,  string Paymenttype, int status)
        {
            try
            {
                var Query = from cm in db.centreMaster
                            join cp in db.CentrePayment on cm.centreId equals cp.centreId
                            where cm.centreId== Centreid
                            orderby cm.centreId
                            select new
                            {
                                cp.id,
                                CentreName = cm.companyName,
                                cp.paymentDate,
                                cp.advancePaymentAmt,
                                cp.paymentType,
                                cp.approved,
                                remarks = string.IsNullOrEmpty(cp.remarks) ? "" : cp.remarks,
                                cp.paymentMode,
                                documentName = string.IsNullOrEmpty(cp.documentName) ? "" : cp.documentName,
                                bank = string.IsNullOrEmpty(cp.bank) ? "" : cp.bank,
                                ChequeNo = string.IsNullOrEmpty(cp.ChequeNo) ? "" : cp.ChequeNo,
                                status = cp.approved == 1 ? "Payment Rcv." : cp.approved == -1 ? "Payment Reject" : "Payment Pending",
                                ConfirmedBy = cp.apprvoedByID != null
                                    ? db.empMaster.Where(e => e.empId == cp.apprvoedByID)
                                        .Select(e => string.Concat(e.fName ?? "", " ", e.lName ?? "")).FirstOrDefault() : "",
                                rejectRemarks = string.IsNullOrEmpty(cp.rejectRemarks) ? "" : cp.rejectRemarks
                            };
                if (Paymenttype != "0")
                {
                    Query = Query.Where(q => q.paymentType.ToString() == Paymenttype);
                }
                if (status != 2)
                {
                    Query = Query.Where(q => q.approved == status);
                }


                var result = await Query.ToListAsync();

                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = result
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

        async Task<ServiceStatusResponseModel> ICentrePaymentServices.GetWorkOrderdetailCentreChange(string WorkOrderid)
        {
            try
            {
                var result = await (from pb in db.tnx_Booking
                                    join pbi in db.tnx_BookingItem on pb.workOrderId equals pbi.workOrderId
                                    join cm in db.centreMaster on pb.centreId equals cm.centreId
                                    join rm in db.rateTypeMaster on pb.rateId equals rm.id
                                    join tm in db.titleMaster on pb.title_id equals tm.id
                                    where pb.workOrderId == WorkOrderid
                                    select new
                                    {
                                        tm.title,
                                        pb.name,
                                        pb.mobileNo,
                                        Centre = cm.companyName,
                                        Ratetype = rm.rateType,
                                        pbi.investigationName,
                                        pbi.itemId,
                                        pbi.rate
                                    }).ToListAsync();

                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = result
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

        async Task<ServiceStatusResponseModel> ICentrePaymentServices.GetWorkOrderNewRate(string WorkOrderid, int rateTypeId)
        {
            try
            {
                var result = await (from pbi in db.tnx_BookingItem
                                    join im in db.itemMaster on pbi.itemId equals im.itemId
                                    join rt in db.rateTypeWiseRateList on im.itemId equals rt.itemid
                                    where pbi.workOrderId == WorkOrderid && rt.rateTypeId == rateTypeId
                                    select new
                                    {
                                        im.itemId,
                                        im.itemName,
                                        rt.rate
                                    }).ToListAsync();

                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = result
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

        //public Task<ServiceStatusResponseModel> ChangeBillingCentre(string WorkOrderId, int Centre, int RateType)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<ServiceStatusResponseModel> GetPatientForSettelmet(int CentreId, DateTime FromDate, DateTime ToDate)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<ServiceStatusResponseModel> UpdatePatientSettelment(List<BulkSettelmentRequest> SettelmentData)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<ServiceStatusResponseModel> CentreRateChange(int Centre, DateTime FromDate, DateTime ToDate)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<ServiceStatusResponseModel> LedgerStatement(int CentreId, DateTime FromDate, DateTime ToDate, string type)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<ServiceStatusResponseModel> paymentRecieptUpload(IFormFile paymentReciept)
        {
            string extension = Path.GetExtension(paymentReciept.FileName).ToLower();
            if (extension != ".pdf" && extension != ".jpg" && extension != ".jpeg" && extension != ".png")
            {

                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = "No valid file extension found. Valid file extensions are (.jpg, .pdf, .png)"
                };
                // testing git 
                // testing git 1 
            }

            try
            {
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedPAymentReceipt");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                string fileName = Guid.NewGuid().ToString() + extension;
                string filePath = Path.Combine(uploadPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await paymentReciept.CopyToAsync(stream);
                }

                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Message = filePath
                };
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success= true,
                    Message= ex.Message
                };
            }

        }

        public byte[] ClientDepositReportExcel(List<int> centreIds, DateTime FromDate, DateTime ToDate, string Paymenttype, int status)
        {
                var Query = from cm in db.centreMaster
                            join cp in db.CentrePayment on cm.centreId equals cp.centreId
                            where centreIds.Contains(cm.centreId) && cp.createdDate >= FromDate && cp.createdDate <= ToDate
                            orderby cm.centreId
                            select new
                            {
                                cp.id,
                                CentreName = cm.companyName,
                                cp.paymentDate,
                                cp.advancePaymentAmt,
                                paymentType = cp.paymentType == 1 ? "Deposit" : cp.paymentType == 1 ? "Credit Note" : "Debit Note",
                                remarks = string.IsNullOrEmpty(cp.remarks) ? "" : cp.remarks,
                                cp.paymentMode,
                                 bank = string.IsNullOrEmpty(cp.bank) ? "" : cp.bank,
                                ChequeNo = string.IsNullOrEmpty(cp.ChequeNo) ? "" : cp.ChequeNo,
                                status = cp.approved == 1 ? "Payment Rcv." : cp.approved == -1 ? "Payment Reject" : "Payment Pending",
                                ConfirmedBy = cp.apprvoedByID != null
                                    ? db.empMaster.Where(e => e.empId == cp.apprvoedByID)
                                        .Select(e => string.Concat(e.fName ?? "", " ", e.lName ?? "")).FirstOrDefault() : "",
                                rejectRemarks = string.IsNullOrEmpty(cp.rejectRemarks) ? "" : cp.rejectRemarks
                            };
                if (Paymenttype != "0")
                {
                    Query = Query.Where(q => q.paymentType.ToString() == Paymenttype);
                }
            if (status != 2)
            {
                Query = Query.Where(q => q.status == (status == 1 ? "Payment Rcv." : status == -1 ? "Payment Reject" : "Payment Pending"));
            }

            var result = Query.ToList();
                var excelByte = MyFunction.ExportToExcel(result, "CollectionReport");
                return excelByte;
            
        }

        public byte[] ClientDepositReportPdf(List<int> centreIds, DateTime FromDate, DateTime ToDate, string Paymenttype, int status)
        {
            var Query = from cm in db.centreMaster
                            join cp in db.CentrePayment on cm.centreId equals cp.centreId
                            where centreIds.Contains(cm.centreId) && cp.createdDate >= FromDate && cp.createdDate <= ToDate
                            orderby cm.centreId
                            select new
                            {
                                cp.id,
                                CentreName = cm.companyName,
                                paymentDate= cp.paymentDate.ToString("yyyy-MMM-dd"),
                                cp.advancePaymentAmt,
                                paymentType= cp.paymentType==1? "Deposit" : cp.paymentType == 1 ?"Credit Note":"Debit Note",
                                cp.approved,
                                remarks = string.IsNullOrEmpty(cp.remarks) ? "" : cp.remarks,
                                cp.paymentMode,
                                documentName = string.IsNullOrEmpty(cp.documentName) ? "" : cp.documentName,
                                bank = string.IsNullOrEmpty(cp.bank) ? "" : cp.bank,
                                ChequeNo = string.IsNullOrEmpty(cp.ChequeNo) ? "" : cp.ChequeNo,
                                status = cp.approved == 1 ? "Payment Rcv." : cp.approved == -1 ? "Payment Reject" : "Payment Pending",
                                ConfirmedBy = cp.apprvoedByID != null
                                    ? db.empMaster.Where(e => e.empId == cp.apprvoedByID)
                                        .Select(e => string.Concat(e.fName ?? "", " ", e.lName ?? "")).FirstOrDefault() : "",
                                rejectRemarks = string.IsNullOrEmpty(cp.rejectRemarks) ? "" : cp.rejectRemarks
                            };
                if (Paymenttype != "0")
                {
                    Query = Query.Where(q => q.paymentType.ToString() == Paymenttype);
                }
                if (status != 2)
                {
                    Query = Query.Where(q => q.approved == status);
                }

                var result = Query.ToList();
            if(result.Count > 0) {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {

                container.Page(page =>
                {
                    page.Size(PageSizes.A4);

                    page.MarginTop(2.5f, Unit.Centimetre);
                    page.MarginLeft(0.5f, Unit.Centimetre);
                    page.MarginRight(0.5f, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.Foreground();
                    page.DefaultTextStyle(x => x.FontFamily("TimesNewRoman"));
                    page.DefaultTextStyle(x => x.FontSize(10));
                    page.Header()
                    .Column(column =>
                    {
                        column.Item().Text("Client Depasit Report").AlignCenter().Bold();

                    });

                    page.Content()
                    .Column(column =>
                    {
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(1.0f, Unit.Centimetre);
                                columns.RelativeColumn();
                                columns.ConstantColumn(3.0f, Unit.Centimetre);
                                columns.ConstantColumn(2.0f, Unit.Centimetre);
                                columns.ConstantColumn(2.0f, Unit.Centimetre);
                                columns.ConstantColumn(3.0f, Unit.Centimetre);
                                columns.ConstantColumn(3.0f, Unit.Centimetre);
                            });
                            table.Header(header =>
                            {
                                string[] headers = { "#", "ClientName", "PaymentDate", "AdvanceAmount", "Pay Type", "Status", "Remark" };
                                foreach (var title in headers)
                                {
                                    header.Cell().BorderBottom(0.5f, Unit.Point).BorderTop(0.5f, Unit.Point)
                                        .Text(title)
                                        .Style(TextStyle.Default.FontSize(10).Bold());
                                }
                            });
                            int rowNumber = 1;
                            foreach (var item in result)
                            {
                                table.Cell().Text("" + rowNumber).Style(TextStyle.Default.FontSize(10));
                                table.Cell().Text(item.CentreName).Style(TextStyle.Default.FontSize(10));
                                table.Cell().Text(item.paymentDate).Style(TextStyle.Default.FontSize(10));
                                table.Cell().Text(item.advancePaymentAmt.ToString()).Style(TextStyle.Default.FontSize(10));
                                table.Cell().Text(item.paymentType).Style(TextStyle.Default.FontSize(10));
                                table.Cell().Text(item.status.ToString()).Style(TextStyle.Default.FontSize(10));
                                table.Cell().Text(item.rejectRemarks.ToString()).Style(TextStyle.Default.FontSize(10));
                                rowNumber++;
                            }
                        });
                    });

                    page.Footer().Height(1.5f, Unit.Centimetre)
                       .Column(column =>
                       {
                           column.Item().Table(table =>
                           {
                               table.ColumnsDefinition(columns =>
                               {
                                   columns.RelativeColumn();
                                   columns.RelativeColumn();
                                   columns.RelativeColumn();
                               });
                               table.Cell().AlignLeft().Text("");
                               table.Cell().AlignLeft().Text("");
                               table.Cell().AlignRight().AlignBottom().Text(text =>
                               {
                                   text.DefaultTextStyle(x => x.FontSize(8));
                                   text.CurrentPageNumber();
                                   text.Span(" of ");
                                   text.TotalPages();
                               });
                           });
                       });
                });

            });
            byte[] pdfBytes = document.GeneratePdf();
            return pdfBytes;
        }
            else
            {
                byte[] pdfbyte = [];
                return pdfbyte;
            }

}
    }
}
