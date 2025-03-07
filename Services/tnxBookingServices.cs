using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace iMARSARLIMS.Services
{
    public class tnxBookingServices : ItnxBookingServices
    {
        private readonly ContextClass db;
        private readonly MySql_Procedure_Services _MySql_Procedure_Services;
        public tnxBookingServices(ContextClass context, ILogger<BaseController<tnx_Booking>> logger, MySql_Procedure_Services mySql_Procedure_Services)
        {
            db = context;
            this._MySql_Procedure_Services = mySql_Procedure_Services;
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

                var fromDate = patientdata.FromDate.Date; // Strip the time portion
                var toDate = patientdata.ToDate.Date.AddHours(24).AddSeconds(-1); // Include the entire end date

                query = query.Where(q => q.datefilter >= fromDate && q.datefilter <= toDate);


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

        async Task<ServiceStatusResponseModel> ItnxBookingServices.GetMicroresult(int testid,int reportStatus)
        {
            try
            {
                var data = await (from tbi in db.tnx_BookingItem
                                  join tr in db.tnx_Observations_Micro_Flowcyto on tbi.id equals tr.testId into trJoin
                                  from tr in trJoin.DefaultIfEmpty()
                                  where tbi.id == testid && tr.reportStatus == reportStatus
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

                var groupedDataOrganismWise = data.GroupBy(m => new
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


                var groupedDatatestid = groupedDataOrganismWise.GroupBy(m => new
                {
                    m.isApproved,
                    m.hold,
                    m.approvalDoctor,
                    m.approvedByID,
                    m.testId,
                    m.colonyCount,
                    m.positivity,

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

                    intensity = group.Key.intensity,
                    comments = group.Key.comments,
                    intrim = group.Key.intrim,
                    result = group.Key.result,
                    OrganismMapped = group
                        .Where(child => child.organismId != 0)
                        .Select(child => new
                        {
                            organismId = child.organismId,
                            organismName = child.organismName,
                            AntibiticMapped = child.AntibiticMapped
                        }).ToList()
                })
                .OrderBy(parent => parent.testId).FirstOrDefault();
                if (data != null)
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Data = groupedDatatestid
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

        async Task<ServiceStatusResponseModel> ItnxBookingServices.GetPatientDetail(string workorderId)
        {
            try
            {
                var bookingData = await (from tb in db.tnx_Booking
                                  join tm in db.titleMaster on tb.title_id equals tm.id
                                  join tbi in db.tnx_BookingItem on tb.workOrderId equals tbi.workOrderId
                                  join cm in db.centreMaster on tb.centreId equals cm.centreId
                                  join dr in db.doctorReferalMaster on tb.refID1 equals dr.doctorId
                                  where tb.workOrderId == workorderId
                                        && (string.IsNullOrEmpty(tbi.packageName) || tbi.packageName == tbi.investigationName)
                                  select new
                                  {
                                      tb.bookingDate,
                                      tb.workOrderId,
                                      tb.transactionId,
                                      PatientName = tm.title + " " + tb.name,
                                      tb.netAmount,
                                      tb.grossAmount,
                                      tb.discount,
                                      tb.paidAmount,
                                      tb.mobileNo,
                                      cm.companyName,
                                      cm.centrecode,
                                      tb.patientId,
                                      Status = tbi.isSampleCollected == "Y" && tbi.isResultDone == 1 && tbi.isApproved == 1 ? "Approved" :
                                               tbi.isSampleCollected == "Y" && tbi.isResultDone == 1 && tbi.isApproved == 0 ? "ResultDone" :
                                               tbi.isSampleCollected == "Y" && tbi.isResultDone == 0 && tbi.isApproved == 0 ? "Sample Received" :
                                               tbi.isSampleCollected == "S" && tbi.isResultDone == 0 && tbi.isApproved == 0 ? "Sample Collected" : "Registered"
                                  }).ToListAsync();

                if(bookingData!=null)
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Data = bookingData
                    };
                }
                else
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message="No Data Found"
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

        async Task<ServiceStatusResponseModel> ItnxBookingServices.GetDispatchData(DispatchDataRequestModel patientdata)
        {
            var query = from tb in db.tnx_Booking
                        join tbi in db.tnx_BookingItem on tb.transactionId equals tbi.transactionId
                        join im in db.itemMaster on tbi.itemId equals im.itemId
                        join cm in db.centreMaster on tb.centreId equals cm.centreId
                        join tm in db.titleMaster on tb.title_id equals tm.id
                        select new
                        {
                            bookingDate = tb.bookingDate.ToString("yyyy-MMM-dd hh:mm tt"),
                            tb.workOrderId,
                            SampleRecievedDate = tbi.sampleReceiveDate.HasValue ? tbi.sampleReceiveDate.Value.ToString("yyyy-MMM-dd hh:mm tt") : "",
                            ApproveDate= tbi.approvedDate.HasValue ? tbi.approvedDate.Value.ToString("yyyy-MMM-dd hh:mm tt") : "",
                            PatientName = string.Concat(tm.title, " ", tb.name),
                            Age = string.Concat(tb.ageYear, " Y ", tb.ageMonth, " M ", tb.ageDay, " D/",tb.gender),
                            tbi.barcodeNo,
                            investigationName = tbi.isPackage == 1 ? string.Concat(tbi.packageName, "<br>", tbi.investigationName) : tbi.investigationName,
                            Remark = tb.labRemarks,
                            cm.centrecode,
                            centreName = cm.companyName,
                            tbi.departmentName,
                            tbi.isSampleCollected,
                            tb.transactionId,
                            tbi.sampleCollectionDate,
                            im.reportType,
                            Comment="",
                            resultdone = tbi.isResultDone,
                            Approved = tbi.isApproved,
                            BokkingDateFilter = tb.bookingDate,
                            approvedDateFilter= tbi.approvedDate,
                            sampleReceiveDateFilter= tbi.sampleReceiveDate,
                            tbi.centreId,tbi.itemId,
                            tbi.deptId                  
                        };
            if (patientdata.Datetype != "")
            {
                var fromDate = patientdata.FromDate.Date; // Strip the time portion
                var toDate = patientdata.ToDate.Date.AddHours(24).AddSeconds(-1); // Include the entire end date

                if (patientdata.Datetype == "tb.bookingDate")
                {
                    query = query.Where(q => q.BokkingDateFilter >= fromDate && q.BokkingDateFilter <= toDate);
                }
                else if (patientdata.searchvalue == "tbi.approvedDate")
                {
                    query = query.Where(q => q.approvedDateFilter >= fromDate && q.approvedDateFilter <= toDate);
                }
                else if (patientdata.searchvalue == "tbi.sampleReceiveDate")
                {
                    query = query.Where(q => q.sampleReceiveDateFilter >= fromDate && q.sampleReceiveDateFilter <= toDate);
                }
                else
                {
                    query = query.Where(q => q.sampleCollectionDate >= fromDate && q.sampleCollectionDate <= toDate);
                }
            }
            if (patientdata.searchvalue != "")
            {
                query = query.Where(q => q.barcodeNo == patientdata.searchvalue || q.workOrderId == patientdata.searchvalue);
            }
            if (patientdata.ItemIds.Count > 0)
            {
                query = query.Where(q => patientdata.ItemIds.Contains(q.itemId));
            }
            if (patientdata.centreId > 0)
            {
                query = query.Where(q => q.centreId== patientdata.centreId);
            }
            else
            {
                List<int> CentreIds = db.empCenterAccess.Where(e => e.empId == patientdata.empid).Select(e => e.centreId).ToList();
                query = query.Where(q => CentreIds.Contains(q.centreId));
            }

            if (patientdata.departmentIds.Count > 0)
            {
                query = query.Where(q => patientdata.departmentIds.Contains(q.deptId));
            }
            query = query.OrderBy(q => q.workOrderId).ThenBy(q => q.deptId);
            var result = await query.ToListAsync();
            return new ServiceStatusResponseModel
            {
                Success = true,
                Data = result
            };
        }

        async Task<ServiceStatusResponseModel> ItnxBookingServices.GetTestInfo(int TestId)
        {
            try
            {
                var data = await (from tb in db.tnx_Booking
                                  join tbi in db.tnx_BookingItem on tb.workOrderId equals tbi.workOrderId
                                  join em in db.empMaster on tb.createdById equals em.empId
                                  where tbi.id == TestId
                                  select new
                                  {
                                      tbi.investigationName,
                                      tbi.barcodeNo,
                                      registrationDate = tb.bookingDate.ToString("yyyy-MMM-dd hh:mm tt"),
                                      RegisterBy = em.fName + " " + em.lName,
                                      sampleCollectionDate = tbi.sampleCollectionDate.HasValue ? tbi.sampleCollectionDate.Value.ToString("yyyy-MMM-dd hh:mm tt") : "",
                                      tbi.sampleCollectedby,
                                      sampleReceiveDate = tbi.sampleReceiveDate.HasValue ? tbi.sampleReceiveDate.Value.ToString("yyyy-MMM-dd hh:mm tt") : "",
                                      tbi.sampleReceivedBY,
                                      resultDate = tbi.resultDate.HasValue ? tbi.resultDate.Value.ToString("yyyy-MMM-dd hh:mm tt") : "",
                                      tbi.resutDoneBy,
                                      approvedDate = tbi.approvedDate.HasValue ? tbi.approvedDate.Value.ToString("yyyy-MMM-dd hh:mm tt") : "",
                                      tbi.approvedbyName,
                                      outhouseDoneOn = tbi.outhouseDoneOn.HasValue ? tbi.outhouseDoneOn.Value.ToString("yyyy-MMM-dd hh:mm tt") : "",
                                      tbi.outhouseLab,
                                      tbi.outhouseDoneBy,
                                      OutSource="",
                                      OutSourceDate="",
                                      OutSouceLab=""

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
                    Message = ex.Message
                };
            }
        }

        async Task<ServiceStatusResponseModel> ItnxBookingServices.GetbarcodeChangedetail(string WorkOrderId)
        {
            try
            {
                var data = await(from tb in db.tnx_Booking
                                 join tbi in db.tnx_BookingItem on tb.workOrderId equals tbi.workOrderId
                                 join cm in db.centreMaster on tb.centreId equals cm.centreId 
                                 where tbi.workOrderId == WorkOrderId && cm.isPrePrintedBarcode == 1
                                 select new
                                 {
                                     TestId=tbi.id,
                                     tb.workOrderId,
                                     patientName= tb.name,
                                     Age=string.Concat(tb.ageYear," Y ",tb.ageMonth," M ",tb.ageDay," D/",tb.gender),
                                     tbi.investigationName,
                                     tbi.barcodeNo,
                                     tbi.sampleTypeName,
                                     registrationDate = tb.bookingDate.ToString("yyyy-MMM-dd hh:mm tt"),
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
                    Message = ex.Message
                };
            }
        }

        async Task<ServiceStatusResponseModel> ItnxBookingServices.UpdateBarcode(List<barcodeChangeRequest> NewBarcodeData)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    foreach (var item in NewBarcodeData)
                    {
                        var count = db.tnx_BookingItem.Where(b => b.barcodeNo == item.BarcodeNoNew).Count();
                        if (count > 0)
                        {
                            return new ServiceStatusResponseModel
                            {
                                Success = false,
                                Message = "BarcodenNo"+ item.BarcodeNoNew + " Already Exit"
                            };
                        }
                        else
                        {
                            var data = db.tnx_BookingItem.Where(b => item.TestIds.Contains(b.id)).ToList();
                            if (data.Count > 0)
                            {
                                foreach (var itemrow in data)
                                {
                                    itemrow.barcodeNo = item.BarcodeNoNew;
                                }
                                db.tnx_BookingItem.UpdateRange(data);
                                await db.SaveChangesAsync();
                            }
                        }
                    }
                    await transaction.CommitAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "Updated Successful"
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

        async Task<ServiceStatusResponseModel> ItnxBookingServices.TatReport(DateTime FromDate, DateTime ToDate, int centreId, int departmentId, int itemid, string TatType)
        {
            var fromDate = FromDate.Date; // Strip the time portion
            var toDate = ToDate.Date.AddHours(24).AddSeconds(-1); // Include the entire end date
            var result =  _MySql_Procedure_Services.TatReportexcel(FromDate, ToDate, centreId,departmentId,itemid,TatType);
            
            if(result.Count>0)
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
                    Message = "No Data Found"
                };
            }
        }

        public byte[] TatReportExcel(DateTime FromDate, DateTime ToDate, int centreId, int departmentId, int itemid, string TatType)
        {


            var fromDate = FromDate.Date; // Strip the time portion
            var toDate = ToDate.Date.AddHours(24).AddSeconds(-1); // Include the entire end date

            var result =  _MySql_Procedure_Services.TatReportexcel(FromDate, ToDate, centreId, departmentId, itemid, TatType);
            var excelByte = MyFunction.ExportToExcel(result, "LedgerReportExcel");
            return excelByte;
        }
        public byte[] TatReportpdf(DateTime FromDate, DateTime ToDate, int centreId, int departmentId, int itemid, string TatType)
        {


            var fromDate = FromDate.Date; // Strip the time portion
            var toDate = ToDate.Date.AddHours(24).AddSeconds(-1); // Include the entire end date

            var result = _MySql_Procedure_Services.TatReportexcel(fromDate, toDate, centreId, departmentId, itemid, TatType);
            if (result.Count > 0)
            {
                QuestPDF.Settings.License = LicenseType.Community;

                var document = Document.Create(container =>
                {

                    container.Page(page =>
                    {
                        page.Size(PageSizes.A3);

                        page.MarginTop(1.0f, Unit.Centimetre);
                        page.MarginLeft(0.5f, Unit.Centimetre);
                        page.MarginRight(0.5f, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.Foreground();
                        page.DefaultTextStyle(x => x.FontFamily("TimesNewRoman"));
                        page.DefaultTextStyle(x => x.FontSize(10));
                        page.Header()
                        .Column(column =>
                        {
                            column.Item().Text("TAT Report").AlignCenter().Bold();
                        });

                        page.Content()
                        .Column(column =>
                        {
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(1.0f,Unit.Centimetre);
                                    columns.ConstantColumn(2.5f,Unit.Centimetre);
                                    columns.ConstantColumn(2.0f,Unit.Centimetre);
                                    columns.ConstantColumn(2.5f,Unit.Centimetre);
                                    columns.ConstantColumn(2.0f,Unit.Centimetre);
                                    columns.ConstantColumn(2.0f,Unit.Centimetre);
                                    columns.ConstantColumn(2.0f,Unit.Centimetre);
                                    columns.ConstantColumn(2.0f,Unit.Centimetre);
                                    columns.ConstantColumn(2.5f,Unit.Centimetre);
                                    columns.ConstantColumn(2.5f,Unit.Centimetre);
                                    columns.ConstantColumn(2.5f,Unit.Centimetre);
                                    columns.ConstantColumn(1.5f,Unit.Centimetre);
                                    columns.ConstantColumn(1.5f,Unit.Centimetre);
                                    columns.ConstantColumn(1.5f, Unit.Centimetre);
                                });
                                table.Cell().Text("Sr.No").Style(TextStyle.Default.FontSize(10).Bold());
                                table.Cell().Text("BookingDate").Style(TextStyle.Default.FontSize(10).Bold());
                                table.Cell().Text("VisitId").Style(TextStyle.Default.FontSize(10).Bold());
                                table.Cell().Text("PatientName").Style(TextStyle.Default.FontSize(10).Bold());
                                table.Cell().Text("TestName").Style(TextStyle.Default.FontSize(10).Bold());
                                table.Cell().Text("Department").Style(TextStyle.Default.FontSize(10).Bold());
                                table.Cell().Text("Centre").Style(TextStyle.Default.FontSize(10).Bold());
                                table.Cell().Text("Coll.Date").Style(TextStyle.Default.FontSize(10).Bold());
                                table.Cell().Text("Rec.Date").Style(TextStyle.Default.FontSize(10).Bold());
                                table.Cell().Text("Res.Date").Style(TextStyle.Default.FontSize(10).Bold());
                                table.Cell().Text("App.Date").Style(TextStyle.Default.FontSize(10).Bold());
                                table.Cell().Text("BTOS").Style(TextStyle.Default.FontSize(10).Bold());
                                table.Cell().Text("STOD").Style(TextStyle.Default.FontSize(10).Bold());
                                table.Cell().Text("DTOR").Style(TextStyle.Default.FontSize(10).Bold());
                                var i = 0;
                            foreach(var item in result)
                            {
                                    i = i + 1;
                                table.Cell().Text(i.ToString()).Style(TextStyle.Default.FontSize(10).Bold());
                                table.Cell().Text(item.BookingDate).Style(TextStyle.Default.FontSize(10).Bold());
                                table.Cell().Text(item.WorkorderId).Style(TextStyle.Default.FontSize(10).Bold());
                                table.Cell().Text(item.PatientName).Style(TextStyle.Default.FontSize(10).Bold());
                                table.Cell().Text(item.TestName).Style(TextStyle.Default.FontSize(10).Bold());
                                table.Cell().Text(item.Department).Style(TextStyle.Default.FontSize(10).Bold());
                                table.Cell().Text(item.CentreCode).Style(TextStyle.Default.FontSize(10).Bold());
                                table.Cell().Text(item.SampleCollectionDate).Style(TextStyle.Default.FontSize(10).Bold());
                                table.Cell().Text(item.SampleReceivedDate).Style(TextStyle.Default.FontSize(10).Bold());
                                table.Cell().Text(item.ResultDate).Style(TextStyle.Default.FontSize(10).Bold());
                                table.Cell().Text(item.ApproveDate).Style(TextStyle.Default.FontSize(10).Bold());
                                table.Cell().Text(item.BTOS.ToString()).Style(TextStyle.Default.FontSize(10).Bold());
                                table.Cell().Text(item.STOD.ToString()).Style(TextStyle.Default.FontSize(10).Bold());
                                table.Cell().Text(item.DTOR.ToString()).Style(TextStyle.Default.FontSize(10).Bold());

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
                                   
                                   table.Cell().ColumnSpan(3).AlignRight().AlignBottom().Text(text =>
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

