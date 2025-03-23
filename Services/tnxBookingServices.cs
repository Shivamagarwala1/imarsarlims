using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Linq;
using System.Text.RegularExpressions;

namespace iMARSARLIMS.Services
{
    public class tnxBookingServices : ItnxBookingServices
    {
        private readonly ContextClass db;
        private readonly IConfiguration _configuration;
        private readonly MySql_Procedure_Services _MySql_Procedure_Services;
        public tnxBookingServices(ContextClass context, ILogger<BaseController<tnx_Booking>> logger, MySql_Procedure_Services mySql_Procedure_Services, IConfiguration configuration)
        {
            db = context;
            this._MySql_Procedure_Services = mySql_Procedure_Services;
            this._configuration = configuration;
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
                                      paymentMode = tr.cashAmt.HasValue ? "Cash" : tr.onlinewalletAmt.HasValue ? "OnliNe" : "Cheque",
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

        async Task<ServiceStatusResponseModel> ItnxBookingServices.GetMicroresult(int testid, int reportStatus)
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

                    var workorderid = settelments.Select(s => s.workOrderId).First();
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
                id = 0,
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
                                             Age= string.Concat(tb.ageYear,"Y /" , tb.gender),
                                             tb.netAmount,
                                             tb.grossAmount,
                                             testid= tbi.id,
                                             tbi.investigationName,
                                             tb.discount,
                                             tb.paidAmount,
                                             tb.mobileNo,
                                             cm.companyName,
                                             cm.centrecode,
                                             tb.patientId,
                                             tbi.isSampleCollected,
                                             Status = tbi.isSampleCollected == "Y" && tbi.isResultDone == 1 && tbi.isApproved == 1 ? "Approved" :
                                                      tbi.isSampleCollected == "Y" && tbi.isResultDone == 1 && tbi.isApproved == 0 ? "ResultDone" :
                                                      tbi.isSampleCollected == "Y" && tbi.isResultDone == 0 && tbi.isApproved == 0 ? "Sample Received" :
                                                      tbi.isSampleCollected == "S" && tbi.isResultDone == 0 && tbi.isApproved == 0 ? "Sample Collected" : "Registered"
                                         }).ToListAsync();

                if (bookingData != null)
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

