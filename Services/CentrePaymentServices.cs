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
                documentName = centerpaymentmodel.fileName

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
            paymentdata.approved = verificationRequestModel.approved;
            paymentdata.apprvoedByID = verificationRequestModel.apprvoedByID;
            paymentdata.updateByID = verificationRequestModel.updateByID;
        }

        async Task<ServiceStatusResponseModel> ICentrePaymentServices.LedgerStatus(List<int> CentreId)
        {
            try
            {
                var result = (from cm in db.centreMaster
                              join tbi in db.tnx_BookingItem on cm.centreId equals tbi.centreId
                              join tb in db.tnx_Booking on tbi.transactionId equals tb.transactionId
                              where tbi.isRemoveItem == 0 &&
                                    (tbi.isPackage == 1 || tbi.isSampleCollected == "Y") &&
                                    CentreId.Contains(cm.centreId)
                              group new { cm, tbi } by cm.parentCentreID into g
                              select new
                              {
                                  IsLock = g.FirstOrDefault().cm.isLock,
                                  CcreditLimt = g.FirstOrDefault().cm.creditLimt,
                                  CentreType = g.FirstOrDefault().cm.centretype,
                                  LCentreId = g.FirstOrDefault().cm.centreId,
                                  centercode = g.FirstOrDefault().cm.centrecode,
                                  CompanyName = g.FirstOrDefault().cm.companyName,
                                  Centreadd = g.FirstOrDefault().cm.address,
                                  Centremobile = g.FirstOrDefault().cm.mobileNo,
                                  CreditPeridos = g.FirstOrDefault().cm.creditPeridos,
                                  Cactive = g.FirstOrDefault().cm.isActive,
                                  UnlockBy = g.FirstOrDefault().cm.unlockBy ?? "",
                                  LockDate = g.FirstOrDefault().cm.LockDate.HasValue ? g.FirstOrDefault().cm.LockDate.Value.ToString("dd-MMM-yyyy HH:mm") : "",
                                  UnlockDate = g.FirstOrDefault().cm.unlockDate.HasValue ? g.FirstOrDefault().cm.unlockDate.Value.ToString("dd-MMM-yyyy HH:mm") : "",
                                  InvoiceNo = (from ip in db.centreInvoice
                                               where ip.centreid == g.FirstOrDefault().cm.centreId
                                               orderby ip.id descending
                                               select ip.invoiceNo).FirstOrDefault() ?? "",
                                  Remarks = (from em in db.centreLedgerRemarks
                                             where em.centreId == g.FirstOrDefault().cm.centreId
                                             orderby em.id descending
                                             select em.remarks).Take(3).ToList(),
                                  CreatedDate = (from ip in db.CentrePayment
                                                 where ip.centreId == g.FirstOrDefault().cm.centreId
                                                 orderby ip.id descending
                                                 select ip.createdDate)
                                                 .FirstOrDefault().ToString("dd-MMM-yyyy") ?? "",
                                  InvoiceAmt = (from ip in db.CentrePayment
                                                where ip.centreId == g.FirstOrDefault().cm.centreId
                                                orderby ip.id descending
                                                select ip.advancePaymentAmt).FirstOrDefault() ?? 0,
                                  CreationPayment = 0,
                                  ApprovedPayment = (from tpp in db.CentrePayment
                                                     where tpp.centreId == g.FirstOrDefault().cm.centreId && tpp.approved == 1
                                                     select tpp.advancePaymentAmt).Sum() ?? 0,
                                  CurrentMPayment = (from tpp in db.CentrePayment
                                                     where tpp.centreId == g.FirstOrDefault().cm.centreId && tpp.paymentType == 1 &&
                                                           tpp.paymentDate.Month == DateTime.Now.Month &&
                                                           tpp.paymentDate.Year == DateTime.Now.Year &&
                                                           tpp.approved == 1
                                                     select tpp.advancePaymentAmt).Sum() ?? 0,
                                  UnPaid = g.Sum(x => x.tbi.mrp),
                                  RemainingPayment = (from tpp in db.CentrePayment
                                                      where tpp.centreId == g.FirstOrDefault().cm.centreId && tpp.approved == 1
                                                      select tpp.advancePaymentAmt).Sum() ?? 0 - g.Sum(x => x.tbi.mrp),
                                  CurrentBuss = (from tbbi in db.tnx_BookingItem
                                                 join tbb in db.tnx_Booking on tbbi.transactionId equals tbb.transactionId
                                                 where tbb.createdDateTime.Month == DateTime.Now.Month &&
                                                       tbb.createdDateTime.Year == DateTime.Now.Year &&
                                                       tbbi.isPackage == 1 && tbbi.isSampleCollected == "Y" &&
                                                       tbb.centreId == g.FirstOrDefault().cm.centreId
                                                 select tbbi.rate).Sum(),
                                  AvailableBalance = (from ip in db.CentrePayment
                                                      where ip.centreId == g.FirstOrDefault().cm.centreId && ip.approved == 1
                                                      select ip.advancePaymentAmt).Sum() ?? 0 -
                                                     (from tbi in db.tnx_BookingItem
                                                      where tbi.centreId == g.FirstOrDefault().cm.centreId
                                                      select tbi.netAmount).Sum()
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

        async Task<ServiceStatusResponseModel> ICentrePaymentServices.ClientLedgerStatus(List<int> CentreId, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                var result = await (from cm in db.centreMaster
                                    join ci in db.centreInvoice on cm.centreId equals ci.centreid
                                    join cp in db.CentrePayment on cm.centreId equals cp.centreId
                                    select new
                                    {
                                        cm.centreId,
                                        cm.centrecode,
                                        cm.centretype,
                                        LastinvoiceAmount = "",
                                        OpeningAmt = "",
                                        DepositeAmt = "",
                                        ClosingAmt = "",
                                        CreditLimit = cm.creditLimt,
                                        CurrentMonthBussiness = "",
                                        CurrentmonthDeposit = "",
                                        CurrentDate = "",
                                        YesterDayBussiness = "",
                                        CreditDays = cm.creditPeridos,
                                        Status = "",
                                        OpenBy = "",
                                        OpenDate = "",
                                        LockDate = ""
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
                          join rttm in db.rateTypeTagging on rtm.rateTypeId equals rttm.rateTypeId
                          join rtrl in db.rateTypeWiseRateList on rtm.rateTypeId equals rtrl.rateTypeId
                          join im in db.itemMaster on rtrl.itemid equals im.itemId
                          join cm in db.centreMaster on rttm.centreId equals cm.centreId
                          where rtm.isActive == 1 && rttm.rateTypeId == ratetypeID
                          orderby im.deptId, rtrl.itemid
                          select new
                          {
                              ItemName = im.itemName,
                              MRP = rtrl.mrp,
                              Rate = rtrl.rate,
                              cm.companyName,
                              cm.address
                          }).ToList();

            if (result.Count > 0)
            {
                QuestPDF.Settings.License = LicenseType.Community;

                var document = Document.Create(container =>
                {

                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);

                        page.MarginTop(3.5f, Unit.Centimetre);
                        page.MarginLeft(0.5f, Unit.Centimetre);
                        page.MarginRight(0.5f, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.Foreground();
                        page.DefaultTextStyle(x => x.FontFamily("TimesNewRoman"));
                        page.DefaultTextStyle(x => x.FontSize(10));
                        page.Header()
                        .Column(column =>
                        {
                            column.Item().Text(result[0].companyName).AlignCenter().Bold();
                            column.Item().Text(result[0].address).AlignCenter().Bold();

                        });

                        page.Content()
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
                                table.Cell().Height(0.5f, Unit.Centimetre).BorderTop(0.4f, Unit.Point).BorderBottom(0.4f, Unit.Point).Text(" Test Name").Style(TextStyle.Default.FontSize(10).Bold());
                                table.Cell().Height(0.5f, Unit.Centimetre).BorderTop(0.4f, Unit.Point).BorderBottom(0.4f, Unit.Point).Text(" MRP").Style(TextStyle.Default.FontSize(10).Bold());
                                table.Cell().Height(0.5f, Unit.Centimetre).BorderTop(0.4f, Unit.Point).BorderBottom(0.4f, Unit.Point).Text(" Rate").Style(TextStyle.Default.FontSize(10).Bold());

                                foreach (var item in result)
                                {
                                    table.Cell().Height(0.5f, Unit.Centimetre).BorderTop(0.4f, Unit.Point).BorderBottom(0.4f, Unit.Point).Text(item.ItemName).Style(TextStyle.Default.FontSize(10));
                                    table.Cell().Height(0.5f, Unit.Centimetre).BorderTop(0.4f, Unit.Point).BorderBottom(0.4f, Unit.Point).Text(item.MRP.ToString()).Style(TextStyle.Default.FontSize(10));
                                    table.Cell().Height(0.5f, Unit.Centimetre).BorderTop(0.4f, Unit.Point).BorderBottom(0.4f, Unit.Point).Text(item.Rate.ToString()).Style(TextStyle.Default.FontSize(10));
                                }
                            });
                        });

                        page.Footer().Height(2.5f, Unit.Centimetre)
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

        async Task<ServiceStatusResponseModel> ICentrePaymentServices.TransferRateToRate(int FromRatetypeid, int ToRatetypeid)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var data = db.rateTypeWiseRateList.Where(r => r.rateTypeId == ToRatetypeid).ToList();
                    db.rateTypeWiseRateList.RemoveRange(data);
                    await db.SaveChangesAsync();
                    var torateList = db.rateTypeWiseRateList.Where(r => r.rateTypeId == FromRatetypeid).ToList();
                    torateList.ForEach(rate => rate.rateTypeId = ToRatetypeid);
                    db.rateTypeWiseRateList.AddRange(torateList);
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
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

        async Task<ServiceStatusResponseModel> ICentrePaymentServices.ClientDepositReport(List<int> Centreids, DateTime FromDate, DateTime ToDate, string Paymenttype)
        {
            try
            {
                var result = (from cm in db.centreMaster
                              join cp in db.CentrePayment on cm.centreId equals cp.centreId
                              where Centreids.Contains(cm.centreId) && cp.createdDate >= FromDate && cp.createdDate <= ToDate
                              orderby cm.centreId
                              select new
                              {
                                  CentreName = cm.companyName,
                                  cp.paymentDate,
                                  cp.advancePaymentAmt,
                                  cp.paymentType,
                                  cp.remarks,
                                  cp.paymentMode
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
    }
}
