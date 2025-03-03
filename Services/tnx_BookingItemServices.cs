using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Services
{
    public class tnx_BookingItemServices : Itnx_BookingItemServices
    {
        private readonly ContextClass db;
        private readonly MySql_Procedure_Services _MySql_Procedure_Services;
        public tnx_BookingItemServices(ContextClass context, ILogger<BaseController<tnx_BookingPatient>> logger, MySql_Procedure_Services MySql_Procedure_Services)
        {

            db = context;
            this._MySql_Procedure_Services = MySql_Procedure_Services;
        }

        async Task<ServiceStatusResponseModel> Itnx_BookingItemServices.GetSampleProcessingData(SampleProcessingRequestModel sampleProcessingRequestModel)
        {
            var query = from tb in db.tnx_Booking
                        join tbi in db.tnx_BookingItem on tb.transactionId equals tbi.transactionId
                        join im in db.itemMaster on tbi.itemId equals im.itemId
                        join cm in db.centreMaster on tb.centreId equals cm.centreId
                        join tm in db.titleMaster on tb.title_id equals tm.id

                        where tbi.itemType != 3
                        select new SampleProcessingResponseModel
                        {
                            centreId = tbi.centreId,
                            centreName = cm.centrecode,
                            deptId = tbi.deptId,
                            departmentName = tbi.departmentName,
                            patientId = tb.patientId,
                            workOrderId = tb.workOrderId,
                            bookingdate = tb.bookingDate.ToString("yyyy-MMM-dd hh:mm tt"),
                            PatientName = string.Concat(tm.title, " ", tb.name),
                            Age = string.Concat(tb.ageYear, " Y ", tb.ageMonth, " M ", tb.ageDay, " D"),
                            itemId = tbi.itemId,
                            investigationName = tbi.isPackage == 1 ? string.Concat(tbi.packageName, " ( ", tbi.investigationName, " )") : tbi.investigationName,
                            barcodeNo = tbi.barcodeNo ?? "",
                            sampleTypeId = tbi.sampleTypeId,
                            sampleTypeName = tbi.sampleTypeName,
                            sampletypedata = db.itemSampleTypeMapping.Where(s => s.itemId == tbi.itemId).Select(s => new { s.sampleTypeId, s.sampleTypeName }).ToList(),
                            isSampleCollected = tbi.isSampleCollected,
                            Comment = tb.labRemarks,
                            samplecollectiondate = tbi.sampleCollectionDate.HasValue ? tbi.sampleCollectionDate.Value.ToString("yyyy-MMM-dd hh:mm tt") : "",
                            SampleRecievedDate = tbi.sampleReceiveDate.HasValue ? tbi.sampleReceiveDate.Value.ToString("yyyy-MMM-dd hh:mm tt") : "",
                            transactionId = tb.transactionId,
                            createdDateTime = tbi.createdDateTime,
                            Urgent = tbi.isUrgent,
                            containercolor = db.containerColorMaster.Where(c => c.id == Convert.ToInt32(im.containerColor)).Select(c => c.ColorCode).FirstOrDefault(),
                            isremark = db.tnx_InvestigationRemarks.Where(s => s.itemId == tbi.itemId && s.transactionId == tbi.transactionId).Count(),
                            rowcolor = tbi.isUrgent == 1 ? "#ff8960" : (tbi.isUrgent == 0 && tbi.isSampleCollected == "N" ? "#D7CDC6" : (tbi.isUrgent == 0 && tbi.isSampleCollected == "S" ? "#8fb4ff" : (tbi.isUrgent == 0 && tbi.isSampleCollected == "Y" ? "#75AADB" : "#FF6865"))),
                            RejectionReason = tbi.RejectedReason,
                            resultdone= (int)tbi.isResultDone
                        };
            // Apply date filter
            if (sampleProcessingRequestModel.fromDate.HasValue && sampleProcessingRequestModel.toDate.HasValue)
            {
                var fromDate = sampleProcessingRequestModel.fromDate.Value.Date; // Strip the time portion
                var toDate = sampleProcessingRequestModel.toDate.Value.Date.AddHours(24).AddSeconds(-1); // Include the entire end date

                query = query.Where(q => q.createdDateTime >= fromDate && q.createdDateTime <= toDate);
            }
            if (!string.IsNullOrEmpty(sampleProcessingRequestModel.barcodeNo))
            {
                query = query.Where(q => q.barcodeNo == sampleProcessingRequestModel.barcodeNo);
            }
            if (sampleProcessingRequestModel.centreId != 0)
            {
                query = query.Where(q => q.centreId == sampleProcessingRequestModel.centreId);
            }
            else
            {
                List<int> CentreIds = db.empCenterAccess.Where(e => e.empId == sampleProcessingRequestModel.empId).Select(e => e.centreId).ToList();
                query = query.Where(q => CentreIds.Contains(q.centreId));
            }
            if (sampleProcessingRequestModel.Status != "U")
            {
                query = query.Where(q => q.isSampleCollected == sampleProcessingRequestModel.Status);
            }
            else
            {
                query = query.Where(q => q.Urgent == 1);
            }
            query = query.OrderBy(q => q.patientId).ThenBy(q => q.transactionId);
            var result = await query.ToListAsync();
            return new ServiceStatusResponseModel
            {
                Success = true,
                Data = result
            };
        }

        async Task<ServiceStatusResponseModel> Itnx_BookingItemServices.GetTestObservations(ResultEntryRequestModle resultEntryRequestModle)
        {

            var result = await _MySql_Procedure_Services.GetTestObservation(resultEntryRequestModle.testId, resultEntryRequestModle.gender, resultEntryRequestModle.fromAge, resultEntryRequestModle.toAge, resultEntryRequestModle.centreId);
            return result;
        }

        async Task<ServiceStatusResponseModel> Itnx_BookingItemServices.UpdateSampleStatus(List<SampleProcessingResponseModel> sampleProcessingResponseModels)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    foreach (var SampleResponseModel in sampleProcessingResponseModels)
                    {
                        var sampleStatus = db.tnx_BookingItem
                            .Where(bi => bi.itemId == SampleResponseModel.itemId && bi.transactionId == SampleResponseModel.transactionId)
                            .FirstOrDefault();

                        if (sampleStatus != null)
                        {
                            UpdateSampleStatus(sampleStatus, SampleResponseModel);
                            db.tnx_BookingItem.Update(sampleStatus);
                            await db.SaveChangesAsync();
                        }
                        else
                        {
                            return new ServiceStatusResponseModel
                            {
                                Success = false,
                                Message = "Sample not found for itemId: " + SampleResponseModel.itemId
                            };
                        }
                    }
                    await transaction.CommitAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "Records saved successfully"
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = ex.InnerException?.Message ?? "An error occurred."
                    };
                }
            }
        }
        private void UpdateSampleStatus(tnx_BookingItem sampleStatus, SampleProcessingResponseModel sampleProcessingResponseModel)
        {


            switch (sampleProcessingResponseModel.isSampleCollected)
            {
                case "S":
                    sampleStatus.isSampleCollected = "S";
                    sampleStatus.sampleCollectedID = sampleProcessingResponseModel.empId;
                    sampleStatus.sampleCollectionDate = DateTime.Now;
                    break;

                case "Y":
                    sampleStatus.isSampleCollected = "Y";
                    sampleStatus.sampleReceivedBY = Convert.ToString(sampleProcessingResponseModel.empId);
                    sampleStatus.sampleReceiveDate = DateTime.Now;
                    break;

                case "R":
                    sampleStatus.isSampleCollected = "R";
                    sampleStatus.sampleRejectionBy = sampleProcessingResponseModel.empId;
                    sampleStatus.sampleRejectionOn = DateTime.Now;
                    sampleStatus.RejectedReason = sampleProcessingResponseModel.RejectionReason;
                    break;

                default:
                    // Optionally, handle invalid values for isSampleCollected here
                    break;
            }
        }


        async Task<ServiceStatusResponseModel> Itnx_BookingItemServices.SaveTestObservations(List<ResultSaveRequestModle> resultSaveRequestModle)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var distinctTestIds = resultSaveRequestModle.Where(x => x.testId.HasValue)
                                                                  .GroupBy(x => x.testId).Select(g => g.First()).ToList();
                    var TestIds = resultSaveRequestModle.Where(x => x.testId.HasValue).Select(x => x.testId.Value).Distinct().ToList();
                    var oldresultdata = await db.tnx_Observations.Where(b => TestIds.Contains((int)b.testId)).ToListAsync();
                    var NumericResultLog = oldresultdata.Select(CreateResultentrylog).ToList();
                    db.tnx_Observations_Log.AddRange(NumericResultLog);
                    await db.SaveChangesAsync();
                    db.tnx_Observations.RemoveRange(oldresultdata);
                    await db.SaveChangesAsync();

                    var testObservations = resultSaveRequestModle.Select(CreateObservationData).ToList();
                    db.tnx_Observations.AddRange(testObservations);
                    await db.SaveChangesAsync();

                    foreach (var testids in distinctTestIds)
                    {
                        var TestId = int.Parse(testids.testId.ToString());
                        var sampleStatus = await db.tnx_BookingItem
                            .Where(bi => bi.id == TestId).SingleOrDefaultAsync();
                        var TestStatus = "";
                        if (sampleStatus != null)
                        {
                            if (testids.isApproved.ToString() == "1")
                            {
                                TestStatus = "Result Save and Approved";
                                sampleStatus.isResultDone = 1;
                                sampleStatus.isApproved = 1;
                                sampleStatus.resultDoneByID = testids.createdById;
                                sampleStatus.approvedByID = testids.createdById;
                                sampleStatus.resutDoneBy = testids.createdBy;
                                sampleStatus.approvedbyName = testids.createdBy;
                            }
                            else
                            {
                                TestStatus = "Result Saved";
                                sampleStatus.isResultDone = 1;
                                sampleStatus.resultDoneByID = testids.createdById;
                                sampleStatus.resutDoneBy = testids.createdBy;
                            }
                            db.tnx_BookingItem.Update(sampleStatus);
                            await db.SaveChangesAsync();
                            tnx_BookingStatus bookingStatus = new tnx_BookingStatus
                            {
                                id = 0,
                                transactionId = testids.transactionId,
                                patientId = testids.patientId,
                                barcodeNo = testids.barcodeNo,
                                status = TestStatus,
                                testId = TestId,
                                createdById = testids.createdById,
                                createdDateTime = testids.createdDate
                            };
                            db.tnx_BookingStatus.Add(bookingStatus);
                            await db.SaveChangesAsync();
                        }
                    }

                    await transaction.CommitAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "All records saved successfully"
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = ex.InnerException?.Message ?? "An error occurred."
                    };
                }
            }
        }

        private tnx_Observations CreateObservationData(ResultSaveRequestModle resultSaveRequestModel)
        {
            return new tnx_Observations
            {
                id = 0,
                testId = resultSaveRequestModel.testId,
                labObservationId = resultSaveRequestModel.labObservationId,
                observationName = resultSaveRequestModel.observationName,
                value = resultSaveRequestModel.value,
                unit = resultSaveRequestModel.unit,
                readingformat = resultSaveRequestModel.readingFormat,
                displayReading = resultSaveRequestModel.displayReading,
                min = resultSaveRequestModel.minVal,
                max = resultSaveRequestModel.maxVal,
                minCritical = resultSaveRequestModel.minCritical,
                maxCritical = resultSaveRequestModel.maxCritical,
                flag = resultSaveRequestModel.flag,
                testMethod = resultSaveRequestModel.method,
                isCritical = resultSaveRequestModel.isCritical,
                machineReading = resultSaveRequestModel.machineReading,
                printseperate = resultSaveRequestModel.printSeperate,
                isBold = resultSaveRequestModel.isBold,
                machineID = resultSaveRequestModel.machineID,
                machineName = resultSaveRequestModel.machineName,
                showInReport = resultSaveRequestModel.showInReport
            };
        }
        private tnx_Observations_Log CreateResultentrylog(tnx_Observations oldresultdata)
        {
            return new tnx_Observations_Log
            {
                id = 0,
                testId = oldresultdata.testId,
                labObservationId = oldresultdata.labObservationId,
                observationName = oldresultdata.observationName,
                value = oldresultdata.value,
                flag = oldresultdata.flag,
                min = oldresultdata.min,
                max = oldresultdata.max,
                minCritical = oldresultdata.minCritical,
                maxCritical = oldresultdata.maxCritical,
                isCritical = oldresultdata.isCritical,
                readingformat = oldresultdata.readingformat,
                unit = oldresultdata.unit,
                testMethod = oldresultdata.testMethod,
                displayReading = oldresultdata.displayReading,
                machineReading = oldresultdata.machineReading,
                machineID = oldresultdata.machineID,
                printseperate = oldresultdata.printseperate,
                isBold = oldresultdata.isBold,
                machineName = oldresultdata.machineName,
                dtEntry = oldresultdata.createdDateTime,
                isActive = oldresultdata.isActive,
                createdById = oldresultdata.createdById,
                createdDateTime = oldresultdata.createdDateTime,
                updateById = oldresultdata.createdById,
                updateDateTime = DateTime.Now,

            };
        }

        async Task<ServiceStatusResponseModel> Itnx_BookingItemServices.SaveHistoResult(HistoResultSaveRequestModle histoResultSaveRequestModle)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    if (histoResultSaveRequestModle.histoObservationId == 0)
                    {
                        var HistoResultsave = CreateHistoResult(histoResultSaveRequestModle);
                        db.tnx_Observations_Histo.Add(HistoResultsave);
                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        var historesult = await db.tnx_Observations_Histo.Where(b => b.histoObservationId == histoResultSaveRequestModle.histoObservationId).FirstOrDefaultAsync();
                        if (historesult != null)
                        {
                            var historesultlog = createHistoresultLog(historesult);
                            db.tnx_Observations_Histo_Log.Add(historesultlog);
                            await db.SaveChangesAsync();
                            UpdateHistoResult(historesult, histoResultSaveRequestModle);
                            db.tnx_Observations_Histo.Update(historesult);
                        }
                        await db.SaveChangesAsync();

                    }
                    var sampleStatus = await db.tnx_BookingItem
                        .Where(bi => bi.id == histoResultSaveRequestModle.testId)
                        .SingleOrDefaultAsync();
                    var TestStatus = "";
                    if (sampleStatus != null)
                    {
                        if (histoResultSaveRequestModle.isApproved.ToString() == "1")
                        {
                            TestStatus = "Result Save and Approved";
                            sampleStatus.isResultDone = 1;
                            sampleStatus.isApproved = 1;
                            sampleStatus.resultDoneByID = histoResultSaveRequestModle.createdById;
                            sampleStatus.approvedByID = histoResultSaveRequestModle.createdById;
                            sampleStatus.resutDoneBy = histoResultSaveRequestModle.createdBy;
                            sampleStatus.approvedbyName = histoResultSaveRequestModle.createdBy;
                            sampleStatus.approvalDoctor = histoResultSaveRequestModle.approvaldoctorId;
                        }
                        else
                        {
                            TestStatus = "Result Saved";
                            sampleStatus.isResultDone = 1;
                            sampleStatus.resultDoneByID = histoResultSaveRequestModle.createdById;
                            sampleStatus.resutDoneBy = histoResultSaveRequestModle.createdBy;
                        }
                        db.tnx_BookingItem.Update(sampleStatus);
                        await db.SaveChangesAsync();
                        tnx_BookingStatus bookingStatus = new tnx_BookingStatus
                        {
                            id = 0,
                            transactionId = histoResultSaveRequestModle.transactionId,
                            patientId = histoResultSaveRequestModle.patientId,
                            barcodeNo = histoResultSaveRequestModle.barcodeNo,
                            status = TestStatus,
                            testId = (int)histoResultSaveRequestModle.testId,
                            createdById = histoResultSaveRequestModle.createdById,
                            createdDateTime = histoResultSaveRequestModle.createdDate
                        };
                        db.tnx_BookingStatus.Add(bookingStatus);
                        await db.SaveChangesAsync();
                    }
                    await transaction.CommitAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "All records saved successfully"
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = ex.InnerException?.Message ?? "An error occurred."
                    };
                }
            }

        }



        private tnx_Observations_Histo CreateHistoResult(HistoResultSaveRequestModle histoResultSaveRequestModle)
        {
            return new tnx_Observations_Histo
            {
                histoObservationId = histoResultSaveRequestModle.histoObservationId,
                testId = histoResultSaveRequestModle.testId,
                clinicalHistory = histoResultSaveRequestModle.clinicalHistory,
                specimen = histoResultSaveRequestModle.specimen,
                gross = histoResultSaveRequestModle.gross,
                typesFixativeUsed = histoResultSaveRequestModle.typesFixativeUsed,
                blockKeys = histoResultSaveRequestModle.blockKeys,
                stainsPerformed = histoResultSaveRequestModle.stainsPerformed,
                biospyNumber = histoResultSaveRequestModle.biospyNumber,
                microscopy = histoResultSaveRequestModle.microscopy,
                finalImpression = histoResultSaveRequestModle.finalImpression,
                comment = histoResultSaveRequestModle.comment,
                dtEntry = histoResultSaveRequestModle.dtEntry,
                createdById = histoResultSaveRequestModle.createdById,
                createdDateTime = histoResultSaveRequestModle.createdDate

            };

        }

        private void UpdateHistoResult(tnx_Observations_Histo historesult, HistoResultSaveRequestModle histoResultSaveRequestModle)
        {

            historesult.testId = histoResultSaveRequestModle.testId;
            historesult.clinicalHistory = histoResultSaveRequestModle.clinicalHistory;
            historesult.specimen = histoResultSaveRequestModle.specimen;
            historesult.gross = histoResultSaveRequestModle.gross;
            historesult.typesFixativeUsed = histoResultSaveRequestModle.typesFixativeUsed;
            historesult.blockKeys = histoResultSaveRequestModle.blockKeys;
            historesult.stainsPerformed = histoResultSaveRequestModle.stainsPerformed;
            historesult.biospyNumber = histoResultSaveRequestModle.biospyNumber;
            historesult.microscopy = histoResultSaveRequestModle.microscopy;
            historesult.finalImpression = histoResultSaveRequestModle.finalImpression;
            historesult.comment = histoResultSaveRequestModle.comment;
            historesult.dtEntry = histoResultSaveRequestModle.dtEntry;
            historesult.createdById = histoResultSaveRequestModle.createdById;
            historesult.createdDateTime = histoResultSaveRequestModle.createdDate;
        }


        private tnx_Observations_Histo_Log createHistoresultLog(tnx_Observations_Histo observations)
        {
            return new tnx_Observations_Histo_Log
            {
                histoObservationId = 0,
                testId = observations.testId,
                clinicalHistory = observations.clinicalHistory,
                specimen = observations.specimen,
                gross = observations.gross,
                typesFixativeUsed = observations.typesFixativeUsed,
                blockKeys = observations.blockKeys,
                stainsPerformed = observations.stainsPerformed,
                biospyNumber = observations.biospyNumber,
                microscopy = observations.microscopy,
                finalImpression = observations.finalImpression,
                comment = observations.comment,
                dtEntry = observations.dtEntry,
            };
        }

        async Task<ActionResult<ServiceStatusResponseModel>> Itnx_BookingItemServices.UpdateSampletransfer(List<tnx_Sra> SRA)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                if (SRA == null)
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = "Data not available"
                    };
                }
                try
                {
                    var TestId = SRA.Select(x => x.testId).Distinct().ToList();
                    var bookingItemData = db.tnx_BookingItem.Where(o => TestId.Contains(o.id)).ToList();
                    updateBookingitemdata(bookingItemData, 1);
                    db.tnx_BookingItem.UpdateRange(bookingItemData);
                    db.tnx_Sra.AddRange(SRA);
                    await db.SaveChangesAsync();
                    var BookingStatusData = SRA.Select(TransferStatusData).ToList();
                    db.tnx_BookingStatus.AddRange(BookingStatusData);
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "Saved SuccessFul"
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

        private void updateBookingitemdata(List<tnx_BookingItem> TnxBookingItemData, byte IsSra)
        {
            foreach (var item in TnxBookingItemData)
            {
                item.isSra = IsSra;
            }

        }
        private tnx_BookingStatus TransferStatusData(tnx_Sra sra)
        {
            return new tnx_BookingStatus
            {
                id = 0,
                transactionId = sra.transactionId,
                barcodeNo = sra.barcodeNo,
                status = "Sample Transfer Batch No" + sra.batchNumber,
                centreId = sra.fromCentreId,
                createdById = sra.entryById,
                createdDateTime = sra.entryDatetime,

                remarks = sra.remarks,
                testId = sra.testId,
                isActive = 1,
            };

        }

        async Task<ActionResult<ServiceStatusResponseModel>> Itnx_BookingItemServices.UpdateBatchReceive(List<BatchStatusRecieveRequestModel> batchStatusRecieveRequestModel)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                if (batchStatusRecieveRequestModel == null)
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = "Data not available"
                    };
                }
                try
                {
                    var TestId = batchStatusRecieveRequestModel.Select(x => x.testId).Distinct().ToList();
                    var bookingItemData = db.tnx_BookingItem.Where(o => TestId.Contains(o.id)).ToList();
                    var receivedBYIds = batchStatusRecieveRequestModel.Select(x => x.receivedBYId).Distinct().ToList();
                    if (receivedBYIds.Count != 1)
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "Inconsistent receivedBYId values"
                        };
                    }
                    var ReceivedBYId = receivedBYIds.First();
                    updateBookingitemBatchReceive(bookingItemData, (int)ReceivedBYId);
                    db.tnx_BookingItem.UpdateRange(bookingItemData);
                    var SraData = db.tnx_Sra.Where(o => TestId.Contains(o.id)).ToList();
                    updateSradata(SraData, (int)ReceivedBYId);
                    db.tnx_Sra.UpdateRange(SraData);
                    await db.SaveChangesAsync();
                    var BookingStatusData = batchStatusRecieveRequestModel.Select(BatchRecieveData).ToList();
                    db.tnx_BookingStatus.AddRange(BookingStatusData);
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "Saved SuccessFul"
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

        private void updateSradata(List<tnx_Sra> TnxSra, int ReceivedBYId)
        {
            foreach (var item in TnxSra)
            {
                item.status = "2";
                item.receivedBYId = ReceivedBYId;
                item.receivedOn = DateTime.Now;
            }
        }
        private void updateBookingitemBatchReceive(List<tnx_BookingItem> TnxBookingItemData, int ReceivedBYId)
        {
            foreach (var item in TnxBookingItemData)
            {
                item.isSra = 2;
                item.isSampleCollected = "Y";
                item.departmentReceiveByID = ReceivedBYId;
                item.departmentReceiveDate = DateTime.Now;
            }

        }
        private tnx_BookingStatus BatchRecieveData(BatchStatusRecieveRequestModel batchStatusRecieveRequestModel)
        {
            return new tnx_BookingStatus
            {
                id = 0,
                transactionId = batchStatusRecieveRequestModel.transactionId,
                barcodeNo = batchStatusRecieveRequestModel.barcodeNo,
                status = "Batch No" + batchStatusRecieveRequestModel.batchNumber + " Received",
                centreId = batchStatusRecieveRequestModel.toCentreId,
                createdById = batchStatusRecieveRequestModel.receivedBYId,
                createdDateTime = DateTime.Now,
                remarks = batchStatusRecieveRequestModel.remarks,
                testId = batchStatusRecieveRequestModel.testId,
                isActive = 1,
            };

        }

        async Task<ServiceStatusResponseModel> Itnx_BookingItemServices.GetitemDetailRate(int ratetype)
        {
            try
            {
                var result = await (from im in db.itemMaster
                                    join rtt in db.rateTypeWiseRateList on im.itemId equals rtt.itemid
                                    where rtt.rateTypeId == ratetype
                                    select new
                                    {
                                        im.itemId,
                                        im.itemName,
                                        im.itemType,
                                        im.sortName,
                                        im.gender,
                                        rtt.rate,
                                        deliveryDate = DateTime.Now.AddHours(3).ToString("yyyy-MM-dd hh:mm tt")
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

        async Task<ServiceStatusResponseModel> Itnx_BookingItemServices.GetitemDetail(int ratetype, int itemId)
        {

            try
            {
                var result = await (from im in db.itemMaster
                                    join rtt in db.rateTypeWiseRateList on im.itemId equals rtt.itemid
                                    join stt in db.itemSampleTypeMapping on im.itemId equals stt.itemId
                                    join st in db.sampletype_master on stt.sampleTypeId equals st.id
                                    join ld in db.labDepartment on im.deptId equals ld.id
                                    where rtt.rateTypeId == ratetype && im.itemId == itemId
                                    select new
                                    {
                                        im.itemId,
                                        im.itemName,
                                        im.itemType,
                                        im.sortName,
                                        testcode = im.code,
                                        im.deptId,
                                        departmentname = ld.deptName,
                                        im.gender,
                                        rtt.mrp,
                                        Grosss = rtt.rate,
                                        discount = 0,
                                        NetAmt = rtt.rate,
                                        stt.sampleTypeId,
                                        st.sampleTypeName,
                                        im.defaultsampletype,
                                        deliveryDate = DateTime.Now.AddHours(3).ToString("yyyy-MM-dd hh:mm tt")
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

        async Task<ServiceStatusResponseModel> Itnx_BookingItemServices.GetPackageTestDetail(int itemId)
        {
            try
            {
                var result = await (from im in db.itemMaster
                                    join iom in db.ItemObservationMapping on im.itemId equals iom.observationID
                                    where iom.itemId == itemId
                                    // group im by iom.itemId into grouped
                                    select new
                                    {
                                        itemNames = im.itemName
                                        //  itemNames = string.Join(", ", grouped.Select(im => im.itemName))
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

        async Task<ServiceStatusResponseModel> Itnx_BookingItemServices.GetOldPatient(string searchValue)
        {
            try
            {
                var patientinfo = (from pb in db.tnx_BookingPatient
                                   join b in db.tnx_Booking on pb.patientId equals b.patientId
                                   where pb.mobileNo == searchValue || b.workOrderId == searchValue
                                   || pb.name == searchValue
                                   select new
                                   {
                                       b.patientId,
                                       b.title_id,
                                       b.name,
                                       ageShow = string.Concat(b.ageYear, " Y ", b.ageMonth, " M ", b.ageDay, " D"),
                                       b.ageDay,
                                       b.ageMonth,
                                       b.ageYear,
                                       b.gender,
                                       b.mobileNo,
                                       pb.emailId,
                                       b.bookingDate
                                   }).FirstOrDefault();
                if (patientinfo != null)
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Data = patientinfo
                    };
                }
                else
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = "No data found"
                    };
                }
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

        async Task<ServiceStatusResponseModel> Itnx_BookingItemServices.GetPatientEditInfo(string searchValue)
        {
            try
            {
                var patientinfo = (from pb in db.tnx_BookingPatient
                                   join b in db.tnx_Booking on pb.patientId equals b.patientId
                                   where b.workOrderId == searchValue
                                   select new
                                   {
                                       b.mobileNo,
                                       b.patientId,
                                       b.workOrderId,
                                       b.transactionId,
                                       b.title_id,
                                       b.name,
                                       b.ageDay,
                                       b.ageMonth,
                                       b.ageYear,
                                       b.dob,
                                       b.gender,
                                       pb.emailId,
                                       b.refID1,
                                       b.refID2,
                                       pb.address,
                                       pb.pinCode,
                                       b.OtherLabReferID,
                                       b.otherLabRefer,
                                       b.uploadDocument
                                   }).FirstOrDefault();
                if (patientinfo != null)
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Data = patientinfo
                    };
                }
                else
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = "No data found"
                    };
                }
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

        async Task<ServiceStatusResponseModel> Itnx_BookingItemServices.GetPatientEditTest(string searchValue)
        {
            try
            {
                var patientinfo = await (from pb in db.tnx_BookingPatient
                                         join b in db.tnx_Booking on pb.patientId equals b.patientId

                                         where b.workOrderId == searchValue
                                         select new
                                         {

                                             b.mobileNo,
                                             b.patientId,
                                             b.workOrderId,
                                             b.transactionId,
                                             b.title_id,
                                             b.name,
                                             b.ageDay,
                                             b.ageMonth,
                                             b.ageYear,
                                             b.dob,
                                             b.gender,
                                             pb.emailId,
                                             b.refID1,
                                             b.refID2,
                                             pb.address,
                                             pb.pinCode,
                                             b.OtherLabReferID,
                                             b.otherLabRefer,
                                             b.uploadDocument,
                                             b.centreId,
                                             b.rateId,
                                             itemdetail = (db.tnx_BookingItem.Where(bi => bi.workOrderId == b.workOrderId && bi.transactionId == b.transactionId && bi.isRemoveItem == 0).ToList())
                                         }).FirstOrDefaultAsync();
                if (patientinfo != null)
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Data = patientinfo
                    };
                }
                else
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = "No data found"
                    };
                }
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

        async Task<ServiceStatusResponseModel> Itnx_BookingItemServices.UpdatePatientinfo(UpdatePatientInfoRequestModel patientInfo)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var bookingPatient = await db.tnx_BookingPatient
                                                  .Where(b => b.patientId == patientInfo.patientId)
                                                  .FirstOrDefaultAsync();

                    var booking = await db.tnx_Booking
                                          .Where(b => b.patientId == patientInfo.patientId && b.workOrderId == patientInfo.workOrderId)
                                          .FirstOrDefaultAsync();

                    if (bookingPatient != null && booking != null)
                    {
                        // Updating bookingPatient
                        bookingPatient.title_id = patientInfo.title_id;
                        bookingPatient.name = patientInfo.name;
                        bookingPatient.ageDays = patientInfo.ageDay;
                        bookingPatient.ageMonth = patientInfo.ageMonth;
                        bookingPatient.ageYear = patientInfo.ageYear;
                        bookingPatient.dob = patientInfo.dob;
                        bookingPatient.gender = patientInfo.gender;
                        bookingPatient.emailId = patientInfo.emailId;
                        bookingPatient.address = patientInfo.address;
                        bookingPatient.pinCode = patientInfo.pinCode;
                        bookingPatient.mobileNo = patientInfo.mobileno;

                        // Updating booking
                        booking.title_id = patientInfo.title_id;
                        booking.name = patientInfo.name;
                        booking.ageDay = patientInfo.ageDay;
                        booking.ageMonth = patientInfo.ageMonth;
                        booking.ageYear = patientInfo.ageYear;
                        booking.dob = patientInfo.dob;
                        booking.gender = patientInfo.gender;
                        booking.refID1 = patientInfo.refID1;
                        booking.refID2 = patientInfo.refID2;
                        booking.OtherLabReferID = patientInfo.OtherLabReferID;
                        booking.otherLabRefer = patientInfo.otherLabRefer;
                        booking.uploadDocument = patientInfo.uploadDocument;
                        booking.mobileNo = patientInfo.mobileno;

                        // Update the entities in DB
                        db.tnx_BookingPatient.Update(bookingPatient);
                        db.tnx_Booking.Update(booking);

                        // Save changes and commit transaction
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();

                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Record Updated Successfully"
                        };
                    }
                    else
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "No data found to update"
                        };
                    }
                }
                catch (Exception ex)
                {
                    // Consider logging the error for debugging purposes
                    // log.Error("Error in UpdatePatientinfo", ex);
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = "An error occurred: " + ex.Message
                    };
                }
            }
        }

        async Task<ServiceStatusResponseModel> Itnx_BookingItemServices.UpdatePatientTest(List<tnx_BookingItem> Updatetestdetail)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                if (Updatetestdetail.Count > 0)
                {
                    var mrp = 0.0;
                    var gross = 0.0;
                    var discount = 0.0;
                    var net = 0.0;
                    var transactionId = 0;
                    foreach (var item in Updatetestdetail)
                    {
                        mrp = mrp + item.mrp;
                        gross = gross + item.rate;
                        discount = discount + item.discount;
                        net = net + item.netAmount;
                        transactionId = item.transactionId;
                        if (item.id == 0)
                        {
                            var data = CreateBookingItem(item);
                            db.tnx_BookingItem.Add(data);
                            if (item.isPackage == 1)
                            {
                                //var itemdetails = _MySql_Procedure_Services.GetPackageItem(BookingItems.itemId, BookingItems.centreId);
                                var itemdetails = (from iom in db.ItemObservationMapping
                                                   join im in db.itemMaster on iom.observationID equals im.itemId
                                                   join ld in db.labDepartment on im.deptId equals ld.id
                                                   where iom.itemId == item.itemId
                                                   join rtr in db.rateTypeWiseRateList on iom.observationID equals rtr.itemid into rtrJoin
                                                   from rtr in rtrJoin.DefaultIfEmpty() // Left Join
                                                   join rtt in db.rateTypeTagging on rtr.rateTypeId equals rtt.rateTypeId into rttJoin
                                                   from rtt in rttJoin.DefaultIfEmpty() // Left Join
                                                   where rtt == null || rtt.centreId == item.centreId // Allow null rtt or filter by centreId
                                                   select new
                                                   {
                                                       testCode = im.code,
                                                       itemId = im.itemId,
                                                       im.deptId,
                                                       departmentName = ld.deptName, // Empty string as per SQL
                                                       investigationName = im.itemName,
                                                       im.itemType,
                                                       mrp = rtr != null ? rtr.mrp : (double?)null, // Explicit null check for rtr.mrp
                                                       rate = rtr != null ? rtr.rate : (double?)null, // Explicit null check for rtr.rate
                                                       discount = rtr != null ? rtr.discount : (double?)null, // Explicit null check for rtr.discount
                                                       netAmount = rtr != null ? rtr.rate : (double?)0
                                                   }).ToList();
                                if (itemdetails != null)
                                {
                                    foreach (var packageitem in itemdetails)
                                    {
                                        var createPackageItemDetail = createPackageItemData(item, packageitem);
                                        db.tnx_BookingItem.Add(createPackageItemDetail);
                                    }
                                }

                            }
                            await db.SaveChangesAsync();
                        }
                        else
                        {
                            var tnxBookingItemData = await db.tnx_BookingItem.FirstOrDefaultAsync(bookingItem => bookingItem.id == item.id);
                            if (tnxBookingItemData != null)
                            {
                                updateBookingItemDetail(tnxBookingItemData, item);
                                var tnxBookingItem = db.tnx_BookingItem.Update(tnxBookingItemData);
                                await db.SaveChangesAsync();
                            }
                            await db.SaveChangesAsync();

                        }
                    }
                    var booking = db.tnx_Booking.Where(b => b.transactionId == transactionId).FirstOrDefault();
                    if (booking != null)
                    {
                        booking.mrp = mrp;
                        booking.grossAmount = gross;
                        booking.discount = discount;
                        booking.netAmount = net;
                        db.tnx_Booking.Update(booking);
                        await db.SaveChangesAsync();
                    }
                    await transaction.CommitAsync();
                }
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Message = "Updated Successful"
                };
            }
        }
        private tnx_BookingItem createPackageItemData(tnx_BookingItem bookingItem, dynamic packageItem)
        {
            return new tnx_BookingItem
            {
                id = bookingItem.id,
                workOrderId = bookingItem.workOrderId,
                transactionId = bookingItem.transactionId,
                testcode = packageItem.testCode,
                itemId = packageItem.itemId,
                packageID = bookingItem.packageID,
                deptId = packageItem.deptId,
                departmentName = packageItem.departmentName,
                barcodeNo = bookingItem.barcodeNo,
                investigationName = packageItem.investigationName,
                isPackage = bookingItem.isPackage,
                packageName = bookingItem.packageName,
                itemType = packageItem.itemType,
                mrp = packageItem.mrp,
                rate = packageItem.rate,
                discount = packageItem.discount,
                netAmount = packageItem.netAmount,
                packMrp = bookingItem.packMrp,
                packItemNet = bookingItem.packItemNet,
                packItemRate = bookingItem.packItemRate,
                packItemDiscount = bookingItem.packItemDiscount,
                reportType = bookingItem.reportType,
                centreId = bookingItem.centreId,
                sessionCentreid = bookingItem.sessionCentreid,
                isSra = bookingItem.isSra,
                isMachineOrder = bookingItem.isMachineOrder,
                isEmailsent = bookingItem.isEmailsent,
                sampleTypeId = bookingItem.sampleTypeId,
                sampleTypeName = bookingItem.sampleTypeName,
                sampleCollectionDate = bookingItem.sampleCollectionDate,
                sampleCollectedby = bookingItem.sampleCollectedby,
                sampleCollectedID = bookingItem.sampleCollectedID,
                sampleReceiveDate = bookingItem.sampleReceiveDate,
                sampleReceivedBY = bookingItem.sampleReceivedBY,
                resultDate = bookingItem.resultDate,
                resultDoneByID = bookingItem.resultDoneByID,
                resutDoneBy = bookingItem.resutDoneBy,
                isApproved = bookingItem.isApproved,
                approvedDate = bookingItem.approvedDate,
                approvedByID = bookingItem.approvedByID,
                approvedbyName = bookingItem.approvedbyName,
                notApprovedBy = bookingItem.notApprovedBy,
                notApprovedDate = bookingItem.notApprovedDate,
                isReporting = bookingItem.isReporting,
                isCritical = bookingItem.isCritical,
                deliveryDate = bookingItem.deliveryDate,
                isInvoiceCreated = bookingItem.isInvoiceCreated,
                invoiceNumber = bookingItem.invoiceNumber,
                isUrgent = bookingItem.isUrgent,
                isSampleCollected = bookingItem.isSampleCollected,
                samplecollectionremarks = bookingItem.samplecollectionremarks,
                departmentReceiveRemarks = bookingItem.departmentReceiveRemarks,
                departmentReceiveDate = bookingItem.departmentReceiveDate,
                departmentReceiveBy = bookingItem.departmentReceiveBy,
                departmentReceiveByID = bookingItem.departmentReceiveByID,
                isRemoveItem = bookingItem.isRemoveItem,
                sampleRejectionBy = bookingItem.sampleRejectionBy,
                sampleRejectionByName = bookingItem.sampleRejectionByName,
                sampleRejectionOn = bookingItem.sampleRejectionOn,
                interpretationId = bookingItem.interpretationId,
                approvalDoctor = bookingItem.approvalDoctor,
                isOuthouse = bookingItem.isOuthouse,
                outhouseLab = bookingItem.outhouseLab,
                labName = bookingItem.labName,
                outhouseDoneBy = bookingItem.outhouseDoneBy,
                outhouseDoneOn = bookingItem.outhouseDoneOn,
                sampleRecollectedby = bookingItem.sampleRecollectedby,
                sampleRecollectedDate = bookingItem.sampleRecollectedDate,
                isrerun = bookingItem.isrerun,
                invoiceNo = bookingItem.invoiceNo,
                invoiceDate = bookingItem.invoiceDate,
                invoiceCycle = bookingItem.invoiceCycle,
                invoiceAmount = bookingItem.invoiceAmount,
                invoiceCreatedBy = bookingItem.invoiceCreatedBy,
                invoiceNoOld = bookingItem.invoiceNoOld,
                remarks = bookingItem.remarks,
                showonReportdate = bookingItem.showonReportdate,
                isActive = bookingItem.isActive,
                createdById = bookingItem.createdById,
                createdDateTime = bookingItem.createdDateTime,
            };

        }

        private void updateBookingItemDetail(tnx_BookingItem tnxBookItem, tnx_BookingItem bookingItem)
        {
            tnxBookItem.workOrderId = bookingItem.workOrderId;
            tnxBookItem.transactionId = bookingItem.transactionId;
            tnxBookItem.testcode = bookingItem.testcode;
            tnxBookItem.itemId = bookingItem.itemId;
            tnxBookItem.packageID = bookingItem.packageID;
            tnxBookItem.deptId = bookingItem.deptId;
            tnxBookItem.departmentName = bookingItem.departmentName;
            tnxBookItem.barcodeNo = bookingItem.barcodeNo;
            tnxBookItem.investigationName = bookingItem.investigationName;
            tnxBookItem.isPackage = bookingItem.isPackage;
            tnxBookItem.packageName = bookingItem.packageName;
            tnxBookItem.itemType = bookingItem.itemType;
            tnxBookItem.mrp = bookingItem.mrp;
            tnxBookItem.rate = bookingItem.rate;
            tnxBookItem.discount = bookingItem.discount;
            tnxBookItem.netAmount = bookingItem.netAmount;
            tnxBookItem.packMrp = bookingItem.packMrp;
            tnxBookItem.packItemNet = bookingItem.packItemNet;
            tnxBookItem.packItemRate = bookingItem.packItemRate;
            tnxBookItem.packItemDiscount = bookingItem.packItemDiscount;
            tnxBookItem.reportType = bookingItem.reportType;
            tnxBookItem.centreId = bookingItem.centreId;
            tnxBookItem.sessionCentreid = bookingItem.sessionCentreid;
            tnxBookItem.isSra = bookingItem.isSra;
            tnxBookItem.isMachineOrder = bookingItem.isMachineOrder;
            tnxBookItem.isEmailsent = bookingItem.isEmailsent;
            tnxBookItem.sampleTypeId = bookingItem.sampleTypeId;
            tnxBookItem.sampleTypeName = bookingItem.sampleTypeName;
            tnxBookItem.sampleCollectionDate = bookingItem.sampleCollectionDate;
            tnxBookItem.sampleCollectedby = bookingItem.sampleCollectedby;
            tnxBookItem.sampleCollectedID = bookingItem.sampleCollectedID;
            tnxBookItem.sampleReceiveDate = bookingItem.sampleReceiveDate;
            tnxBookItem.sampleReceivedBY = bookingItem.sampleReceivedBY;
            tnxBookItem.resultDate = bookingItem.resultDate;
            tnxBookItem.resultDoneByID = bookingItem.resultDoneByID;
            tnxBookItem.resutDoneBy = bookingItem.resutDoneBy;
            tnxBookItem.isApproved = bookingItem.isApproved;
            tnxBookItem.approvedDate = bookingItem.approvedDate;
            tnxBookItem.approvedByID = bookingItem.approvedByID;
            tnxBookItem.approvedbyName = bookingItem.approvedbyName;
            tnxBookItem.notApprovedBy = bookingItem.notApprovedBy;
            tnxBookItem.notApprovedDate = bookingItem.notApprovedDate;
            tnxBookItem.isActive = bookingItem.isActive;
            tnxBookItem.isReporting = bookingItem.isReporting;
            tnxBookItem.isCritical = bookingItem.isCritical;
            tnxBookItem.deliveryDate = bookingItem.deliveryDate;
            tnxBookItem.isInvoiceCreated = bookingItem.isInvoiceCreated;
            tnxBookItem.invoiceNumber = bookingItem.invoiceNumber;
            tnxBookItem.isUrgent = bookingItem.isUrgent;
            tnxBookItem.isSampleCollected = bookingItem.isSampleCollected;
            tnxBookItem.samplecollectionremarks = bookingItem.samplecollectionremarks;
            tnxBookItem.departmentReceiveRemarks = bookingItem.departmentReceiveRemarks;
            tnxBookItem.departmentReceiveDate = bookingItem.departmentReceiveDate;
            tnxBookItem.departmentReceiveBy = bookingItem.departmentReceiveBy;
            tnxBookItem.departmentReceiveByID = bookingItem.departmentReceiveByID;
            tnxBookItem.isRemoveItem = bookingItem.isRemoveItem;
            tnxBookItem.sampleRejectionBy = bookingItem.sampleRejectionBy;
            tnxBookItem.sampleRejectionByName = bookingItem.sampleRejectionByName;
            tnxBookItem.sampleRejectionOn = bookingItem.sampleRejectionOn;
            tnxBookItem.interpretationId = bookingItem.interpretationId;
            tnxBookItem.approvalDoctor = bookingItem.approvalDoctor;
            tnxBookItem.isOuthouse = bookingItem.isOuthouse;
            tnxBookItem.outhouseLab = bookingItem.outhouseLab;
            tnxBookItem.labName = bookingItem.labName;
            tnxBookItem.outhouseDoneBy = bookingItem.outhouseDoneBy;
            tnxBookItem.outhouseDoneOn = bookingItem.outhouseDoneOn;
            tnxBookItem.sampleRecollectedby = bookingItem.sampleRecollectedby;
            tnxBookItem.sampleRecollectedDate = bookingItem.sampleRecollectedDate;
            tnxBookItem.isrerun = bookingItem.isrerun;
            tnxBookItem.invoiceNo = bookingItem.invoiceNo;
            tnxBookItem.invoiceDate = bookingItem.invoiceDate;
            tnxBookItem.invoiceCycle = bookingItem.invoiceCycle;
            tnxBookItem.invoiceAmount = bookingItem.invoiceAmount;
            tnxBookItem.invoiceCreatedBy = bookingItem.invoiceCreatedBy;
            tnxBookItem.invoiceNoOld = bookingItem.invoiceNoOld;
            tnxBookItem.remarks = bookingItem.remarks;
            tnxBookItem.showonReportdate = bookingItem.showonReportdate;
            tnxBookItem.isActive = bookingItem.isActive;
            tnxBookItem.updateById = bookingItem.updateById;
            tnxBookItem.updateDateTime = bookingItem.updateDateTime;
        }

        private tnx_BookingItem CreateBookingItem(tnx_BookingItem bookingItem)
        {
            return new tnx_BookingItem
            {
                id = bookingItem.id,
                workOrderId = bookingItem.workOrderId,
                transactionId = bookingItem.transactionId,
                testcode = bookingItem.testcode,
                itemId = bookingItem.itemId,
                packageID = bookingItem.packageID,
                deptId = bookingItem.deptId,
                departmentName = bookingItem.departmentName,
                barcodeNo = bookingItem.barcodeNo,
                investigationName = bookingItem.investigationName,
                isPackage = bookingItem.isPackage,
                packageName = bookingItem.packageName,
                itemType = bookingItem.itemType,
                mrp = bookingItem.mrp,
                rate = bookingItem.rate,
                discount = bookingItem.discount,
                netAmount = bookingItem.netAmount,
                packMrp = bookingItem.packMrp,
                packItemNet = bookingItem.packItemNet,
                packItemRate = bookingItem.packItemRate,
                packItemDiscount = bookingItem.packItemDiscount,
                reportType = bookingItem.reportType,
                centreId = bookingItem.centreId,
                sessionCentreid = bookingItem.sessionCentreid,
                isSra = bookingItem.isSra,
                isMachineOrder = bookingItem.isMachineOrder,
                isEmailsent = bookingItem.isEmailsent,
                sampleTypeId = bookingItem.sampleTypeId,
                sampleTypeName = bookingItem.sampleTypeName,
                sampleCollectionDate = bookingItem.sampleCollectionDate,
                sampleCollectedby = bookingItem.sampleCollectedby,
                sampleCollectedID = bookingItem.sampleCollectedID,
                sampleReceiveDate = bookingItem.sampleReceiveDate,
                sampleReceivedBY = bookingItem.sampleReceivedBY,
                resultDate = bookingItem.resultDate,
                resultDoneByID = bookingItem.resultDoneByID,
                resutDoneBy = bookingItem.resutDoneBy,
                isApproved = bookingItem.isApproved,
                approvedDate = bookingItem.approvedDate,
                approvedByID = bookingItem.approvedByID,
                approvedbyName = bookingItem.approvedbyName,
                notApprovedBy = bookingItem.notApprovedBy,
                notApprovedDate = bookingItem.notApprovedDate,
                isReporting = bookingItem.isReporting,
                isCritical = bookingItem.isCritical,
                deliveryDate = bookingItem.deliveryDate,
                isInvoiceCreated = bookingItem.isInvoiceCreated,
                invoiceNumber = bookingItem.invoiceNumber,
                isUrgent = bookingItem.isUrgent,
                isSampleCollected = bookingItem.isSampleCollected,
                samplecollectionremarks = bookingItem.samplecollectionremarks,
                departmentReceiveRemarks = bookingItem.departmentReceiveRemarks,
                departmentReceiveDate = bookingItem.departmentReceiveDate,
                departmentReceiveBy = bookingItem.departmentReceiveBy,
                departmentReceiveByID = bookingItem.departmentReceiveByID,
                isRemoveItem = bookingItem.isRemoveItem,
                sampleRejectionBy = bookingItem.sampleRejectionBy,
                sampleRejectionByName = bookingItem.sampleRejectionByName,
                sampleRejectionOn = bookingItem.sampleRejectionOn,
                interpretationId = bookingItem.interpretationId,
                approvalDoctor = bookingItem.approvalDoctor,
                isOuthouse = bookingItem.isOuthouse,
                outhouseLab = bookingItem.outhouseLab,
                labName = bookingItem.labName,
                outhouseDoneBy = bookingItem.outhouseDoneBy,
                outhouseDoneOn = bookingItem.outhouseDoneOn,
                sampleRecollectedby = bookingItem.sampleRecollectedby,
                sampleRecollectedDate = bookingItem.sampleRecollectedDate,
                isrerun = bookingItem.isrerun,
                invoiceNo = bookingItem.invoiceNo,
                invoiceDate = bookingItem.invoiceDate,
                invoiceCycle = bookingItem.invoiceCycle,
                invoiceAmount = bookingItem.invoiceAmount,
                invoiceCreatedBy = bookingItem.invoiceCreatedBy,
                invoiceNoOld = bookingItem.invoiceNoOld,
                remarks = bookingItem.remarks,
                showonReportdate = bookingItem.showonReportdate,
                isActive = bookingItem.isActive,
                createdById = bookingItem.createdById,
                createdDateTime = bookingItem.createdDateTime,
            };

        }

        async Task<ServiceStatusResponseModel> Itnx_BookingItemServices.GetResultEntryAllData(ResultEntryAllDataRequestModel resultentrydata)
        {
            var query = from tb in db.tnx_Booking
                        join tbi in db.tnx_BookingItem on tb.transactionId equals tbi.transactionId
                        join im in db.itemMaster on tbi.itemId equals im.itemId
                        join cm in db.centreMaster on tb.centreId equals cm.centreId
                        join tm in db.titleMaster on tb.title_id equals tm.id
                        select new
                        {
                            bookingDate= tb.bookingDate.ToString("yyyy-MMM-dd hh:mm tt"),
                            tb.patientId,
                            BokkingDateFilter= tb.bookingDate,
                            tb.workOrderId,
                            SampleRecievedDateShow = tbi.sampleReceiveDate.HasValue ? tbi.sampleReceiveDate.Value.ToString("yyyy-MMM-dd hh:mm tt") : "",
                            ApproveDateShow = tbi.approvedDate.HasValue ? tbi.approvedDate.Value.ToString("yyyy-MMM-dd hh:mm tt") : "",
                            PatientName = string.Concat(tm.title, " ", tb.name),
                            Age = string.Concat(tb.ageYear, " Y ", tb.ageMonth, " M ", tb.ageDay, " D"),
                            tb.gender,
                            tbi.barcodeNo,
                            testid = tbi.id,
                            tbi.itemId,
                            investigationName = tbi.isPackage == 1 ? string.Concat(tbi.packageName, "<br>", tbi.investigationName) : tbi.investigationName,
                            investigationSortName = im.sortName,
                            Comment = tb.labRemarks,
                            ApprovedDateShow = tbi.approvedDate,
                            tbi.centreId,
                            cm.centrecode,
                            centreName = cm.companyName,
                            tbi.deptId,
                            tbi.departmentName,
                            tbi.isSampleCollected,
                            tb.transactionId,
                            tbi.createdDateTime,
                            tbi.approvedDate,
                            tbi.sampleReceiveDate,
                            tbi.sampleCollectionDate,
                            isremark = db.tnx_InvestigationRemarks.Where(s => s.itemId == tbi.itemId && s.transactionId == tbi.transactionId).Count(),
                            im.reportType,
                            Urgent = tbi.isUrgent,
                            resultdone = tbi.isResultDone,
                            Approved = tbi.isApproved,
                            Rerun = tbi.isrerun,

                            rowcolor = tbi.isUrgent == 1 ? "#FF4E12" : (tbi.isUrgent == 0 && tbi.isSampleCollected == "N" ? "#D7CDC6" : (tbi.isUrgent == 0 && tbi.isSampleCollected == "S" ? "#6699ff" : (tbi.isUrgent == 0 && tbi.isSampleCollected == "Y" ? "#75AADB" : "#F0466B")))

                        };
            if (resultentrydata.Datetype != "")
            {
                var fromDate = resultentrydata.FromDate.Date; // Strip the time portion
                var toDate = resultentrydata.ToDate.Date.AddHours(24).AddSeconds(-1); // Include the entire end date

                if (resultentrydata.Datetype == "tb.bookingDate")
                {
                    query = query.Where(q => q.BokkingDateFilter >= fromDate && q.BokkingDateFilter <= toDate);
                }
                else if (resultentrydata.searchvalue == "tbi.approvedDate")
                {
                    query = query.Where(q => q.approvedDate >= fromDate && q.approvedDate <= toDate);
                }
                else if (resultentrydata.searchvalue == "tbi.sampleReceiveDate")
                {
                    query = query.Where(q => q.sampleReceiveDate >= fromDate && q.sampleReceiveDate <= toDate);
                }
                else
                {
                    query = query.Where(q => q.sampleCollectionDate >= fromDate && q.sampleCollectionDate <= toDate);
                }
            }
            if (resultentrydata.searchvalue != "")
            {
                query = query.Where(q => q.barcodeNo == resultentrydata.searchvalue || q.workOrderId == resultentrydata.searchvalue);
            }
            if (resultentrydata.ItemIds.Count > 0)
            {
                query = query.Where(q => resultentrydata.ItemIds.Contains(q.itemId));
            }
            if (resultentrydata.centreIds.Count > 0)
            {
                query = query.Where(q => resultentrydata.centreIds.Contains(q.centreId));
            }
            else
            {
                List<int> CentreIds = db.empCenterAccess.Where(e => e.empId == resultentrydata.empid).Select(e => e.centreId).ToList();
                query = query.Where(q => CentreIds.Contains(q.centreId));
            }

            if (resultentrydata.departmentIds.Count > 0)
            {
                query = query.Where(q => resultentrydata.departmentIds.Contains(q.deptId));
            }
            if (resultentrydata.status != "")
            {
                if (resultentrydata.status == "Pending")
                {
                    query = query.Where(q => q.isSampleCollected == "Y");
                }
                else if (resultentrydata.status == "Tested")
                {
                    query = query.Where(q => q.resultdone == 1);
                }
                else if (resultentrydata.status == "Approved")
                {
                    query = query.Where(q => q.Approved == 1);
                }
                else if (resultentrydata.status == "Rejected")
                {
                    query = query.Where(q => q.isSampleCollected == "R");
                }
                else if (resultentrydata.status == "Received")
                {
                    query = query.Where(q => q.isSampleCollected == "Y");
                }
                else
                {
                    query = query.Where(q => q.isSampleCollected == "S");
                }
            }
            if (resultentrydata.reporttype < 3)
                query = query.Where(q => q.reportType == 1 || q.reportType == 2 );
            else
                query = query.Where(q => q.reportType == resultentrydata.reporttype);

            query = query.OrderBy(q => q.workOrderId).ThenBy(q => q.deptId);
            var result = await query.ToListAsync();
            return new ServiceStatusResponseModel
            {
                Success = true,
                Data = result
            };
        }

        async Task<ServiceStatusResponseModel> Itnx_BookingItemServices.SaveMicroResult(MicroResultSaveRequestModel microFlowcyto)
        {
            await using var transaction = await db.Database.BeginTransactionAsync();
            try
            {
                // Remove old data
                var dataOld = db.tnx_Observations_Micro_Flowcyto
                                .Where(t => t.testId == microFlowcyto.testId && t.reportStatus == microFlowcyto.reportStatus)
                                .ToList();
                db.tnx_Observations_Micro_Flowcyto.RemoveRange(dataOld);

                // Ensure selectedAntibiotic is not null
                microFlowcyto.selectedorganism ??= new List<organismdata>();
                if (microFlowcyto.selectedorganism.Count > 0)
                {
                    foreach (var item in microFlowcyto.selectedorganism)
                    {
                        var organismId = item.organismId;
                        var OrganismName = item.organismName;
                        var itemsToAdd = item.selectedAntibiotic.ConvertAll(item => CreateMicroResult(item, microFlowcyto, organismId, OrganismName));

                        db.tnx_Observations_Micro_Flowcyto.AddRange(itemsToAdd);
                    }
                }
                else
                {
                    var intrimData = CreateIntrimReport(microFlowcyto);
                    db.tnx_Observations_Micro_Flowcyto.Add(intrimData);
                }

                // Update booking item status
                var sampleStatus = await db.tnx_BookingItem
                                           .Where(bi => bi.id == microFlowcyto.testId)
                                           .SingleOrDefaultAsync();

                if (sampleStatus != null)
                {
                    string testStatus = microFlowcyto.isApproved == 1 ?
                                        "Result Save and Approved" : "Result Saved";

                    sampleStatus.isResultDone = 1;
                    sampleStatus.resultDoneByID = microFlowcyto.createdById;
                    sampleStatus.resutDoneBy = microFlowcyto.createdBy;

                    if (microFlowcyto.isApproved == 1)
                    {
                        sampleStatus.isApproved = 1;
                        sampleStatus.approvedByID = microFlowcyto.createdById;
                        sampleStatus.approvedbyName = microFlowcyto.createdBy;
                    }

                    db.tnx_BookingItem.Update(sampleStatus);

                    // Log booking status
                    var bookingStatus = new tnx_BookingStatus
                    {
                        id = 0,
                        transactionId = microFlowcyto.transactionId,
                        status = testStatus,
                        testId = microFlowcyto.testId,
                        createdById = microFlowcyto.createdById,
                        createdDateTime = DateTime.Now
                    };

                    db.tnx_BookingStatus.Add(bookingStatus);
                }

                await db.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Message = "All records saved successfully"
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = ex.ToString() // Logs full exception details
                };
            }
        }

        private tnx_Observations_Micro_Flowcyto CreateMicroResult(antibioticData antibioticdata, MicroResultSaveRequestModel microResult, short organismId, string OrganismName)
        {
            return new tnx_Observations_Micro_Flowcyto
            {
                testId = microResult.testId,
                labTestId = microResult.labTestId,
                transactionId = microResult.transactionId,
                observationName = microResult.observationName,
                result = microResult.result,
                machineID = microResult.machineID,
                flag = microResult.flag,
                isBold = microResult.isBold,
                reportType = microResult.reportType,
                organismId = organismId,
                organismName = OrganismName,
                antibiticId = antibioticdata.antibiticId,
                antibitiName = antibioticdata.antibitiName,
                colonyCount = microResult.colonyCount,
                interpretation = antibioticdata.interpretation,
                mic = antibioticdata.mic,
                positivity = microResult.positivity,
                intensity = microResult.intensity,
                reportStatus = microResult.reportStatus,
                approvedBy = microResult.approvedBy,
                approvedName = microResult.approvedName,
                comments = microResult.comments
            };
        }

        private tnx_Observations_Micro_Flowcyto CreateIntrimReport( MicroResultSaveRequestModel microResult)
        {
            return new tnx_Observations_Micro_Flowcyto
            {
                testId = microResult.testId,
                labTestId = microResult.labTestId,
                transactionId = microResult.transactionId,
                observationName = microResult.observationName,
                result = microResult.result,
                machineID = microResult.machineID,
                flag = microResult.flag,
                isBold = microResult.isBold,
                reportType = microResult.reportType,
                organismId = 0,
                organismName = "",
                antibiticId = 0,
                antibitiName = "",
                colonyCount = microResult.colonyCount,
                interpretation = "",
                mic = "",
                positivity = microResult.positivity,
                intensity = microResult.intensity,
                reportStatus = microResult.reportStatus,
                approvedBy = microResult.approvedBy,
                approvedName = microResult.approvedName,
                comments = microResult.comments
            };
        }

    }
}