        async Task<ServiceStatusResponseModel> ItnxBookingServices.GetDispatchData(DispatchDataRequestModel patientdata)
        {
            var query = from tb in db.tnx_Booking
                        join tbi in db.tnx_BookingItem on tb.transactionId equals tbi.transactionId
                        join im in db.itemMaster on tbi.itemId equals im.itemId
                        join cm in db.centreMaster on tb.centreId equals cm.centreId
                        join dr in db.doctorReferalMaster on tb.refID1 equals dr.doctorId
                        join tm in db.titleMaster on tb.title_id equals tm.id
                        join em in db.empMaster on tb.createdById equals em.empId
                        select new
                        {
                            bookingDate = tb.bookingDate.ToString("yyyy-MMM-dd hh:mm tt"),
                            tb.workOrderId,
                            SampleRecievedDate = tbi.sampleReceiveDate.HasValue ? tbi.sampleReceiveDate.Value.ToString("yyyy-MMM-dd hh:mm tt") : "",
                            ApproveDate = tbi.approvedDate.HasValue ? tbi.approvedDate.Value.ToString("yyyy-MMM-dd hh:mm tt") : "",
                            PatientName = string.Concat(tm.title, " ", tb.name),
                            Age = string.Concat(tb.ageYear, " Y ", tb.ageMonth, " M ", tb.ageDay, " D/", tb.gender),
                            tbi.barcodeNo,
                            tb.mobileNo,
                            investigationName = tbi.isPackage == 1 ? string.Concat(tbi.packageName, "-", tbi.investigationName) : tbi.investigationName,
                            Remark = tb.labRemarks,
                            cm.centrecode,
                            centreName = cm.companyName,
                            tbi.departmentName,
                            tbi.isSampleCollected,
                            tb.transactionId,
                            tbi.sampleCollectionDate,
                            im.reportType,
                            Comment = "",
                            resultdone = tbi.isResultDone,
                            Approved = tbi.isApproved,
                            BokkingDateFilter = tb.bookingDate,
                            approvedDateFilter = tbi.approvedDate,
                            sampleReceiveDateFilter = tbi.sampleReceiveDate,
                            tbi.centreId,
                            tbi.itemId,
                            tbi.deptId,
                            ReferDoctor = dr.doctorName,
                            CreatedBy = em.fName,
                            testId = tbi.id,
                            Email = tbi.isEmailsent,
                            Whatsapp = tbi.isWhatsApp,
                            isremark = db.tnx_InvestigationRemarks.Where(s => s.itemId == tbi.itemId && s.transactionId == tbi.transactionId).Count()


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
                query = query.Where(q => q.centreId == patientdata.centreId);
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

        async Task<ServiceStatusResponseModel> ItnxBookingServices.GetTestInfo(string TestId)
        {
            try
            {
                // Check for null or empty TestId and return early if invalid
                if (string.IsNullOrEmpty(TestId))
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = "TestId cannot be null or empty."
                    };
                }

                List<int> testIds = TestId.Split(',').Select(id => int.Parse(id)).ToList();

                var data = await (from tb in db.tnx_Booking
                                  join tbi in db.tnx_BookingItem on tb.workOrderId equals tbi.workOrderId
                                  join em in db.empMaster on tb.createdById equals em.empId
                                  where testIds.Contains(tbi.id)
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
                                      OutSource = "",
                                      OutSourceDate = "",
                                      OutSouceLab = ""
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
                var data = await (from tb in db.tnx_Booking
                                  join tbi in db.tnx_BookingItem on tb.workOrderId equals tbi.workOrderId
                                  join cm in db.centreMaster on tb.centreId equals cm.centreId
                                  where tbi.workOrderId == WorkOrderId && cm.isPrePrintedBarcode == 1
                                  select new
                                  {
                                      TestId = tbi.id,
                                      tb.workOrderId,
                                      patientName = tb.name,
                                      Age = string.Concat(tb.ageYear, " Y ", tb.ageMonth, " M ", tb.ageDay, " D/", tb.gender),
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
                                Message = "BarcodenNo" + item.BarcodeNoNew + " Already Exit"
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
            var result = _MySql_Procedure_Services.TatReportexcel(FromDate, ToDate, centreId, departmentId, itemid, TatType);

            if (result.Count > 0)
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

            var result = _MySql_Procedure_Services.TatReportexcel(FromDate, ToDate, centreId, departmentId, itemid, TatType);
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
                                    columns.ConstantColumn(1.0f, Unit.Centimetre);
                                    columns.ConstantColumn(2.5f, Unit.Centimetre);
                                    columns.ConstantColumn(2.0f, Unit.Centimetre);
                                    columns.ConstantColumn(2.5f, Unit.Centimetre);
                                    columns.ConstantColumn(2.0f, Unit.Centimetre);
                                    columns.ConstantColumn(2.0f, Unit.Centimetre);
                                    columns.ConstantColumn(2.0f, Unit.Centimetre);
                                    columns.ConstantColumn(2.0f, Unit.Centimetre);
                                    columns.ConstantColumn(2.5f, Unit.Centimetre);
                                    columns.ConstantColumn(2.5f, Unit.Centimetre);
                                    columns.ConstantColumn(2.5f, Unit.Centimetre);
                                    columns.ConstantColumn(1.5f, Unit.Centimetre);
                                    columns.ConstantColumn(1.5f, Unit.Centimetre);
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
                                foreach (var item in result)
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

        async Task<ServiceStatusResponseModel> ItnxBookingServices.GetMethodChangedetail(string WorkOrderId)
        {
            try
            {
                var data = await (from tb in db.tnx_Booking
                                  join tbi in db.tnx_BookingItem on tb.workOrderId equals tbi.workOrderId
                                  join tbo in db.tnx_Observations on tbi.id equals tbo.testId
                                  where tbi.workOrderId == WorkOrderId
                                  select new
                                  {
                                      tb.workOrderId,
                                      patientName = tb.name,
                                      Age = string.Concat(tb.ageYear, " Y ", tb.ageMonth, " M ", tb.ageDay, " D/", tb.gender),
                                      tbi.investigationName,
                                      tbi.barcodeNo,
                                      observationdataid = tbo.id,
                                      tbo.observationName,
                                      tbo.testMethod,
                                      registrationDate = tb.bookingDate.ToString("yyyy-MMM-dd hh:mm tt"),
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
                        Message = "Result Not Saved"
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

        async Task<ServiceStatusResponseModel> ItnxBookingServices.UpdateMethod(List<methodChangeRequestModel> methoddata)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    foreach (var item in methoddata)
                    {
                        var data = db.tnx_Observations.Where(b => b.id == item.id).FirstOrDefault();
                        if (data != null)
                        {
                            data.testMethod = item.Method;
                            db.tnx_Observations.Update(data);
                            await db.SaveChangesAsync();
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
        async Task<ServiceStatusResponseModel> ItnxBookingServices.GetWorkSheetData(WorkSheetRequestModel worksheetdata)
        {
            try
            {
                var query = from tb in db.tnx_Booking
                            join tbi in db.tnx_BookingItem on tb.transactionId equals tbi.transactionId
                            join im in db.itemMaster on tbi.itemId equals im.itemId
                            join cm in db.centreMaster on tb.centreId equals cm.centreId
                            join tm in db.titleMaster on tb.title_id equals tm.id
                            where tb.bookingDate >= worksheetdata.FromDate && tb.bookingDate <= worksheetdata.ToDate
                            select new
                            {
                                BookingDate = tb.bookingDate.ToString("yyyy-MMM-dd hh:mm tt"),
                                tbi.barcodeNo,
                                PatientName = string.Concat(tm.title, " ", tb.name),
                                Age = string.Concat(tb.ageYear, " Y ", tb.ageMonth, " M ", tb.ageDay, " D/", tb.gender),
                                investigationName = tbi.isPackage == 1 ? string.Concat(tbi.packageName, "<br>", tbi.investigationName) : tbi.investigationName,
                                cm.centrecode,
                                centreName = cm.companyName,
                                testid = tbi.id,
                                tbi.itemId,
                                tbi.deptId,
                                tbi.departmentName,
                                tbi.isSampleCollected,
                                Urgent = tbi.isUrgent,
                                resultdone = tbi.isResultDone,
                                Approved = tbi.isApproved,
                                tb.centreId,
                                tb.workOrderId
                            };

                if (worksheetdata.BarcodeNo != "")
                {
                    query = query.Where(q => q.barcodeNo == worksheetdata.BarcodeNo);
                }
                if (worksheetdata.DeptId > 0)
                {
                    query = query.Where(q => q.deptId == worksheetdata.DeptId);
                }
                if (worksheetdata.CentreId > 0)
                {
                    query = query.Where(q => q.centreId == worksheetdata.CentreId);
                }
                else
                {
                    List<int> CentreIds = db.empCenterAccess.Where(e => e.empId == worksheetdata.empid).Select(e => e.centreId).ToList();
                    query = query.Where(q => CentreIds.Contains(q.centreId));
                }

                if (worksheetdata.ItemId > 0)
                {
                    query = query.Where(q => q.itemId == worksheetdata.ItemId);
                }
                if (worksheetdata.Status != "")
                {
                    if (worksheetdata.Status == "Pending")
                    {
                        query = query.Where(q => q.isSampleCollected == "Y" && q.resultdone == 0);
                    }
                    else if (worksheetdata.Status == "Tested")
                    {
                        query = query.Where(q => q.resultdone == 1 && q.Approved == 0);
                    }
                    else if (worksheetdata.Status == "Approved")
                    {
                        query = query.Where(q => q.Approved == 1);
                    }
                    else if (worksheetdata.Status == "MachineData")
                    {
                        // query = query.Where(q => q.isSampleCollected == "R");
                    }
                    else
                    {
                        query = query.Where(q => q.Urgent == 1);
                    }
                }

                query = query.OrderBy(q => q.workOrderId).ThenBy(q => q.deptId);
                var result = await query.ToListAsync();
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel { Success = false, Message = ex.Message };
            }
        }
        public byte[] PrintWorkSheet(string TestIds)
        {

            var result = _MySql_Procedure_Services.PrintWorkSheet(TestIds);
            if (result.Count > 0)
            {
                QuestPDF.Settings.License = LicenseType.Community;

                var document = Document.Create(container =>
                {

                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);

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
                            column.Item().Text("WorkSheet").AlignCenter().Bold();
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
                                    columns.RelativeColumn();


                                });

                                var i = 0;
                                var workorderId = "";
                                var investigation = "";

                                foreach (var item in result)
                                {
                                    if (workorderId != "" && workorderId != item.WorkOrderId)
                                    {
                                        table.Cell().RowSpan(4).ColumnSpan(2).BorderBottom(0.5f, Unit.Point).Text("").Style(TextStyle.Default.FontSize(10).Bold());

                                    }
                                    if (workorderId != item.WorkOrderId)
                                    {
                                        table.Cell().ColumnSpan(2).BorderBottom(0.5f, Unit.Point).Text("PatientName: " + item.Pname).Style(TextStyle.Default.FontSize(10).Bold());
                                        table.Cell().ColumnSpan(2).BorderBottom(0.5f, Unit.Point).Text("WorkOrderId: " + item.WorkOrderId).Style(TextStyle.Default.FontSize(10).Bold());
                                        workorderId = item.WorkOrderId;
                                    }
                                    if (investigation != item.InvestigationName)
                                    {
                                        table.Cell().ColumnSpan(4).Text("").Style(TextStyle.Default.FontSize(10).Bold());
                                        table.Cell().ColumnSpan(2).Height(0.5f, Unit.Centimetre).BorderBottom(0.5f, Unit.Point).Text("Test Name:" + item.InvestigationName).Style(TextStyle.Default.FontSize(10).Bold());
                                        table.Cell().ColumnSpan(2).Height(0.5f, Unit.Centimetre).BorderBottom(0.5f, Unit.Point).Text("Barcode No: " + item.Barcodeno).Style(TextStyle.Default.FontSize(10).Bold());

                                    }
                                    investigation = item.InvestigationName;

                                    table.Cell().ColumnSpan(2).Height(0.5f, Unit.Centimetre).Text(item.ObservationName).Style(TextStyle.Default.FontSize(10));
                                    if (item.Value != "Header")
                                        table.Cell().Height(0.5f, Unit.Centimetre).Text(item.Value).Style(TextStyle.Default.FontSize(10));
                                    else
                                        table.Cell().Height(0.5f, Unit.Centimetre).Text("").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().Height(0.5f, Unit.Centimetre).Text("").Style(TextStyle.Default.FontSize(10));

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

        async Task<ServiceStatusResponseModel> ItnxBookingServices.GetSampleTypedetail(string WorkOrderId)
        {
            try
            {
                var data = await (from tb in db.tnx_Booking
                                  join tbi in db.tnx_BookingItem on tb.workOrderId equals tbi.workOrderId
                                  where tbi.workOrderId == WorkOrderId
                                  select new
                                  {
                                      tb.workOrderId,
                                      patientName = tb.name,
                                      Age = string.Concat(tb.ageYear, " Y ", tb.ageMonth, " M ", tb.ageDay, " D/", tb.gender),
                                      tbi.investigationName,
                                      tbi.barcodeNo,
                                      testid = tbi.id,
                                      tbi.sampleTypeName,
                                      tbi.sampleTypeId,
                                      tbi.reportType,
                                      SampletypeData = (from stm in db.itemSampleTypeMapping where stm.itemId == tbi.itemId select new { stm.sampleTypeId, stm.sampleTypeName }).ToList(),
                                      registrationDate = tb.bookingDate.ToString("yyyy-MMM-dd hh:mm tt"),
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
                        Message = "Result Not Saved"
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

        async Task<ServiceStatusResponseModel> ItnxBookingServices.UpdateSampleType(List<SampltypeChangeRequestModel> sampletypedata)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    foreach (var item in sampletypedata)
                    {
                        var data = db.tnx_BookingItem.Where(b => b.id == item.testid).FirstOrDefault();
                        if (data != null)
                        {
                            var changelog = new tnx_BookingStatus
                            {
                                transactionId = data.transactionId,
                                barcodeNo = data.barcodeNo,
                                centreId = data.centreId,
                                status = "oldsample type " + data.sampleTypeName,
                                roleId = 0,
                                isActive = 1,
                                createdById = item.empid,
                                createdDateTime = DateTime.Now,
                                patientId = 0,
                            };
                            db.tnx_BookingStatus.Add(changelog);
                            await db.SaveChangesAsync();
                            data.sampleTypeName = item.Sampletypename;
                            data.sampleTypeId = item.sampletypeId;
                            data.reportType = item.reporttype;
                            db.tnx_BookingItem.Update(data);
                            await db.SaveChangesAsync();
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

        async Task<ServiceStatusResponseModel> ItnxBookingServices.MachineResult(MachineResultRequestModel machineResult)
        {
            try
            {
                var query = from tb in db.tnx_Booking
                            join tbi in db.tnx_BookingItem on tb.transactionId equals tbi.transactionId
                            join mr in db.machine_result on tbi.id equals mr.testId
                            join io in db.itemObservationMaster on mr.observationId equals io.id
                            where mr.createdDateTime >= machineResult.FromDate && mr.createdDateTime <= machineResult.ToDate
                            select new
                            {
                                patientName = tb.name,
                                visitId = tb.workOrderId,
                                tbi.barcodeNo,
                                tbi.investigationName,
                                io.labObservationName,
                                mr.machineName1,
                                mr.macId1,
                                Reading = mr.macReading1,
                                mr.machineComments,
                                tb.centreId
                            };

                if (machineResult.BarcodeNo != "")
                {
                    query = query.Where(q => q.barcodeNo == machineResult.BarcodeNo);
                }
                if (machineResult.MachineId > 0)
                {
                    query = query.Where(q => q.macId1 == machineResult.MachineId);
                }
                if (machineResult.centreId > 0)
                {
                    query = query.Where(q => q.centreId == machineResult.centreId);
                }
                else
                {
                    List<int> CentreIds = db.empCenterAccess.Where(e => e.empId == machineResult.empId).Select(e => e.centreId).ToList();
                    query = query.Where(q => CentreIds.Contains(q.centreId));
                }

                query = query.OrderBy(q => q.visitId);
                var result = await query.ToListAsync();
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

        async Task<ServiceStatusResponseModel> ItnxBookingServices.GetReportDateChangeData(string WorkOrderId)
        {
            try
            {
                var data = await (from tb in db.tnx_Booking
                                  join tbi in db.tnx_BookingItem on tb.workOrderId equals tbi.workOrderId
                                  where tbi.workOrderId == WorkOrderId
                                  select new
                                  {
                                      tb.workOrderId,
                                      patientName = tb.name,
                                      Age = string.Concat(tb.ageYear, " Y ", tb.ageMonth, " M ", tb.ageDay, " D/", tb.gender),
                                      tbi.investigationName,
                                      tbi.barcodeNo,
                                      tbi.isResultDone,
                                      tbi.isApproved,
                                      tbi.isSampleCollected,
                                      TestId = tbi.id,
                                      tbi.sampleCollectionDate,
                                      tbi.sampleReceiveDate,
                                      tbi.resultDate,
                                      tbi.approvedDate,
                                      registrationDate = tb.bookingDate.ToString("yyyy-MMM-dd hh:mm tt"),
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
                        Message = "Result Not Saved"
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

        async Task<ServiceStatusResponseModel> ItnxBookingServices.ReportDateChange(List<DateChangeRequestModel> DateData)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    foreach (var dateChange in DateData)
                    {
                        var data = db.tnx_BookingItem.Where(b => b.id == dateChange.testId).FirstOrDefault();
                        if (data != null)
                        {
                            data.sampleCollectionDate = dateChange.SamplecollectedDate;
                            data.sampleReceiveDate = dateChange.SamplerecievedDate;
                            data.resultDate = dateChange.Resultdate;
                            data.approvedDate = dateChange.ApproveDate;
                        }
                        db.tnx_BookingItem.Update(data);
                        await db.SaveChangesAsync();
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

        async Task<ServiceStatusResponseModel> ItnxBookingServices.SendWhatsapp(string workOrderId, int Userid, string MobileNo, int header)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var data = (from tb in db.tnx_Booking
                                join tbi in db.tnx_BookingItem on tb.workOrderId equals tbi.workOrderId
                                where tb.workOrderId == workOrderId
                                select new
                                {
                                    PateintName = tb.name,
                                    tb.workOrderId,
                                    tb.labRemarks
                                }).FirstOrDefault();
                    if (data != null)
                    {
                        var whatsappData = new whatsapp
                        {
                            id = 0,
                            workOrderId = data.workOrderId,
                            name = data.PateintName,
                            mobileNo = MobileNo,
                            isSend = 0,
                            isAutoSend = 0,
                            sentBy = Userid,
                            sendDate = DateTime.Now,
                            remarks = "",
                            type = "",
                            Header = header,
                            createdDate = DateTime.Now
                        };
                        db.whatsapp.Add(whatsappData);
                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "WorkOrder Id Not Found"
                        };
                    }
                    var databi = db.tnx_BookingItem.Where(bi => bi.workOrderId == workOrderId).ToList();
                    foreach (var item in databi)
                    {
                        item.isWhatsApp = 1;

                    }
                    db.tnx_BookingItem.UpdateRange(databi);
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "Whatsapp Report Requested"
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

        async Task<ServiceStatusResponseModel> ItnxBookingServices.SendEmail(string workOrderId, int Userid, string EmailId, int header)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var data = (from tb in db.tnx_Booking
                                join tbi in db.tnx_BookingItem on tb.workOrderId equals tbi.workOrderId
                                where tb.workOrderId == workOrderId
                                select new
                                {
                                    PateintName = tb.name,
                                    tb.workOrderId,
                                    tb.mobileNo,
                                    tb.labRemarks
                                }).FirstOrDefault();
                    if (data != null)
                    {
                        var EmailData = new ReportEmail
                        {
                            id = 0,
                            workOrderId = data.workOrderId,
                            name = data.PateintName,
                            emailId = EmailId,
                            isSend = 0,
                            isAutoSend = 0,
                            sentBy = Userid,
                            sendDate = DateTime.Now,
                            remarks = "",
                            type = "",
                            Header = header,
                            createdDate = DateTime.Now
                        };
                        db.ReportEmail.Add(EmailData);
                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "WorkOrder Id Not Found"
                        };
                    }
                    var databi = db.tnx_BookingItem.Where(bi => bi.workOrderId == workOrderId).ToList();
                    foreach (var item in databi)
                    {
                        item.isEmailsent = 1;

                    }
                    db.tnx_BookingItem.UpdateRange(databi);
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "Email Report Requested"
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

        public byte[] CollectionReport(collectionReportRequestModel collectionData)
        {
            try
            {
                // Query for data
                var Query = from tb in db.tnx_Booking
                            join tr in db.tnx_ReceiptDetails on tb.workOrderId equals tr.workOrderId
                            join cm in db.centreMaster on tb.centreId equals cm.centreId
                            where tr.collectionDate >= collectionData.FromDate && tr.collectionDate <= collectionData.ToDate
                            select new
                            {
                                collectionDate = tr.collectionDate.Value.ToString("dd-MMM-yyyy hh:mm TT"),
                                tb.grossAmount,
                                tb.discount,
                                tb.netAmount,
                                tr.cashAmt,
                                tr.onlinewalletAmt,
                                tr.NEFTamt,
                                tr.chequeAmt,
                                tr.receivedBy,
                                tr.receivedID,
                                tb.workOrderId,
                                tb.name,
                                tb.centreId,
                                cm.centrecode,
                                cm.companyName
                            };

                // If empIds and centreIds are provided, filter the query
                if (collectionData.empIds.Count > 0)
                {
                    // Filter by empIds (uncomment if needed)
                    // Query = Query.Where(q => collectionData.empIds.Contains((int)q.receivedID));
                }
                if (collectionData.centreIds.Count > 0)
                {
                    Query = Query.Where(q => collectionData.centreIds.Contains(q.centreId));
                }

                // Get the data
                var collectiondata = Query.ToList();

                // If no data found, return an empty PDF
                if (collectiondata.Count == 0)
                {
                    return new byte[0]; // Zero-byte return when no data exists
                }

                // Log or Debug to confirm data is loaded
                Console.WriteLine($"Found {collectiondata.Count} records.");

                // QuestPDF document generation
                QuestPDF.Settings.License = LicenseType.Community;

                // Handling the company logo
                var image1 = _configuration["FileBase64:CompanyLogo1"];
                byte[] image1Bytes = null;

                if (!string.IsNullOrEmpty(image1))
                {
                    image1Bytes = Convert.FromBase64String(image1.Split(',')[1]);
                }

                // Generate the PDF document
                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(0.5f, Unit.Centimetre);
                        page.PageColor(Colors.White);

                        // Page Header
                        page.Header().Column(column =>
                        {
                            column.Item().Text("Collection Report").Style(TextStyle.Default.FontSize(16).Bold());
                        });

                        // Table Layout
                        page.Content().Table(table =>
                        {
                            // Define the columns
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(1f, Unit.Centimetre); // # column
                                columns.ConstantColumn(2f, Unit.Centimetre); // Visit ID column
                                columns.RelativeColumn();  // Patient Name
                                columns.RelativeColumn();  // Booking Date
                                columns.ConstantColumn(2f, Unit.Centimetre);  // Gross column
                                columns.ConstantColumn(2f, Unit.Centimetre);  // Discount column
                                columns.ConstantColumn(2f, Unit.Centimetre);  // Net column
                                columns.ConstantColumn(2f, Unit.Centimetre);  // Cash column
                                columns.ConstantColumn(2f, Unit.Centimetre);  // Cheque column
                                columns.ConstantColumn(2f, Unit.Centimetre);  // Wallet column
                            });

                            // Add table header
                            table.Cell().Text("#").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().Text("Visit Id").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().Text("Patient Name").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().Text("Booking Date").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().Text("Gross").Style(TextStyle.Default.FontSize(10).Bold()).AlignRight();
                            table.Cell().Text("Discount").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().Text("Net").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().Text("Cash").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().Text("Cheque").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().Text("Wallet").Style(TextStyle.Default.FontSize(10).Bold()).AlignRight();

                            // Populate table rows
                            int rowNumber = 1;
                            foreach (var item in collectiondata)
                            {
                                table.Cell().Text(rowNumber.ToString()).Style(TextStyle.Default.FontSize(10));  // Serial number
                                table.Cell().Text(item.workOrderId).Style(TextStyle.Default.FontSize(10));  // Visit ID
                                table.Cell().Text(item.name).Style(TextStyle.Default.FontSize(10));  // Patient Name
                                table.Cell().Text(item.collectionDate).Style(TextStyle.Default.FontSize(10));  // Booking Date
                                table.Cell().Text(item.grossAmount.ToString("0.00")).Style(TextStyle.Default.FontSize(10)).AlignRight();  // Gross
                                table.Cell().Text(item.discount.ToString("0.00")).Style(TextStyle.Default.FontSize(10));  // Discount
                                table.Cell().Text(item.netAmount.ToString("0.00")).Style(TextStyle.Default.FontSize(10));  // Net
                                table.Cell().Text(item.cashAmt.ToString()).Style(TextStyle.Default.FontSize(10));  // Cash
                                table.Cell().Text(item.chequeAmt.ToString()).Style(TextStyle.Default.FontSize(10));  // Cheque
                                table.Cell().Text(item.onlinewalletAmt.ToString()).Style(TextStyle.Default.FontSize(10)).AlignRight();  // Wallet
                                rowNumber++;
                            }
                        });

                        // Page Footer
                        page.Footer().Column(column =>
                        {
                            column.Item().AlignCenter().Text(text =>
                            {
                                text.DefaultTextStyle(x => x.FontSize(8));
                                text.CurrentPageNumber();
                                text.Span(" of ");
                                text.TotalPages();
                            });
                        });
                    });
                });

                // Generate the PDF byte array
                byte[] pdfBytes = document.GeneratePdf();

                // Save the PDF to file for debugging (optional)
                File.WriteAllBytes("collection_report.pdf", pdfBytes);

                return pdfBytes;  // Return the generated PDF
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                // Return empty byte array in case of error
                return new byte[0];
            }
        }


        public byte[] DiscountReport(collectionReportRequestModel collectionData)
        {
            try
            {
                // Query for data
                var Query = from tb in db.tnx_Booking
                            join tr in db.tnx_ReceiptDetails on tb.workOrderId equals tr.workOrderId
                            join cm in db.centreMaster on tb.centreId equals cm.centreId
                            where tr.collectionDate >= collectionData.FromDate && tr.collectionDate <= collectionData.ToDate
                            select new
                            {
                                collectionDate = tr.collectionDate.Value.ToString("dd-MMM-yyyy hh:mm TT"),
                                tb.grossAmount,
                                tb.discount,
                                tb.netAmount,
                                tr.receivedBy,
                                tr.receivedID,
                                tb.workOrderId,
                                tb.name,
                                tb.centreId,
                                cm.centrecode,
                                cm.companyName
                            };

                // If empIds and centreIds are provided, filter the query
                if (collectionData.empIds.Count > 0)
                {
                    // Filter by empIds (uncomment if needed)
                    // Query = Query.Where(q => collectionData.empIds.Contains((int)q.receivedID));
                }
                if (collectionData.centreIds.Count > 0)
                {
                    Query = Query.Where(q => collectionData.centreIds.Contains(q.centreId));
                }

                // Get the data
                var collectiondata = Query.ToList();

                // If no data found, return an empty PDF
                if (collectiondata.Count == 0)
                {
                    return new byte[0]; // Zero-byte return when no data exists
                }

                // Log or Debug to confirm data is loaded
                Console.WriteLine($"Found {collectiondata.Count} records.");

                // QuestPDF document generation
                QuestPDF.Settings.License = LicenseType.Community;

                // Handling the company logo
                var image1 = _configuration["FileBase64:CompanyLogo1"];
                byte[] image1Bytes = null;

                if (!string.IsNullOrEmpty(image1))
                {
                    image1Bytes = Convert.FromBase64String(image1.Split(',')[1]);
                }

                // Generate the PDF document
                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(0.5f, Unit.Centimetre);
                        page.PageColor(Colors.White);

                        // Page Header
                        page.Header().Column(column =>
                        {
                            column.Item().Text("Collection Report").Style(TextStyle.Default.FontSize(16).Bold());
                        });

                        // Table Layout
                        page.Content().Table(table =>
                        {
                            // Define the columns
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(1f, Unit.Centimetre); // # column
                                columns.ConstantColumn(2f, Unit.Centimetre); // Visit ID column
                                columns.RelativeColumn();  // Patient Name
                                columns.RelativeColumn();  // Booking Date
                                columns.ConstantColumn(2f, Unit.Centimetre);  // Gross column
                                columns.ConstantColumn(2f, Unit.Centimetre);  // Discount column
                                columns.ConstantColumn(2f, Unit.Centimetre);  // Net column
                            });

                            // Add table header
                            table.Cell().Text("#").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().Text("Visit Id").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().Text("Patient Name").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().Text("Booking Date").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().Text("Gross").Style(TextStyle.Default.FontSize(10).Bold()).AlignRight();
                            table.Cell().Text("Discount").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().Text("Net").Style(TextStyle.Default.FontSize(10).Bold());

                            // Populate table rows
                            int rowNumber = 1;
                            foreach (var item in collectiondata)
                            {
                                table.Cell().Text(rowNumber.ToString()).Style(TextStyle.Default.FontSize(10));  // Serial number
                                table.Cell().Text(item.workOrderId).Style(TextStyle.Default.FontSize(10));  // Visit ID
                                table.Cell().Text(item.name).Style(TextStyle.Default.FontSize(10));  // Patient Name
                                table.Cell().Text(item.collectionDate).Style(TextStyle.Default.FontSize(10));  // Booking Date
                                table.Cell().Text(item.grossAmount.ToString("0.00")).Style(TextStyle.Default.FontSize(10)).AlignRight();  // Gross
                                table.Cell().Text(item.discount.ToString("0.00")).Style(TextStyle.Default.FontSize(10));  // Discount
                                table.Cell().Text(item.netAmount.ToString("0.00")).Style(TextStyle.Default.FontSize(10));  // Net
                                rowNumber++;
                            }
                        });

                        // Page Footer
                        page.Footer().Column(column =>
                        {
                            column.Item().AlignCenter().Text(text =>
                            {
                                text.DefaultTextStyle(x => x.FontSize(8));
                                text.CurrentPageNumber();
                                text.Span(" of ");
                                text.TotalPages();
                            });
                        });
                    });
                });

                // Generate the PDF byte array
                byte[] pdfBytes = document.GeneratePdf();

                // Save the PDF to file for debugging (optional)
                File.WriteAllBytes("discount_report.pdf", pdfBytes);

                return pdfBytes;  // Return the generated PDF
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                // Return empty byte array in case of error
                return new byte[0];
            }
        }

        async Task<ServiceStatusResponseModel> ItnxBookingServices.WhatsappNo(string workOrderId)
        {
            try
            {
                var mobileno = (from tb in db.tnx_Booking
                                join cm in db.centreMaster on tb.centreId equals cm.centreId
                                where tb.workOrderId == workOrderId
                                select new
                                {
                                    mobileNo = cm.centretype == "2" ? cm.mobileNo : tb.mobileNo
                                }).First();
                if (mobileno == null)
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = "Mobile no Not available"
                    };
                }
                else
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = mobileno.mobileNo
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

        async Task<ServiceStatusResponseModel> ItnxBookingServices.SendEmailId(string workOrderId)
        {
            try
            {
                var EmailData = (from tb in db.tnx_Booking
                                 join tbp in db.tnx_BookingPatient on tb.patientId equals tbp.patientId
                                 join cm in db.centreMaster on tb.centreId equals cm.centreId
                                 where tb.workOrderId == workOrderId
                                 select new
                                 {
                                     EmailId = cm.centretype == "2" ? cm.reportEmail : tbp.emailId
                                 }).First();
                if (EmailData == null)
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = "Email Id Not available"
                    };
                }
                else
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = EmailData.EmailId
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

        async Task<ServiceStatusResponseModel> ItnxBookingServices.DiscountReportData(collectionReportRequestModel collectionData)
        {
            try
            {
                // Query for data
                var Query = from tb in db.tnx_Booking
                            join tr in db.tnx_ReceiptDetails on tb.workOrderId equals tr.workOrderId
                            join cm in db.centreMaster on tb.centreId equals cm.centreId
                            where tr.collectionDate >= collectionData.FromDate && tr.collectionDate <= collectionData.ToDate
                            select new
                            {
                                collectionDate = tr.collectionDate.Value.ToString("dd-MMM-yyyy hh:mm TT"),
                                tb.grossAmount,
                                tb.discount,
                                tb.netAmount,
                                tr.receivedBy,
                                tr.receivedID,
                                tb.workOrderId,
                                tb.name,
                                tb.centreId,
                                cm.centrecode,
                                cm.companyName
                            };

                // If empIds and centreIds are provided, filter the query
                if (collectionData.empIds.Count > 0)
                {
                    // Filter by empIds (uncomment if needed)
                    // Query = Query.Where(q => collectionData.empIds.Contains((int)q.receivedID));
                }
                if (collectionData.centreIds.Count > 0)
                {
                    Query = Query.Where(q => collectionData.centreIds.Contains(q.centreId));
                }

                // Get the data
                var collectiondata = await Query.ToListAsync();
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = collectiondata
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

        async Task<ServiceStatusResponseModel> ItnxBookingServices.CollectionReportData(collectionReportRequestModel collectionData)
        {
            try
            {
                // Query for data
                var Query = from tb in db.tnx_Booking
                            join tr in db.tnx_ReceiptDetails on tb.workOrderId equals tr.workOrderId
                            join cm in db.centreMaster on tb.centreId equals cm.centreId
                            where tr.collectionDate >= collectionData.FromDate && tr.collectionDate <= collectionData.ToDate
                            select new
                            {
                                collectionDate = tr.collectionDate.Value.ToString("dd-MMM-yyyy hh:mm TT"),
                                tb.grossAmount,
                                tb.discount,
                                tb.netAmount,
                                tr.cashAmt,
                                tr.onlinewalletAmt,
                                tr.NEFTamt,
                                tr.chequeAmt,
                                tr.receivedBy,
                                tr.receivedID,
                                tb.workOrderId,
                                tb.name,
                                tb.centreId,
                                cm.centrecode,
                                cm.companyName
                            };

                // If empIds and centreIds are provided, filter the query
                if (collectionData.empIds.Count > 0)
                {
                    // Filter by empIds (uncomment if needed)
                    // Query = Query.Where(q => collectionData.empIds.Contains((int)q.receivedID));
                }
                if (collectionData.centreIds.Count > 0)
                {
                    Query = Query.Where(q => collectionData.centreIds.Contains(q.centreId));
                }

                // Get the data
                var collectiondata = await Query.ToListAsync();
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = collectiondata
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

        public byte[] DiscountReportExcel(collectionReportRequestModel collectionData)
        {

            var Query = from tb in db.tnx_Booking
                        join tr in db.tnx_ReceiptDetails on tb.workOrderId equals tr.workOrderId
                        join cm in db.centreMaster on tb.centreId equals cm.centreId
                        where tr.collectionDate >= collectionData.FromDate && tr.collectionDate <= collectionData.ToDate
                        select new
                        {
                            collectionDate = tr.collectionDate.Value.ToString("dd-MMM-yyyy hh:mm TT"),
                            tb.grossAmount,
                            tb.discount,
                            tb.netAmount,
                            tr.receivedBy,
                            tr.receivedID,
                            tb.workOrderId,
                            tb.name,
                            tb.centreId,
                            cm.centrecode,
                            cm.companyName
                        };

            if (collectionData.empIds.Count > 0)
            {
                // Query = Query.Where(q => collectionData.empIds.Contains((int)q.receivedID));
            }
            if (collectionData.centreIds.Count > 0)
            {
                Query = Query.Where(q => collectionData.centreIds.Contains(q.centreId));
            }

            // Get the data
            var collectiondata = Query.ToList();
            var excelByte = MyFunction.ExportToExcel(collectiondata, "DiscountReport");
            return excelByte;
        }

        public byte[] CollectionReportExcel(collectionReportRequestModel collectionData)
        {

            var Query = from tb in db.tnx_Booking
                        join tr in db.tnx_ReceiptDetails on tb.workOrderId equals tr.workOrderId
                        join cm in db.centreMaster on tb.centreId equals cm.centreId
                        where tr.collectionDate >= collectionData.FromDate && tr.collectionDate <= collectionData.ToDate
                        select new
                        {
                            collectionDate = tr.collectionDate.Value.ToString("dd-MMM-yyyy hh:mm TT"),
                            tb.grossAmount,
                            tb.discount,
                            tb.netAmount,
                            tr.cashAmt,
                            tr.onlinewalletAmt,
                            tr.NEFTamt,
                            tr.chequeAmt,
                            tr.receivedBy,
                            tr.receivedID,
                            tb.workOrderId,
                            tb.name,
                            tb.centreId,
                            cm.centrecode,
                            cm.companyName
                        };

            // If empIds and centreIds are provided, filter the query
            if (collectionData.empIds.Count > 0)
            {
                // Filter by empIds (uncomment if needed)
                // Query = Query.Where(q => collectionData.empIds.Contains((int)q.receivedID));
            }
            if (collectionData.centreIds.Count > 0)
            {
                Query = Query.Where(q => collectionData.centreIds.Contains(q.centreId));
            }

            // Get the data
            var collectiondata = Query.ToList();
            var excelByte = MyFunction.ExportToExcel(collectiondata, "CollectionReport");
            return excelByte;
        }

        public byte[] CollectionReportSummury(collectionReportRequestModel collectiondata)
        {
            try
            {
                var Query = from tb in db.tnx_Booking
                            join em in db.empMaster on tb.createdById equals em.empId
                            join tr in db.tnx_ReceiptDetails on tb.workOrderId equals tr.workOrderId
                            join cm in db.centreMaster on tb.centreId equals cm.centreId
                            where tb.bookingDate >= collectiondata.FromDate && tb.bookingDate <= collectiondata.ToDate
                            group new { tb, tr, em } by new { em.empId, empname = string.Concat(em.fName, " ", em.lName) } into grouped
                            select new
                            {
                                EmployeeId = grouped.Key.empId,
                                empCode = grouped.Select(g => g.em.empCode).FirstOrDefault(),
                                EmployeeName = grouped.Key.empname,
                                grossAmountSum = grouped.Sum(g => g.tb.grossAmount),
                                discountSum = grouped.Sum(g => g.tb.discount),
                                netAmountSum = grouped.Sum(g => g.tb.netAmount),
                                centreId = grouped.Select(g => g.tb.centreId).FirstOrDefault(),

                                cashAmtSum = grouped.Sum(g => g.tr.cashAmt),
                                onlinewalletAmtSum = grouped.Sum(g => g.tr.onlinewalletAmt),
                                NEFTamtSum = grouped.Sum(g => g.tr.NEFTamt),
                                chequeAmtSum = grouped.Sum(g => g.tr.chequeAmt)
                            };

                if (collectiondata.empIds.Count > 0)
                {
                    // Uncomment to filter by empIds if needed
                    // Query = Query.Where(q => collectionData.empIds.Contains((int)q.receivedID));
                }
                if (collectiondata.centreIds.Count > 0)
                {
                    Query = Query.Where(q => collectiondata.centreIds.Contains(q.centreId));
                }

                // Get the data
                var collectionData = Query.ToList();
                if (collectionData.Count == 0)
                {
                    return new byte[0]; // Zero-byte return when no data exists
                }
                QuestPDF.Settings.License = LicenseType.Community;

                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(0.5f, Unit.Centimetre);
                        page.PageColor(Colors.White);

                        // Page Header
                        page.Header().Column(column =>
                        {
                            column.Item().Text("Collection Report").Style(TextStyle.Default.FontSize(14).Bold()).AlignCenter();
                        });

                        // Table Layout
                        page.Content().Table(table =>
                        {
                            // Define the columns
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(1f, Unit.Centimetre); // # column
                                columns.ConstantColumn(2f, Unit.Centimetre); // Visit ID column
                                columns.RelativeColumn();  // Patient Name
                                columns.ConstantColumn(1.5f, Unit.Centimetre);  // Booking Date
                                columns.ConstantColumn(3f, Unit.Centimetre);  // Gross column
                                columns.ConstantColumn(2f, Unit.Centimetre);  // Discount column
                                columns.ConstantColumn(1.5f, Unit.Centimetre);
                                // Gross column
                                columns.ConstantColumn(1.5f, Unit.Centimetre);  // Discount column
                                columns.ConstantColumn(1.5f, Unit.Centimetre); // Net column
                            });

                            // Add table header
                            table.Cell().BorderBottom(0.5f,Unit.Point).BorderTop(0.5f,Unit.Point).Text("#").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().BorderBottom(0.5f,Unit.Point).BorderTop(0.5f,Unit.Point).Text("EmpCode").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().BorderBottom(0.5f,Unit.Point).BorderTop(0.5f,Unit.Point).Text("Emp Name").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().BorderBottom(0.5f,Unit.Point).BorderTop(0.5f,Unit.Point).Text("Gross").Style(TextStyle.Default.FontSize(10).Bold()).AlignRight();
                            table.Cell().BorderBottom(0.5f,Unit.Point).BorderTop(0.5f,Unit.Point).Text("Discount").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().BorderBottom(0.5f,Unit.Point).BorderTop(0.5f,Unit.Point).Text("Net").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().BorderBottom(0.5f,Unit.Point).BorderTop(0.5f,Unit.Point).Text("Cash").Style(TextStyle.Default.FontSize(10).Bold()).AlignRight();
                            table.Cell().BorderBottom(0.5f,Unit.Point).BorderTop(0.5f,Unit.Point).Text("Cheque").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().BorderBottom(0.5f, Unit.Point).BorderTop(0.5f, Unit.Point).Text("OnlineWallet").Style(TextStyle.Default.FontSize(10).Bold());

                            // Populate table rows
                            int rowNumber = 1;
                            foreach (var item in collectionData)
                            {
                                table.Cell().Text(rowNumber.ToString()).Style(TextStyle.Default.FontSize(10));  // Serial number
                                table.Cell().Text(item.empCode).Style(TextStyle.Default.FontSize(10));  // Visit ID
                                table.Cell().Text(item.EmployeeName).Style(TextStyle.Default.FontSize(10));  // Patient Name
                                table.Cell().Text(item.grossAmountSum.ToString("0.00")).Style(TextStyle.Default.FontSize(10)).AlignRight();  // Gross
                                table.Cell().Text(item.discountSum.ToString("0.00")).Style(TextStyle.Default.FontSize(10));  // Discount
                                table.Cell().Text(item.netAmountSum.ToString("0.00")).Style(TextStyle.Default.FontSize(10));  // Net
                                table.Cell().Text(item.cashAmtSum.ToString()).Style(TextStyle.Default.FontSize(10)).AlignRight();  // Gross
                                table.Cell().Text(item.chequeAmtSum.ToString()).Style(TextStyle.Default.FontSize(10));  // Discount
                                table.Cell().Text(item.onlinewalletAmtSum.ToString()).Style(TextStyle.Default.FontSize(10));  // Net
                                rowNumber++;
                            }
                        });

                        // Page Footer
                        page.Footer().Column(column =>
                        {
                            column.Item().AlignCenter().Text(text =>
                            {
                                text.DefaultTextStyle(x => x.FontSize(8));
                                text.CurrentPageNumber();
                                text.Span(" of ");
                                text.TotalPages();
                            });
                        });
                    });
                });

                // Generate the PDF byte array
                byte[] pdfBytes = document.GeneratePdf();

                // Save the PDF to file for debugging (optional)
                File.WriteAllBytes("discount_report.pdf", pdfBytes);

                return pdfBytes;  // Return the generated PDF
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                // Return empty byte array in case of error
                return new byte[0];
            }
        }

        async Task<ServiceStatusResponseModel> ItnxBookingServices.CollectionReportDataSummury(collectionReportRequestModel collectiondata)
        {
            try
            {

                var Query = from tb in db.tnx_Booking
                            join em in db.empMaster on tb.createdById equals em.empId
                            join tr in db.tnx_ReceiptDetails on tb.workOrderId equals tr.workOrderId
                            join cm in db.centreMaster on tb.centreId equals cm.centreId
                            where tb.bookingDate >= collectiondata.FromDate && tb.bookingDate <= collectiondata.ToDate
                            group new { tb, tr, em } by new { em.empId, empname = string.Concat(em.fName, " ", em.lName) } into grouped
                            select new
                            {
                                EmployeeId = grouped.Key.empId,
                                empCode = grouped.Select(g => g.em.empCode).FirstOrDefault(),
                                EmployeeName = grouped.Key.empname,
                                grossAmountSum = grouped.Sum(g => g.tb.grossAmount),
                                discountSum = grouped.Sum(g => g.tb.discount),
                                netAmountSum = grouped.Sum(g => g.tb.netAmount),
                                centreId = grouped.Select(g => g.tb.centreId).FirstOrDefault(),
                                cashAmtSum = grouped.Sum(g => g.tr.cashAmt),
                                onlinewalletAmtSum = grouped.Sum(g => g.tr.onlinewalletAmt),
                                NEFTamtSum = grouped.Sum(g => g.tr.NEFTamt),
                                chequeAmtSum = grouped.Sum(g => g.tr.chequeAmt)
                            };

                if (collectiondata.empIds.Count > 0)
                {
                    // Uncomment to filter by empIds if needed
                    // Query = Query.Where(q => collectionData.empIds.Contains((int)q.receivedID));
                }
                if (collectiondata.centreIds.Count > 0)
                {
                    Query = Query.Where(q => collectiondata.centreIds.Contains(q.centreId));
                }

                // Get the data
                var collectionData = Query.ToList();
                if (collectionData != null)
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Data = collectionData
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

        public byte[] CollectionReportExcelSummury(collectionReportRequestModel collectiondata)
        {
            var Query = from tb in db.tnx_Booking
                        join em in db.empMaster on tb.createdById equals em.empId
                        join tr in db.tnx_ReceiptDetails on tb.workOrderId equals tr.workOrderId
                        join cm in db.centreMaster on tb.centreId equals cm.centreId
                        where tb.bookingDate >= collectiondata.FromDate && tb.bookingDate <= collectiondata.ToDate
                        group new { tb, tr, em } by new { em.empId,empname= string.Concat(em.fName," ",em.lName) } into grouped
                        select new
                        {
                            EmployeeId = grouped.Key.empId,
                            empCode = grouped.Select(g => g.em.empCode).FirstOrDefault(),
                            EmployeeName = grouped.Key.empname,
                            grossAmountSum = grouped.Sum(g => g.tb.grossAmount),
                            discountSum = grouped.Sum(g => g.tb.discount),
                            netAmountSum = grouped.Sum(g => g.tb.netAmount),
                            centreId = grouped.Select(g => g.tb.centreId).FirstOrDefault(),
                            
                            cashAmtSum = grouped.Sum(g => g.tr.cashAmt),
                            onlinewalletAmtSum = grouped.Sum(g => g.tr.onlinewalletAmt),
                            NEFTamtSum = grouped.Sum(g => g.tr.NEFTamt),
                            chequeAmtSum = grouped.Sum(g => g.tr.chequeAmt)
                        };

            if (collectiondata.empIds.Count > 0)
            {
                // Uncomment to filter by empIds if needed
                // Query = Query.Where(q => collectionData.empIds.Contains((int)q.receivedID));
            }
            if (collectiondata.centreIds.Count > 0)
            {
                Query = Query.Where(q => collectiondata.centreIds.Contains(q.centreId));
            }

            // Get the data
            var collectionData = Query.ToList();
            var excelByte = MyFunction.ExportToExcel(collectionData, "CollectionReport");
            return excelByte;

        }

        public byte[] DiscountReportSummury(collectionReportRequestModel collectionData)
        {
            try
            {
                var Query = from tb in db.tnx_Booking
                            join em in db.empMaster on tb.createdById equals em.empId
                            join cm in db.centreMaster on tb.centreId equals cm.centreId
                            where tb.bookingDate >= collectionData.FromDate && tb.bookingDate <= collectionData.ToDate
                            group new { tb, em, cm } by new { tb.createdById, empname = string.Concat(em.fName, " ", em.lName) } into grouped
                            select new
                            {
                                EmployeeId = grouped.Key.createdById,
                                EmployeeName = grouped.Key.empname,
                                grossAmountSum = grouped.Sum(g => g.tb.grossAmount),
                                discountSum = grouped.Sum(g => g.tb.discount),
                                netAmountSum = grouped.Sum(g => g.tb.netAmount),
                                centreId = grouped.Select(g => g.tb.centreId).FirstOrDefault(),
                                empCode = grouped.Select(g => g.em.empCode).FirstOrDefault()
                            };

                // If empIds and centreIds are provided, filter the query
                if (collectionData.empIds.Count > 0)
                {
                    // Filter by empIds (uncomment if needed)
                    // Query = Query.Where(q => collectionData.empIds.Contains((int)q.receivedID));
                }
                if (collectionData.centreIds.Count > 0)
                {
                    Query = Query.Where(q => collectionData.centreIds.Contains(q.centreId));
                }

                // Get the data
                
                var collectiondata = Query.ToList();

                // If no data found, return an empty PDF
                if (collectiondata.Count == 0)
                {
                    return new byte[0]; // Zero-byte return when no data exists
                }

                // Log or Debug to confirm data is loaded
                Console.WriteLine($"Found {collectiondata.Count} records.");

                // QuestPDF document generation
                QuestPDF.Settings.License = LicenseType.Community;

                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(0.5f, Unit.Centimetre);
                        page.PageColor(Colors.White);

                        // Page Header
                        page.Header().Column(column =>
                        {
                            column.Item().Text("Collection Report").Style(TextStyle.Default.FontSize(16).Bold());
                        });

                        // Table Layout
                        page.Content().Table(table =>
                        {
                            // Define the columns
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(1f, Unit.Centimetre); // # column
                                columns.ConstantColumn(2f, Unit.Centimetre); // Visit ID column
                                columns.RelativeColumn();  // Patient Name
                                columns.ConstantColumn(2f, Unit.Centimetre);  // Gross column
                                columns.ConstantColumn(2f, Unit.Centimetre);  // Discount column
                                columns.ConstantColumn(2f, Unit.Centimetre);  // Net column
                            });

                            // Add table header
                            table.Cell().BorderBottom(0.5f,Unit.Point).BorderTop(0.5f,Unit.Point).Text("#").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().BorderBottom(0.5f,Unit.Point).BorderTop(0.5f,Unit.Point).Text("Emp Code").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().BorderBottom(0.5f,Unit.Point).BorderTop(0.5f,Unit.Point).Text("Emp Name").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().BorderBottom(0.5f,Unit.Point).BorderTop(0.5f,Unit.Point).Text("Gross").Style(TextStyle.Default.FontSize(10).Bold()).AlignRight();
                            table.Cell().BorderBottom(0.5f,Unit.Point).BorderTop(0.5f,Unit.Point).Text("Discount").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().BorderBottom(0.5f, Unit.Point).BorderTop(0.5f, Unit.Point).Text("Net").Style(TextStyle.Default.FontSize(10).Bold());

                            // Populate table rows
                            int rowNumber = 1;
                            foreach (var item in collectiondata)
                            {
                                table.Cell().Text(rowNumber.ToString()).Style(TextStyle.Default.FontSize(10));  // Serial number
                                table.Cell().Text(item.empCode).Style(TextStyle.Default.FontSize(10));  // Visit ID
                                table.Cell().Text(item.EmployeeName).Style(TextStyle.Default.FontSize(10));  // Patient Name
                                table.Cell().Text(item.grossAmountSum.ToString("0.00")).Style(TextStyle.Default.FontSize(10)).AlignRight();  // Gross
                                table.Cell().Text(item.discountSum.ToString("0.00")).Style(TextStyle.Default.FontSize(10));  // Discount
                                table.Cell().Text(item.netAmountSum.ToString("0.00")).Style(TextStyle.Default.FontSize(10));  // Net
                                rowNumber++;
                            }
                        });

                        // Page Footer
                        page.Footer().Column(column =>
                        {
                            column.Item().AlignCenter().Text(text =>
                            {
                                text.DefaultTextStyle(x => x.FontSize(8));
                                text.CurrentPageNumber();
                                text.Span(" of ");
                                text.TotalPages();
                            });
                        });
                    });
                });

                // Generate the PDF byte array
                byte[] pdfBytes = document.GeneratePdf();

                // Save the PDF to file for debugging (optional)
                File.WriteAllBytes("discount_report.pdf", pdfBytes);

                return pdfBytes;  // Return the generated PDF
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                // Return empty byte array in case of error
                return new byte[0];
            }
        }


        public byte[] DiscountReportExcelSummury(collectionReportRequestModel collectionData)
        {


            var Query = from tb in db.tnx_Booking
                        join em in db.empMaster on tb.createdById equals em.empId
                        join cm in db.centreMaster on tb.centreId equals cm.centreId
                        where tb.bookingDate >= collectionData.FromDate && tb.bookingDate <= collectionData.ToDate
                        group new { tb, em, cm } by new { tb.createdById, empname = string.Concat(em.fName, " ", em.lName) } into grouped
                        select new
                        {
                            EmployeeId = grouped.Key.createdById,
                            EmployeeName = grouped.Key.empname,
                            grossAmountSum = grouped.Sum(g => g.tb.grossAmount),
                            discountSum = grouped.Sum(g => g.tb.discount),
                            netAmountSum = grouped.Sum(g => g.tb.netAmount),
                            centreId = grouped.Select(g => g.tb.centreId).FirstOrDefault(),
                            empCode = grouped.Select(g => g.em.empCode).FirstOrDefault()
                        };

            // If empIds and centreIds are provided, filter the query
            if (collectionData.empIds.Count > 0)
            {
                // Filter by empIds (uncomment if needed)
                // Query = Query.Where(q => collectionData.empIds.Contains((int)q.receivedID));
            }
            if (collectionData.centreIds.Count > 0)
            {
                Query = Query.Where(q => collectionData.centreIds.Contains(q.centreId));
            }

            // Get the data
            var collectiondata = Query.ToList();
            var excelByte = MyFunction.ExportToExcel(collectiondata, "DiscountReport");
            return excelByte;
        }

        async Task<ServiceStatusResponseModel> ItnxBookingServices.DiscountReportDataSummury(collectionReportRequestModel collectionData)
        {
            try
            {

                var Query = from tb in db.tnx_Booking
                            join em in db.empMaster on tb.createdById equals em.empId
                            join cm in db.centreMaster on tb.centreId equals cm.centreId
                            where tb.bookingDate >= collectionData.FromDate && tb.bookingDate <= collectionData.ToDate
                            group new { tb, em, cm } by new { tb.createdById, empname=string.Concat(em.fName," ",em.lName) } into grouped
                            select new
                            {
                                EmployeeId = grouped.Key.createdById,
                                EmployeeName = grouped.Key.empname,
                                grossAmountSum = grouped.Sum(g => g.tb.grossAmount),
                                discountSum = grouped.Sum(g => g.tb.discount),
                                netAmountSum = grouped.Sum(g => g.tb.netAmount),
                                centreId= grouped.Select(g=> g.tb.centreId).FirstOrDefault(),
                                empCode = grouped.Select(g => g.em.empCode).FirstOrDefault()
                            };

                // If empIds and centreIds are provided, filter the query
                if (collectionData.empIds.Count > 0)
                {
                    // Filter by empIds (uncomment if needed)
                    // Query = Query.Where(q => collectionData.empIds.Contains((int)q.receivedID));
                }
                if (collectionData.centreIds.Count > 0)
                {
                    Query = Query.Where(q => collectionData.centreIds.Contains(q.centreId));
                }

                // Get the data
                var collectiondata = await Query.ToListAsync();
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = collectiondata
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

        async Task<ServiceStatusResponseModel> ItnxBookingServices.DiscountAfterBill(DicountAfterBillRequestModel DiscountData)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var bookingData = db.tnx_Booking.Where(b => b.workOrderId == DiscountData.workOrderId).FirstOrDefault();
                    var discPer = 0.00;
                    if(bookingData!=null)
                    {
                        discPer = DiscountData.discountAmt * 100 / bookingData.grossAmount;
                        bookingData.discount = DiscountData.discountAmt;
                        bookingData.netAmount = bookingData.grossAmount - DiscountData.discountAmt;
                        
                        db.tnx_Booking.Update(bookingData);
                        await db.SaveChangesAsync();
                        var bookingitem = db.tnx_BookingItem.Where(bi => bi.workOrderId == DiscountData.workOrderId).ToList();
                        foreach (var item in bookingitem)
                        {
                            item.discount = item.rate * discPer;
                            item.netAmount= item.rate- (item.rate*discPer);
                        }
                        db.tnx_BookingItem.UpdateRange(bookingitem);
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
                            Message = "Patient Not Found"
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

        async Task<ServiceStatusResponseModel> ItnxBookingServices.patientDataDiscount(string workorderId)
        {
            try
            {
                var bookingData = await(from tb in db.tnx_Booking
                                        join tm in db.titleMaster on tb.title_id equals tm.id
                                        join cm in db.centreMaster on tb.centreId equals cm.centreId
                                        join dr in db.doctorReferalMaster on tb.refID1 equals dr.doctorId
                                        where tb.workOrderId == workorderId
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
                                        }).ToListAsync();

                if (bookingData != null)
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

        async Task<ServiceStatusResponseModel> ItnxBookingServices.TestRefund(testRefundModel RefundData)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var bookingitem= db.tnx_BookingItem.Where(bi=> RefundData.testIds.Contains(bi.id)).ToList();
                    foreach (var item in bookingitem)
                    {
                        item.isRemoveItem = 1;
                        item.refundBy = RefundData.refundBy;
                        item.refundReason = RefundData.refundReason;
                        item.RefundDate = DateTime.Now;
                    }
                    db.tnx_BookingItem.UpdateRange(bookingitem);
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "refund Successful"
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

         byte[] ItnxBookingServices.ClientRevenueReport(collectionReportRequestModel collectionData)
        {
            try
            {
                // Query for data
                var Query = from tb in db.tnx_Booking
                            join tr in db.tnx_ReceiptDetails on tb.workOrderId equals tr.workOrderId
                            join cm in db.centreMaster on tb.centreId equals cm.centreId
                            where tr.collectionDate >= collectionData.FromDate && tr.collectionDate <= collectionData.ToDate
                            select new
                            {
                                collectionDate = tr.collectionDate.Value.ToString("dd-MMM-yyyy hh:mm tt"),
                                tb.grossAmount,
                                tb.discount,
                                tb.netAmount,
                                tr.cashAmt,
                                tr.onlinewalletAmt,
                                tr.NEFTamt,
                                tr.chequeAmt,
                                tr.receivedBy,
                                tr.receivedID,
                                tb.workOrderId,
                                tb.name,
                                tb.centreId,
                                cm.centrecode,
                                cm.companyName
                            };

                if (collectionData.centreIds.Count > 0)
                {
                    Query = Query.Where(q => collectionData.centreIds.Contains(q.centreId));
                }

                // Get the data
                var collectiondata = Query.ToList();

                // If no data found, return an empty PDF
                if (collectiondata.Count == 0)
                {
                    return new byte[0]; // Zero-byte return when no data exists
                }

                // Log or Debug to confirm data is loaded
                Console.WriteLine($"Found {collectiondata.Count} records.");

                // QuestPDF document generation
                QuestPDF.Settings.License = LicenseType.Community;

                // Handling the company logo
                var image1 = _configuration["FileBase64:CompanyLogo1"];
                byte[] image1Bytes = null;

                if (!string.IsNullOrEmpty(image1))
                {
                    image1Bytes = Convert.FromBase64String(image1.Split(',')[1]);
                }

                // Generate the PDF document
                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(0.5f, Unit.Centimetre);
                        page.PageColor(Colors.White);

                        // Page Header
                        page.Header().Column(column =>
                        {
                            column.Item().Text("Collection Report").Style(TextStyle.Default.FontSize(16).Bold());
                        });

                        // Table Layout
                        page.Content().Table(table =>
                        {
                            // Define the columns
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(1f, Unit.Centimetre); // # column
                                columns.ConstantColumn(2f, Unit.Centimetre); // Visit ID column
                                columns.RelativeColumn();  // Patient Name
                                columns.RelativeColumn();  // Booking Date
                                columns.ConstantColumn(2f, Unit.Centimetre);  // Gross column
                                columns.ConstantColumn(2f, Unit.Centimetre);  // Discount column
                                columns.ConstantColumn(2f, Unit.Centimetre);  // Net column
                                columns.ConstantColumn(1.5f, Unit.Centimetre);  // Cash column
                                columns.ConstantColumn(1.5f, Unit.Centimetre);  // Cheque column
                                columns.ConstantColumn(1.5f, Unit.Centimetre);  // Wallet column
                            });

                            // Add table header
                            table.Cell().Text("#").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().Text("Visit Id").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().Text("Patient Name").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().Text("Booking Date").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().Text("Gross").Style(TextStyle.Default.FontSize(10).Bold()).AlignRight();
                            table.Cell().Text("Discount").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().Text("Net").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().Text("Cash").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().Text("Cheque").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().Text("Wallet").Style(TextStyle.Default.FontSize(10).Bold()).AlignRight();

                            // Populate table rows
                            int rowNumber = 1;
                            int centreid = 0;
                            foreach (var item in collectiondata)
                            {
                                if(centreid!= item.centreId)
                                {
                                    table.Cell().ColumnSpan(10).Border(0.5f, Unit.Point).Text(item.companyName).Style(TextStyle.Default.FontSize(10)).AlignCenter();  // Serial number
                                    centreid = item.centreId;
                                }
                                table.Cell().Text(rowNumber.ToString()).Style(TextStyle.Default.FontSize(10));  // Serial number
                                table.Cell().Text(item.workOrderId).Style(TextStyle.Default.FontSize(10));  // Visit ID
                                table.Cell().Text(item.name).Style(TextStyle.Default.FontSize(10));  // Patient Name
                                table.Cell().Text(item.collectionDate).Style(TextStyle.Default.FontSize(10));  // Booking Date
                                table.Cell().Text(item.grossAmount.ToString("0.00")).Style(TextStyle.Default.FontSize(10)).AlignRight();  // Gross
                                table.Cell().Text(item.discount.ToString("0.00")).Style(TextStyle.Default.FontSize(10));  // Discount
                                table.Cell().Text(item.netAmount.ToString("0.00")).Style(TextStyle.Default.FontSize(10));  // Net
                                table.Cell().Text(item.cashAmt.ToString()).Style(TextStyle.Default.FontSize(10));  // Cash
                                table.Cell().Text(item.chequeAmt.ToString()).Style(TextStyle.Default.FontSize(10));  // Cheque
                                table.Cell().Text(item.onlinewalletAmt.ToString()).Style(TextStyle.Default.FontSize(10)).AlignRight();  // Wallet
                                rowNumber++;
                            }
                        });

                        // Page Footer
                        page.Footer().Column(column =>
                        {
                            column.Item().AlignCenter().Text(text =>
                            {
                                text.DefaultTextStyle(x => x.FontSize(8));
                                text.CurrentPageNumber();
                                text.Span(" of ");
                                text.TotalPages();
                            });
                        });
                    });
                });

                // Generate the PDF byte array
                byte[] pdfBytes = document.GeneratePdf();

                // Save the PDF to file for debugging (optional)
                File.WriteAllBytes("collection_report.pdf", pdfBytes);

                return pdfBytes;  // Return the generated PDF
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                // Return empty byte array in case of error
                return new byte[0];
            }
        }

        async Task<ServiceStatusResponseModel> ItnxBookingServices.ClientRevenueReportData(collectionReportRequestModel collectionData)
        {
            try
            {
                // Query for data
                var Query = from tb in db.tnx_Booking
                            join tr in db.tnx_ReceiptDetails on tb.workOrderId equals tr.workOrderId
                            join cm in db.centreMaster on tb.centreId equals cm.centreId
                            where tr.collectionDate >= collectionData.FromDate && tr.collectionDate <= collectionData.ToDate
                            select new
                            {
                                collectionDate = tr.collectionDate.Value.ToString("dd-MMM-yyyy hh:mm TT"),
                                tb.grossAmount,
                                tb.discount,
                                tb.netAmount,
                                tr.cashAmt,
                                tr.onlinewalletAmt,
                                tr.NEFTamt,
                                tr.chequeAmt,
                                tr.receivedBy,
                                tr.receivedID,
                                tb.workOrderId,
                                tb.name,
                                tb.centreId,
                                cm.centrecode,
                                cm.companyName
                            };
                if (collectionData.centreIds.Count > 0)
                {
                    Query = Query.Where(q => collectionData.centreIds.Contains(q.centreId));
                }

                // Get the data
                var collectiondata = await Query.ToListAsync();
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = collectiondata
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

        public byte[] ClientRevenueReportExcel(collectionReportRequestModel collectionData)
        {
            
                var Query = from tb in db.tnx_Booking
                            join tr in db.tnx_ReceiptDetails on tb.workOrderId equals tr.workOrderId
                            join cm in db.centreMaster on tb.centreId equals cm.centreId
                            where tr.collectionDate >= collectionData.FromDate && tr.collectionDate <= collectionData.ToDate
                            select new
                            {
                                collectionDate = tr.collectionDate.Value.ToString("dd-MMM-yyyy hh:mm TT"),
                                tb.grossAmount,
                                tb.discount,
                                tb.netAmount,
                                tr.cashAmt,
                                tr.onlinewalletAmt,
                                tr.NEFTamt,
                                tr.chequeAmt,
                                tr.receivedBy,
                                tr.receivedID,
                                tb.workOrderId,
                                tb.name,
                                tb.centreId,
                                cm.centrecode,
                                cm.companyName
                            };
                if (collectionData.centreIds.Count > 0)
                {
                    Query = Query.Where(q => collectionData.centreIds.Contains(q.centreId));
                }

                // Get the data
                var collectiondata =  Query.ToList();
                var excelByte = MyFunction.ExportToExcel(collectiondata, "CollectionReport");
                return excelByte;
           
        }

        public byte[] ClientRevenueReportSummury(collectionReportRequestModel collectionData)
        {
            try
            {
                // Query for data
                var Query = from tb in db.tnx_Booking
                            join tr in db.tnx_ReceiptDetails on tb.workOrderId equals tr.workOrderId
                            join cm in db.centreMaster on tb.centreId equals cm.centreId
                            where tr.collectionDate >= collectionData.FromDate && tr.collectionDate <= collectionData.ToDate
                            group new { tb, tr, cm } by new { tb.centreId, cm.centrecode, cm.companyName } into g
                            select new
                            {
                                CentreId = g.Key.centreId,
                                CentreCode = g.Key.centrecode,
                                CompanyName = g.Key.companyName,
                                TotalGrossAmount = g.Sum(x => x.tb.grossAmount),
                                TotalDiscount = g.Sum(x => x.tb.discount),
                                TotalNetAmount = g.Sum(x => x.tb.netAmount),
                                TotalCashAmount = g.Sum(x => x.tr.cashAmt),
                                TotalOnlineWalletAmount = g.Sum(x => x.tr.onlinewalletAmt),
                                TotalNEFTAmount = g.Sum(x => x.tr.NEFTamt),
                                TotalChequeAmount = g.Sum(x => x.tr.chequeAmt),
                                TotalTransactions = g.Count()
                            };

                if (collectionData.centreIds.Count > 0)
                {
                    Query = Query.Where(q => collectionData.centreIds.Contains(q.CentreId));
                }

                // Get the summarized data
                var collectiondata = Query.ToList();

                // If no data found, return an empty PDF
                if (collectiondata.Count == 0)
                {
                    return new byte[0]; // Zero-byte return when no data exists
                }

                // Log or Debug to confirm data is loaded
                Console.WriteLine($"Found {collectiondata.Count} records.");

                // QuestPDF document generation
                QuestPDF.Settings.License = LicenseType.Community;

                // Handling the company logo
                var image1 = _configuration["FileBase64:CompanyLogo1"];
                byte[] image1Bytes = null;

                if (!string.IsNullOrEmpty(image1))
                {
                    image1Bytes = Convert.FromBase64String(image1.Split(',')[1]);
                }

                // Generate the PDF document
                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(0.5f, Unit.Centimetre);
                        page.PageColor(Colors.White);

                        // Page Header
                        page.Header().Column(column =>
                        {
                            column.Item().Text("Collection Report").Style(TextStyle.Default.FontSize(16).Bold());
                        });

                        // Table Layout
                        page.Content().Table(table =>
                        {
                            // Define the columns
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(1f, Unit.Centimetre); // # column
                                columns.ConstantColumn(2f, Unit.Centimetre); // Visit ID column
                                columns.RelativeColumn();  // Patient Name
                                columns.RelativeColumn();  // Booking Date
                                columns.ConstantColumn(2f, Unit.Centimetre);  // Gross column
                                columns.ConstantColumn(2f, Unit.Centimetre);  // Discount column
                                columns.ConstantColumn(2f, Unit.Centimetre);  // Net column
                                columns.ConstantColumn(2f, Unit.Centimetre);  // Cash column
                                columns.ConstantColumn(2f, Unit.Centimetre);  // Cheque column
                                columns.ConstantColumn(2f, Unit.Centimetre);  // Wallet column
                            });

                            // Add table header
                            table.Cell().Text("#").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().Text("CentreCode").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().Text("CentreName").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().Text("Gross").Style(TextStyle.Default.FontSize(10).Bold()).AlignRight();
                            table.Cell().Text("Discount").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().Text("Net").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().Text("Cash").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().Text("Cheque").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().Text("Wallet").Style(TextStyle.Default.FontSize(10).Bold()).AlignRight();

                            // Populate table rows
                            int rowNumber = 1;
                            int centreid = 0;
                            foreach (var item in collectiondata)
                            {
                                
                                table.Cell().Text(rowNumber.ToString()).Style(TextStyle.Default.FontSize(10));  // Serial number
                                table.Cell().Text(item.CentreCode).Style(TextStyle.Default.FontSize(10));  // Visit ID
                                table.Cell().Text(item.CompanyName).Style(TextStyle.Default.FontSize(10));  // Patient Name
                                table.Cell().Text(item.TotalGrossAmount.ToString("0.00")).Style(TextStyle.Default.FontSize(10)).AlignRight();  // Gross
                                table.Cell().Text(item.TotalDiscount.ToString("0.00")).Style(TextStyle.Default.FontSize(10));  // Discount
                                table.Cell().Text(item.TotalNetAmount.ToString("0.00")).Style(TextStyle.Default.FontSize(10));  // Net
                                table.Cell().Text(item.TotalCashAmount.ToString()).Style(TextStyle.Default.FontSize(10));  // Cash
                                table.Cell().Text(item.TotalChequeAmount.ToString()).Style(TextStyle.Default.FontSize(10));  // Cheque
                                table.Cell().Text(item.TotalOnlineWalletAmount.ToString()).Style(TextStyle.Default.FontSize(10)).AlignRight();  // Wallet
                                rowNumber++;
                            }
                        });

                        // Page Footer
                        page.Footer().Column(column =>
                        {
                            column.Item().AlignCenter().Text(text =>
                            {
                                text.DefaultTextStyle(x => x.FontSize(8));
                                text.CurrentPageNumber();
                                text.Span(" of ");
                                text.TotalPages();
                            });
                        });
                    });
                });

                // Generate the PDF byte array
                byte[] pdfBytes = document.GeneratePdf();

                // Save the PDF to file for debugging (optional)
                File.WriteAllBytes("collection_report.pdf", pdfBytes);

                return pdfBytes;  // Return the generated PDF
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                // Return empty byte array in case of error
                return new byte[0];
            }
        }

