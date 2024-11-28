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

        Task<List<SampleProcessingResponseModel>> Itnx_BookingItemServices.GetSampleProcessingData(SampleProcessingRequestModel sampleProcessingRequestModel)
        {
            var query = from tb in db.tnx_Booking
                        join tbi in db.tnx_BookingItem on tb.transactionId equals tbi.transactionId
                        select new SampleProcessingResponseModel
                        {
                            patientId = tb.patientId,
                            transactionId = tb.transactionId,
                            workOrderId = tb.workOrderId,
                            name = tb.name,
                            itemId = tbi.itemId,
                            investigationName = tbi.investigationName,
                            sampleTypeId = tbi.sampleTypeId,
                            sampleTypeName = tbi.sampleTypeName,
                            isSampleCollected = tbi.isSampleCollected,
                            createdDateTime = tbi.createdDateTime,
                            deptId = tbi.deptId,
                            barcodeNo = tbi.barcodeNo,
                            centreId = tbi.centreId
                        };

            // Apply date filter
            if (sampleProcessingRequestModel.fromDate.HasValue && sampleProcessingRequestModel.toDate.HasValue)
            {
                query = query.Where(q => q.createdDateTime >= sampleProcessingRequestModel.fromDate.Value && q.createdDateTime <= sampleProcessingRequestModel.toDate.Value);
            }
            if (sampleProcessingRequestModel.patientId != 0)
            {
                query = query.Where(q => q.patientId == sampleProcessingRequestModel.patientId);
            }

            if (sampleProcessingRequestModel.transactionId != 0)
            {
                query = query.Where(q => q.transactionId == sampleProcessingRequestModel.transactionId);
            }

            if (sampleProcessingRequestModel.department != 0)
            {
                query = query.Where(q => q.deptId == sampleProcessingRequestModel.department);
            }

            if (!string.IsNullOrEmpty(sampleProcessingRequestModel.barcodeNo))
            {
                query = query.Where(q => q.barcodeNo == sampleProcessingRequestModel.barcodeNo);
            }

            if (sampleProcessingRequestModel.centreId != 0)
            {
                query = query.Where(q => q.centreId == sampleProcessingRequestModel.centreId);
            }

            if (sampleProcessingRequestModel.investigationId != 0)
            {
                query = query.Where(q => q.itemId == sampleProcessingRequestModel.investigationId);
            }

            query = query.OrderBy(q => q.patientId).ThenBy(q => q.transactionId);
            return query.ToListAsync();
        }

        async Task<List<ResultEntryResponseModle>> Itnx_BookingItemServices.GetTestObservations(ResultEntryRequestModle resultEntryRequestModle)
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
                        var sampleStatus = await db.tnx_BookingItem
                            .Where(bi => bi.itemId == SampleResponseModel.itemId && bi.transactionId == SampleResponseModel.transactionId)
                            .SingleOrDefaultAsync();

                        if (sampleStatus != null)
                        {
                            UpdateSampleStatus(sampleStatus, SampleResponseModel);
                            db.tnx_BookingItem.Update(sampleStatus);
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
                        Message = ex.InnerException?.Message ?? "An error occurred."
                    };
                }
            }
        }
        private void UpdateSampleStatus(tnx_BookingItem sampleStatus, SampleProcessingResponseModel sampleProcessingResponseModel)
        {
            var empId = 1;

            switch (sampleProcessingResponseModel.isSampleCollected)
            {
                case "S":
                    sampleStatus.isSampleCollected = "S";
                    sampleStatus.sampleCollectedby = Convert.ToString(empId);
                    sampleStatus.sampleCollectionDate = DateTime.Now;
                    break;

                case "Y":
                    sampleStatus.isSampleCollected = "Y";
                    sampleStatus.sampleReceivedBY = Convert.ToString(empId);
                    sampleStatus.sampleReceiveDate = DateTime.Now;
                    break;

                case "R":
                    sampleStatus.isSampleCollected = "R";
                    sampleStatus.sampleRejectionBy = empId;
                    sampleStatus.sampleRejectionOn = DateTime.Now;
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

        async Task<ActionResult<ServiceStatusResponseModel>> Itnx_BookingItemServices.SaveHistoResult(HistoResultSaveRequestModle histoResultSaveRequestModle)
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
                status = "Sample Transfer Batch No"+ sra.batchNumber,
                centreId = sra.fromCentreId,
                createdById = sra.entryById,
                createdDateTime = sra.entryDatetime,

                remarks = sra.remarks,
                testId = sra.testId,
                isActive = true,
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
                status = "Batch No" + batchStatusRecieveRequestModel.batchNumber +" Received",
                centreId = batchStatusRecieveRequestModel.toCentreId,
                createdById = batchStatusRecieveRequestModel.receivedBYId,
                createdDateTime = DateTime.Now,
                remarks = batchStatusRecieveRequestModel.remarks,
                testId = batchStatusRecieveRequestModel.testId,
                isActive = true,
            };

        }
    }
}

