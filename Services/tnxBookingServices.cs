using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using iText.Pdfua.Checkers.Utils;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Services
{
    public class tnxBookingServices : ItnxBookingServices
    {
        private readonly ContextClass db;
        public tnxBookingServices(ContextClass context, ILogger<BaseController<tnx_Booking>> logger)
        {
            db = context;
        }

        public string GetPatientDocumnet(string workOrderId)
        {
            try
            {
                var documentData = db.tnx_Booking
                                     .Where(b => b.workOrderId == workOrderId)
                                     .Select(b => b.uploadDocument)
                                     .FirstOrDefault();

                return documentData;

            }
            catch
            {
                return "";
            }
        }

        async Task<ServiceStatusResponseModel> ItnxBookingServices.GetPaymentDetails(string workOrderId)
        {
            try
            {
                var data = await (from tb in db.tnx_Booking
                                  join tr in db.tnx_ReceiptDetails on tb.transactionId equals tr.transactionId into trJoin
                                  from tr in trJoin.DefaultIfEmpty()  // Left join: if no matching receipt detail, it will be null
                                  where tb.workOrderId == workOrderId && (tr.isCancel == 0 || tr.isCancel == null)
                                  select new
                                  {
                                      tb.workOrderId,
                                      tb.name,
                                      Age = string.Concat(tb.ageYear, " Y ", tb.ageMonth, " M ", tb.ageDay, " D/", tb.gender),
                                      tb.transactionId,
                                      tb.centreId,
                                      tb.grossAmount,
                                      tb.discount,
                                      tb.netAmount,
                                      tb.paidAmount,
                                      DueAmt = tb.netAmount - tb.paidAmount,
                                      tb.paymentMode,
                                      RecieptNo = tr != null ? tr.id : 0,
                                      cashAmt = tr.cashAmt.HasValue ? tr.cashAmt.Value.ToString() : "",
                                      onlineAmt = tr.onlinewalletAmt.HasValue ? tr.onlinewalletAmt.Value.ToString() : "",

                                      ChequeAmt = tr.chequeAmt.HasValue ? tr.chequeAmt.Value.ToString() : ""
                                  }).ToListAsync();

                if (data != null)
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Data = data
                    };
                }
                else
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = "No Data Found"
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

        async Task<ServiceStatusResponseModel> ItnxBookingServices.GetPatientData(patientDataRequestModel patientdata)
        {
            try
            {
                var query = from tb in db.tnx_Booking
                            join dr in db.doctorReferalMaster on tb.refID1 equals dr.doctorId
                            join cm in db.centreMaster on tb.centreId equals cm.centreId
                            join em in db.empMaster on tb.createdById equals em.empId
                            join rt in db.rateTypeMaster on tb.rateId equals rt.rateTypeId
                            select new
                            {
                                bookingDate = tb.bookingDate.ToString("yyyy-MMM-dd hh:mm tt"),
                                tb.transactionId,
                                datefilter = tb.bookingDate,
                                tb.workOrderId,
                                tb.name,
                                Age = string.Concat(tb.ageYear, " Y ", tb.ageMonth, " M ", tb.ageDay, " D/", tb.gender),
                                tb.mobileNo,
                                RefDoctor = dr.doctorName,
                                CentreName = cm.centrecode,
                                ratetype = rt.rateType,
                                RegBy = string.Concat(em.fName, " ", em.lName),
                                tb.grossAmount,
                                tb.discount,
                                tb.netAmount,
                                tb.paidAmount,
                                tb.createdById,
                                tb.centreId,
                                tb.paymentMode,
                                documnet = !string.IsNullOrEmpty(tb.uploadDocument) ? 1 : 0,
                                DueAmt = tb.netAmount - tb.paidAmount
                            };

                query = query.Where(q => q.datefilter >= patientdata.FromDate && q.datefilter <= patientdata.ToDate);


                if (!string.IsNullOrEmpty(patientdata.SearchValue))
                {
                    query = query.Where(q => q.workOrderId == patientdata.SearchValue || q.name == patientdata.SearchValue);
                }
                if (patientdata.UserId != 0)
                {
                    query = query.Where(q => q.createdById == patientdata.UserId);
                }
                if (patientdata.CentreIds.Count > 0)
                {
                    query = query.Where(q => patientdata.CentreIds.Contains(q.centreId));
                }
                if (patientdata.status != "")
                {
                    if (patientdata.status == "Credit")
                        query = query.Where(q => q.paymentMode == "Credit");
                    else if (patientdata.status == "PartialPaid")
                        query = query.Where(q => q.netAmount != q.paidAmount);
                    else if (patientdata.status == "Paid")
                        query = query.Where(q => q.netAmount == q.paidAmount);
                    else
                        query = query.Where(q => q.paidAmount == 0);
                }
                query = query.OrderBy(q => q.workOrderId);

                var patientData = await query.ToListAsync();
                if (patientData != null && patientData.Any())
                {
                    return new ServiceStatusResponseModel { Success = true, Data = patientData };
                }
                else
                {
                    return new ServiceStatusResponseModel { Success = false, Message = "No Data Found" };
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

        async Task<ServiceStatusResponseModel> ItnxBookingServices.GetHistoresult(int testid)
        {
            try
            {
                var data = await (from tbi in db.tnx_BookingItem
                                  join tr in db.tnx_Observations_Histo on tbi.id equals tr.testId into trJoin
                                  from tr in trJoin.DefaultIfEmpty()  // Left join: if no matching receipt detail, it will be null
                                  where tbi.id == testid
                                  select new
                                  {
                                      tbi.isApproved,
                                      tbi.hold,
                                      tbi.approvalDoctor,
                                      tbi.approvedByID,
                                      HistoObservationId = tr != null ? tr.histoObservationId : 0,
                                      testId = tr.testId.HasValue ? tr.testId.Value.ToString() : "",
                                      clinicalHistory = !string.IsNullOrEmpty(tr.clinicalHistory) ? tr.clinicalHistory : "",
                                      specimen = !string.IsNullOrEmpty(tr.specimen) ? tr.specimen : "",
                                      gross = !string.IsNullOrEmpty(tr.gross) ? tr.gross : "",
                                      typesFixativeUsed = !string.IsNullOrEmpty(tr.typesFixativeUsed) ? tr.typesFixativeUsed : "",
                                      blockKeys = !string.IsNullOrEmpty(tr.blockKeys) ? tr.blockKeys : "",
                                      stainsPerformed = !string.IsNullOrEmpty(tr.stainsPerformed) ? tr.stainsPerformed : "",
                                      biospyNumber = !string.IsNullOrEmpty(tr.biospyNumber) ? tr.biospyNumber : "",
                                      microscopy = !string.IsNullOrEmpty(tr.microscopy) ? tr.microscopy : "",
                                      finalImpression = !string.IsNullOrEmpty(tr.finalImpression) ? tr.finalImpression : "",
                                      comment = !string.IsNullOrEmpty(tr.comment) ? tr.comment : ""

                                  }).ToListAsync();

                if (data != null)
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Data = data
                    };
                }
                else
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = "No Data Found"
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

        async Task<ServiceStatusResponseModel> ItnxBookingServices.GetMicroresult(int testid)
        {
            try
            {
                var data = await (from tbi in db.tnx_BookingItem
                                  join tr in db.tnx_Observations_Micro_Flowcyto on tbi.id equals tr.testId into trJoin
                                  from tr in trJoin.DefaultIfEmpty()
                                  where tbi.id == testid
                                  select new
                                  {
                                      tbi.isApproved,
                                      tbi.hold,
                                      tbi.approvalDoctor,
                                      tbi.approvedByID,
                                      testId = tbi.id,
                                      antibiticId = tr != null ? tr.antibiticId : 0,
                                      antibitiName = tr != null && !string.IsNullOrEmpty(tr.antibitiName) ? tr.antibitiName : "",
                                      mic = tr != null && !string.IsNullOrEmpty(tr.mic) ? tr.mic : "",
                                      interpretation = tr != null && !string.IsNullOrEmpty(tr.interpretation) ? tr.interpretation : "",
                                      colonyCount = tr != null && !string.IsNullOrEmpty(tr.colonyCount) ? tr.colonyCount : "",
                                      positivity = tr != null && !string.IsNullOrEmpty(tr.positivity) ? tr.positivity : "",
                                      organismId = tr != null ? tr.organismId : 0,
                                      organismName = tr != null && !string.IsNullOrEmpty(tr.organismName) ? tr.organismName : "",
                                      intensity = tr != null && !string.IsNullOrEmpty(tr.intensity) ? tr.intensity : "",
                                      comments = tr != null && !string.IsNullOrEmpty(tr.comments) ? tr.comments : "",
                                      intrim = tr != null ? tr.reportStatus : null,
                                      result = tr != null && !string.IsNullOrEmpty(tr.result) ? tr.result : "",
                                  }).ToListAsync();

                var groupedData = data.GroupBy(m => new
                {
                    m.isApproved,
                    m.hold,
                    m.approvalDoctor,
                    m.approvedByID,
                    m.testId,
                    m.colonyCount,
                    m.positivity,
                    m.organismId,
                    m.organismName,
                    m.intensity,
                    m.comments,
                    m.intrim,
                    m.result,
                })
                .Select(group => new
                {
                    isApproved = group.Key.isApproved,
                    hold = group.Key.hold,
                    approvalDoctor = group.Key.approvalDoctor,
                    approvedByID = group.Key.approvedByID,
                    testId = group.Key.testId,
                    colonyCount = group.Key.colonyCount,
                    positivity = group.Key.positivity,
                    organismId = group.Key.organismId,
                    organismName = group.Key.organismName,
                    intensity = group.Key.intensity,
                    comments = group.Key.comments,
                    intrim = group.Key.intrim,
                    result = group.Key.result,
                    AntibiticMapped = group
                        .Where(child => child.antibiticId != 0)
                        .Select(child => new
                        {
                            antibiticId = child.antibiticId,
                            antibitiName = child.antibitiName,
                            mic = child.mic,
                            interpretation = child.interpretation,
                        }).ToList()
                })
                .OrderBy(parent => parent.organismId)
                .ToList();


                if (data != null)
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Data = groupedData
                    };
                }
                else
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = "No Data Found"
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

        async Task<ServiceStatusResponseModel> ItnxBookingServices.SaveSettelmentDetail(List<settelmentRequestModel> settelments)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var settelmentDataList = settelments.Select(settelment => CreatesettelmentData(settelment)).ToList();
                    db.tnx_ReceiptDetails.AddRange(settelmentDataList);
                    await db.SaveChangesAsync();

                    var workorderid= settelments.Select(s=>s.workOrderId).First();
                    var paidamt = settelments.Select(s => s.PaidAmt).First();

                    var data = db.tnx_Booking.Where(b => b.workOrderId == workorderid).FirstOrDefault();
                    if (data != null)
                    {
                        data.paidAmount = (double)paidamt;
                        db.tnx_Booking.Update(data);
                        await db.SaveChangesAsync();
                    }
                    await transaction.CommitAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "settelment Successful"
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

        private tnx_ReceiptDetails CreatesettelmentData(settelmentRequestModel settelment)
        {
            return new tnx_ReceiptDetails
            {
                id=0,
                transactionId = settelment.transactionId,
                transactionType = settelment.transactionType,
                workOrderId = settelment.workOrderId,
                receiptNo = settelment.receiptNo,
                receivedAmt = settelment.receivedAmt,
                cashAmt = settelment.cashAmt,
                creditCardAmt = settelment.creditCardAmt,
                creditCardNo = settelment.creditCardNo,
                chequeAmt = settelment.chequeAmt,
                chequeNo = settelment.chequeNo,
                onlinewalletAmt = settelment.onlinewalletAmt,
                walletno = settelment.walletno,
                NEFTamt = settelment.NEFTamt,
                BankName = settelment.BankName,
                paymentModeId = settelment.paymentModeId,
                isCancel = settelment.isCancel,
                cancelDate = settelment.cancelDate,
                canceledBy = settelment.canceledBy,
                cancelReason = settelment.cancelReason,
                bookingCentreId = settelment.bookingCentreId,
                settlementCentreID = settelment.settlementCentreID,
                receivedBy = settelment.receivedBy,
                receivedID = settelment.receivedID
            };
        }
    }
}