        async Task<ServiceStatusResponseModel> ItnxBookingServices.ClientRevenueReportDataSummury(collectionReportRequestModel collectionData)
        {
            try
            {
                var Query = from tb in db.tnx_Booking
                            join tr in db.tnx_ReceiptDetails on tb.workOrderId equals tr.workOrderId
                            join cm in db.centreMaster on tb.centreId equals cm.centreId
                            where tr.collectionDate >= collectionData.FromDate && tr.collectionDate <= collectionData.ToDate
                            group new { tb, tr, cm } by new { tb.centreId, cm.centrecode, cm.companyName } into g
                            select new
                            {
                                CentreId = g.Key.centreId,
                                CentreCode = g.Key.centrecode,
                                CompanyName = g.Key.companyName,
                                TotalGrossAmount = g.Sum(x => x.tb.grossAmount),
                                TotalDiscount = g.Sum(x => x.tb.discount),
                                TotalNetAmount = g.Sum(x => x.tb.netAmount),
                                TotalCashAmount = g.Sum(x => x.tr.cashAmt),
                                TotalOnlineWalletAmount = g.Sum(x => x.tr.onlinewalletAmt),
                                TotalNEFTAmount = g.Sum(x => x.tr.NEFTamt),
                                TotalChequeAmount = g.Sum(x => x.tr.chequeAmt),
                                TotalTransactions = g.Count()
                            };

                if (collectionData.centreIds.Count > 0)
                {
                    Query = Query.Where(q => collectionData.centreIds.Contains(q.CentreId));
                }

                // Get the summarized data
                var summaryData = await Query.ToListAsync();
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = summaryData
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

        public byte[] ClientRevenueReportExcelSummury(collectionReportRequestModel collectionData)
        {
            var Query = from tb in db.tnx_Booking
                        join tr in db.tnx_ReceiptDetails on tb.workOrderId equals tr.workOrderId
                        join cm in db.centreMaster on tb.centreId equals cm.centreId
                        where tr.collectionDate >= collectionData.FromDate && tr.collectionDate <= collectionData.ToDate
                        group new { tb, tr, cm } by new { tb.centreId, cm.centrecode, cm.companyName } into g
                        select new
                        {
                            CentreId = g.Key.centreId,
                            CentreCode = g.Key.centrecode,
                            CompanyName = g.Key.companyName,
                            TotalGrossAmount = g.Sum(x => x.tb.grossAmount),
                            TotalDiscount = g.Sum(x => x.tb.discount),
                            TotalNetAmount = g.Sum(x => x.tb.netAmount),
                            TotalCashAmount = g.Sum(x => x.tr.cashAmt),
                            TotalOnlineWalletAmount = g.Sum(x => x.tr.onlinewalletAmt),
                            TotalNEFTAmount = g.Sum(x => x.tr.NEFTamt),
                            TotalChequeAmount = g.Sum(x => x.tr.chequeAmt),
                            TotalTransactions = g.Count()
                        };

            if (collectionData.centreIds.Count > 0)
            {
                Query = Query.Where(q => collectionData.centreIds.Contains(q.CentreId));
            }

            // Get the summarized data
            var summaryData = Query.ToList();
            var excelByte = MyFunction.ExportToExcel(summaryData, "CollectionSummaryReport");
            return excelByte;
        }

        async Task<ServiceStatusResponseModel> ItnxBookingServices.GetPendingPayment(string workOrderId)
        {
            try
            {
                var pendingPay = (from tb in db.tnx_Booking
                                  where tb.workOrderId == workOrderId
                                  select new
                                  {
                                      DueAmt = tb.grossAmount - tb.paidAmount
                                  }).FirstOrDefault();

                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = pendingPay
                };
            }
            catch(Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message= ex.Message
                };
            }
        }
    }
}

