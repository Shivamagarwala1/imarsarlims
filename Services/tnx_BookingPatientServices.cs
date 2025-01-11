using Google.Rpc;
using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace iMARSARLIMS.Services
{
    public class tnx_BookingPatientServices : Itnx_BookingPatientServices
    {
        private readonly ContextClass db;
        private readonly IConfiguration _configuration;
        private readonly MySql_Function_Services _MySql_Function_Services;
        private readonly MySql_Procedure_Services _MySql_Procedure_Services;

        public int transactionId;

        public tnx_BookingPatientServices(ContextClass context, ILogger<BaseController<tnx_BookingPatient>> logger, MySql_Function_Services MySql_Function_Services, MySql_Procedure_Services mySql_Procedure_Services, IConfiguration configuration)
        {

            db = context;
            this._MySql_Function_Services = MySql_Function_Services;
            this._MySql_Procedure_Services = mySql_Procedure_Services;
            this._configuration = configuration;
        }

        public async Task<string> Getworkorderid(int centreId, string type)
        {
            var result = _MySql_Function_Services.Getworkorderid(centreId, type);
            return result;
        }
        async Task<ServiceStatusResponseModel> Itnx_BookingPatientServices.SavePatientRegistration(tnx_BookingPatient tnxBookingPatient)
        {

            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var TnxBookingData = tnxBookingPatient.addBooking.FirstOrDefault();
                    if (tnxBookingPatient.patientId == 0)
                    {
                        if (tnxBookingPatient.addBooking == null)
                        {
                            return new ServiceStatusResponseModel
                            {
                                Success = true,
                                Message = "Booking ledger Data can't null"
                            };
                        }
                        if (TnxBookingData.addBookingItem == null)
                        {
                            return new ServiceStatusResponseModel
                            {
                                Success = true,
                                Message = "Booking item Data can't null"
                            };

                        }
                        var bookingPatientData = CreateBookingPatient(tnxBookingPatient);
                        var bookingPatient = db.tnx_BookingPatient.Add(bookingPatientData);
                        await db.SaveChangesAsync();
                        var patientId = bookingPatient.Entity.patientId;
                        var workOrderId = _MySql_Function_Services.Getworkorderid(tnxBookingPatient.centreId, "W");
                        var transactionId = await SaveBooking(tnxBookingPatient.addBooking, patientId, workOrderId);
                        await SaveBookingStatus(TnxBookingData.addBookingStatus, patientId, transactionId);
                        await SaveBookingItem(TnxBookingData.addBookingItem, patientId, workOrderId, transactionId);
                        await transaction.CommitAsync();
                        var result = db.tnx_BookingPatient.Where(x => x.patientId == patientId).Include(b => b.addBooking).ThenInclude(b => b.addBookingItem)
                                  .ToList();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Data = result
                        };

                    }
                    else
                    {
                        var tnxBookingPatients = db.tnx_BookingPatient.FirstOrDefault(bookingPatient => bookingPatient.patientId == tnxBookingPatient.patientId);
                        if (tnxBookingPatients != null)
                        {
                            UpdateBookingPatient(tnxBookingPatients, tnxBookingPatient);
                        }
                        var bookingPatients = db.tnx_BookingPatient.Update(tnxBookingPatients);
                        await db.SaveChangesAsync();
                        var patientId = bookingPatients.Entity.patientId;
                        var workOrderId = tnxBookingPatient.addBooking[0].workOrderId;
                        var transactionId = tnxBookingPatient.addBooking[0].transactionId;
                        await UpdateBooking(tnxBookingPatient.addBooking, patientId, workOrderId);
                        await UpdateBookingStatus(TnxBookingData.addBookingStatus, patientId, transactionId);
                        await UpdateBookingItem(TnxBookingData.addBookingItem, patientId, workOrderId, transactionId);
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Data = tnxBookingPatient
                        };

                    }
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
        private tnx_BookingPatient CreateBookingPatient(tnx_BookingPatient tnxBookingPatient)
        {
            return new tnx_BookingPatient
            {
                patientId = tnxBookingPatient.patientId,
                title_id = tnxBookingPatient.title_id,
                name = tnxBookingPatient.name,
                gender = tnxBookingPatient.gender,
                ageTotal = tnxBookingPatient.ageTotal,
                ageDays = tnxBookingPatient.ageDays,
                ageMonth = tnxBookingPatient.ageMonth,
                ageYear = tnxBookingPatient.ageYear,
                dob = tnxBookingPatient.dob,
                isActualDOB = tnxBookingPatient.isActualDOB,
                emailId = tnxBookingPatient.emailId,
                mobileNo = tnxBookingPatient.mobileNo,
                address = tnxBookingPatient.address,
                pinCode = tnxBookingPatient.pinCode,
                cityId = tnxBookingPatient.cityId,
                centreId = tnxBookingPatient.centreId,
                areaId = tnxBookingPatient.areaId,
                stateId = tnxBookingPatient.stateId,
                districtId = tnxBookingPatient.districtId,
                countryId = tnxBookingPatient.countryId,
                visitCount = tnxBookingPatient.visitCount,
                remarks = tnxBookingPatient.remarks,
                documentId = tnxBookingPatient.documentId,
                documnetnumber = tnxBookingPatient.documnetnumber,
                createdDateTime = tnxBookingPatient.createdDateTime,
                createdById = tnxBookingPatient.createdById,
                password = tnxBookingPatient.password

            };
        }

        private async Task<int> SaveBooking(IEnumerable<tnx_Booking> tnxBooking, int patientId, string workOrderId)
        {
            var booking = tnxBooking.FirstOrDefault();
            if (booking != null)
            {
                var tnxbookingData = CreateBookingDetail(booking, patientId, workOrderId);
                var tnxbooking = db.tnx_Booking.Add(tnxbookingData);
                await db.SaveChangesAsync();
                transactionId = tnxbooking.Entity.transactionId;
            }
            return transactionId;

        }
        private tnx_Booking CreateBookingDetail(tnx_Booking bookingData, int patientId, string workOrderId)
        {
            return new tnx_Booking
            {

                transactionId = bookingData.transactionId,
                workOrderId = workOrderId,
                patientId = patientId,

                billNo = bookingData.billNo,
                title_id = bookingData.title_id,
                name = bookingData.name,
                gender = bookingData.gender,
                dob = bookingData.dob,
                ageYear = bookingData.ageYear,
                ageMonth = bookingData.ageMonth,
                ageDay = bookingData.ageDay,
                totalAge = bookingData.totalAge,
                clientCode = bookingData.clientCode,
                bookingDate = bookingData.bookingDate,
                mobileNo = bookingData.mobileNo,
                mrp = bookingData.mrp,
                grossAmount = bookingData.grossAmount,
                discount = bookingData.discount,
                netAmount = bookingData.netAmount,
                paidAmount = bookingData.paidAmount,
                discountid = bookingData.discountid,
                discountType = bookingData.discountType,
                discountApproved = bookingData.discountApproved,
                discountReason = bookingData.discountReason,
                isDisCountApproved = bookingData.isDisCountApproved,
                paymentMode = bookingData.paymentMode,
                patientRemarks = bookingData.patientRemarks,
                sessionCentreid = bookingData.sessionCentreid,
                centreId = bookingData.centreId,
                isCredit = bookingData.isCredit,
                rateId = bookingData.rateId,
                source = bookingData.source,
                labRemarks = bookingData.labRemarks,
                otherLabRefer = bookingData.otherLabRefer,
                OtherLabReferID = bookingData.OtherLabReferID,
                RefDoctor1 = bookingData.RefDoctor1,
                refDoctor2 = bookingData.refDoctor2,
                refID1 = bookingData.refID1,
                refID2 = bookingData.refID2,
                tempDOCID = bookingData.tempDOCID,
                tempDoctroName = bookingData.tempDoctroName,
                updateById = bookingData.updateById,
                updateDateTime = bookingData.updateDateTime,
                uploadDocument = bookingData.uploadDocument,
                invoiceNo = bookingData.invoiceNo,
                createdById = bookingData.createdById,
                salesExecutiveID = bookingData.salesExecutiveID,
                createdDateTime = bookingData.createdDateTime



            };
        }

        private async Task<ServiceStatusResponseModel> SaveBookingStatus(IEnumerable<tnx_BookingStatus> tnxBookingStatus, int patientId, int transactionId)
        {
            foreach (var bookingStatus in tnxBookingStatus)
            {
                var tnxbookingStatus = CreateBookingStatus(bookingStatus, patientId, transactionId);
                var tnxBooking = db.tnx_BookingStatus.Add(tnxbookingStatus);
                await db.SaveChangesAsync();

            }
            return new ServiceStatusResponseModel
            {
                Success = true,
                Data = tnxBookingStatus
            };

        }

        private tnx_BookingStatus CreateBookingStatus(tnx_BookingStatus bookingStatus, int patientId, int transactionId)
        {
            return new tnx_BookingStatus
            {
                id = bookingStatus.id,
                transactionId = transactionId,
                patientId = patientId,
                barcodeNo = bookingStatus.barcodeNo,
                status = bookingStatus.status,
                createdById = bookingStatus.createdById,
                createdDateTime = bookingStatus.createdDateTime,
                centreId = bookingStatus.centreId,
                roleId = bookingStatus.roleId,
                remarks = bookingStatus.remarks,
                testId = bookingStatus.testId,
                isActive = bookingStatus.isActive,
            };

        }

        private async Task<ServiceStatusResponseModel> SaveBookingItem(IEnumerable<tnx_BookingItem> tnxBookingItems, int patientId, string workOrderId, int transactionId)
        {
            foreach (var bookingItem in tnxBookingItems)
            {
                var tnxbookingitem = CreateBookingItem(bookingItem, patientId, workOrderId, transactionId);
                db.tnx_BookingItem.Add(tnxbookingitem);
                if (tnxbookingitem.isPackage == 1)
                {
                    var itemdetails = (from iom in db.ItemObservationMapping
                                       join im in db.itemMaster on iom.itemObservationId equals im.id
                                       where iom.itemId == 3
                                       join rtr in db.rateTypeWiseRateList on iom.itemObservationId equals rtr.itemid into rtrJoin
                                       from rtr in rtrJoin.DefaultIfEmpty() // Left Join
                                       join rtt in db.rateTypeTagging on rtr.rateTypeId equals rtt.rateTypeId into rttJoin
                                       from rtt in rttJoin.DefaultIfEmpty() // Left Join
                                       where rtt == null || rtt.centreId == 1 // Allow null rtt or filter by centreId
                                       select new
                                       {
                                           testCode = im.code,
                                           itemId = im.id,
                                           im.deptId,
                                           departmentName = "", // Empty string as per SQL
                                           investigationName = im.itemName,
                                           im.itemType,
                                           mrp = rtr != null ? rtr.mrp : (double?)null, // Explicit null check for rtr.mrp
                                           rate = rtr != null ? rtr.rate : (double?)null, // Explicit null check for rtr.rate
                                           discount = rtr != null ? rtr.discount : (double?)null, // Explicit null check for rtr.discount
                                           netAmount = rtr != null ? rtr.rate : (double?)0
                                       }).ToList();

                    if (itemdetails != null)
                    {
                        foreach (var item in itemdetails)
                        {
                            //if(item.rate== null || item.rate==0 || item.mrp == null || item.mrp == 0)
                            //{
                            //    return new ServiceStatusResponseModel
                            //    {
                            //        Success = false,
                            //        Message = "Rate not Available for " + item.investigationName
                            //
                            //    }; 
                            //}
                            var createPackageItemDetail = createPackageItemData(bookingItem, patientId, workOrderId, transactionId, item);
                            db.tnx_BookingItem.Add(createPackageItemDetail);
                        }
                    }

                }
                await db.SaveChangesAsync();

            }
            return new ServiceStatusResponseModel
            {
                Success = true,
                Data = tnxBookingItems
            };
        }

        private tnx_BookingItem CreateBookingItem(tnx_BookingItem bookingItem, int patientId, string workOrderId, int transactionId)
        {
            return new tnx_BookingItem
            {
                id = bookingItem.id,
                workOrderId = workOrderId,
                transactionId = transactionId,
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
        private tnx_BookingItem createPackageItemData(tnx_BookingItem bookingItem, int patientId, string workOrderId, int transactionId, dynamic packageItem)
        {
            return new tnx_BookingItem
            {
                id = bookingItem.id,
                workOrderId = workOrderId,
                transactionId = transactionId,
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
        private void UpdateBookingPatient(tnx_BookingPatient tnxBookingPatients, tnx_BookingPatient tnxBookingPatient)
        {
            tnxBookingPatients.title_id = tnxBookingPatient.title_id;
            tnxBookingPatients.name = tnxBookingPatient.name;
            tnxBookingPatients.gender = tnxBookingPatient.gender;
            tnxBookingPatients.ageTotal = tnxBookingPatient.ageTotal;
            tnxBookingPatients.ageDays = tnxBookingPatient.ageDays;
            tnxBookingPatients.ageMonth = tnxBookingPatient.ageMonth;
            tnxBookingPatients.ageYear = tnxBookingPatient.ageYear;
            tnxBookingPatients.dob = tnxBookingPatient.dob;
            tnxBookingPatients.isActualDOB = tnxBookingPatient.isActualDOB;
            tnxBookingPatients.emailId = tnxBookingPatient.emailId;
            tnxBookingPatients.mobileNo = tnxBookingPatient.mobileNo;
            tnxBookingPatients.address = tnxBookingPatient.address;
            tnxBookingPatients.pinCode = tnxBookingPatient.pinCode;
            tnxBookingPatients.cityId = tnxBookingPatient.cityId;
            tnxBookingPatients.centreId = tnxBookingPatient.centreId;
            tnxBookingPatients.areaId = tnxBookingPatient.areaId;
            tnxBookingPatients.stateId = tnxBookingPatient.stateId;
            tnxBookingPatients.districtId = tnxBookingPatient.districtId;
            tnxBookingPatients.countryId = tnxBookingPatient.countryId;
            tnxBookingPatients.visitCount = tnxBookingPatient.visitCount;
            tnxBookingPatients.remarks = tnxBookingPatient.remarks;
            tnxBookingPatients.documentId = tnxBookingPatient.documentId;
            tnxBookingPatients.documnetnumber = tnxBookingPatient.documnetnumber;
            tnxBookingPatients.updateDateTime = tnxBookingPatient.updateDateTime;
            tnxBookingPatients.updateById = tnxBookingPatient.updateById;
            tnxBookingPatients.password = tnxBookingPatient.password;

        }

        private async Task<ServiceStatusResponseModel> UpdateBooking(IEnumerable<tnx_Booking> tnxBookings, int patientId, string workOrderId)
        {
            foreach (var bookings in tnxBookings)
            {
                if (bookings.transactionId != 0)
                {
                    var tnxbookingData = await db.tnx_Booking.FirstOrDefaultAsync(booking => booking.transactionId == bookings.transactionId);
                    if (tnxbookingData != null)
                    {
                        UpdateBookingDetail(tnxbookingData, bookings);
                        var tnxbooking = db.tnx_Booking.Update(tnxbookingData);
                        await db.SaveChangesAsync();
                        var id = tnxbooking.Entity.transactionId;
                    }
                }
                else
                {

                    var tnxbookingData = CreateBookingDetail(bookings, patientId, workOrderId);
                    var tnxbooking = db.tnx_Booking.Add(tnxbookingData);
                    await db.SaveChangesAsync();
                }

            }
            return new ServiceStatusResponseModel
            {
                Success = true,
                Data = tnxBookings
            };
        }
        private void UpdateBookingDetail(tnx_Booking tnxBooking, tnx_Booking bookingData)
        {
            tnxBooking.title_id = bookingData.title_id;
            tnxBooking.name = bookingData.name;
            tnxBooking.gender = bookingData.gender;
            tnxBooking.dob = bookingData.dob;
            tnxBooking.ageYear = bookingData.ageYear;
            tnxBooking.ageMonth = bookingData.ageMonth;
            tnxBooking.ageDay = bookingData.ageDay;
            tnxBooking.totalAge = bookingData.totalAge;
            tnxBooking.clientCode = bookingData.clientCode;
            tnxBooking.mobileNo = bookingData.mobileNo;
            tnxBooking.mrp = bookingData.mrp;
            tnxBooking.grossAmount = bookingData.grossAmount;
            tnxBooking.discount = bookingData.discount;
            tnxBooking.netAmount = bookingData.netAmount;
            tnxBooking.paidAmount = bookingData.paidAmount;
            tnxBooking.discountid = bookingData.discountid;
            tnxBooking.discountType = bookingData.discountType;
            tnxBooking.discountApproved = bookingData.discountApproved;
            tnxBooking.discountReason = bookingData.discountReason;
            tnxBooking.isDisCountApproved = bookingData.isDisCountApproved;
            tnxBooking.paymentMode = bookingData.paymentMode;
            tnxBooking.patientRemarks = bookingData.patientRemarks;
            tnxBooking.sessionCentreid = bookingData.sessionCentreid;
            tnxBooking.centreId = bookingData.centreId;
            tnxBooking.isCredit = bookingData.isCredit;
            tnxBooking.rateId = bookingData.rateId;
            tnxBooking.source = bookingData.source;
            tnxBooking.labRemarks = bookingData.labRemarks;
            tnxBooking.otherLabRefer = bookingData.otherLabRefer;
            tnxBooking.OtherLabReferID = bookingData.OtherLabReferID;
            tnxBooking.RefDoctor1 = bookingData.RefDoctor1;
            tnxBooking.refDoctor2 = bookingData.refDoctor2;
            tnxBooking.refID1 = bookingData.refID1;
            tnxBooking.refID2 = bookingData.refID2;
            tnxBooking.tempDOCID = bookingData.tempDOCID;
            tnxBooking.tempDoctroName = bookingData.tempDoctroName;
            tnxBooking.updateById = bookingData.updateById;
            tnxBooking.updateDateTime = bookingData.updateDateTime;
            tnxBooking.uploadDocument = bookingData.uploadDocument;
            tnxBooking.invoiceNo = bookingData.invoiceNo;
            tnxBooking.salesExecutiveID = bookingData.salesExecutiveID;
        }

        private async Task<ServiceStatusResponseModel> UpdateBookingStatus(IEnumerable<tnx_BookingStatus> tnxBookingStatus, int patientId, int transactionId)
        {
            foreach (var bookingStatus in tnxBookingStatus)
            {
                if (bookingStatus.id != 0)
                {
                    var tnxbookingStatusData = await db.tnx_BookingStatus.FirstOrDefaultAsync(booking => booking.id == bookingStatus.id);
                    if (tnxbookingStatusData != null)
                    {
                        UpdateBookingStatusDetail(tnxbookingStatusData, bookingStatus);
                        var tnxbooking = db.tnx_BookingStatus.Update(tnxbookingStatusData);
                        await db.SaveChangesAsync();
                        var id = tnxbooking.Entity.id;
                    }
                }
                else
                {
                    var tnxbookingStatusData = CreateBookingStatus(bookingStatus, patientId, transactionId);
                    var tnxbooking = db.tnx_BookingStatus.Add(tnxbookingStatusData);
                    await db.SaveChangesAsync();
                }

            }
            return new ServiceStatusResponseModel
            {
                Success = true,
                Data = tnxBookingStatus
            };
        }
        private void UpdateBookingStatusDetail(tnx_BookingStatus tnxBookingStatus, tnx_BookingStatus bookingStatus)
        {
            tnxBookingStatus.transactionId = bookingStatus.transactionId;
            tnxBookingStatus.patientId = bookingStatus.patientId;
            tnxBookingStatus.barcodeNo = bookingStatus.barcodeNo;
            tnxBookingStatus.status = bookingStatus.status;
            tnxBookingStatus.centreId = bookingStatus.centreId;
            tnxBookingStatus.roleId = bookingStatus.roleId;
            tnxBookingStatus.remarks = bookingStatus.remarks;
            tnxBookingStatus.testId = bookingStatus.testId;
            tnxBookingStatus.isActive = bookingStatus.isActive;
            tnxBookingStatus.updateById = bookingStatus.updateById;
            tnxBookingStatus.updateDateTime = bookingStatus.updateDateTime;
        }


        private async Task<ServiceStatusResponseModel> UpdateBookingItem(IEnumerable<tnx_BookingItem> tnxBookingItems, int patientId, string workOrderId, int transactionId)
        {
            foreach (var BookingItems in tnxBookingItems)
            {
                if (BookingItems.id != 0)
                {
                    var tnxBookingItemData = await db.tnx_BookingItem.FirstOrDefaultAsync(bookingItem => bookingItem.id == BookingItems.id);
                    if (tnxBookingItemData != null)
                    {
                        updateBookingItemDetail(tnxBookingItemData, BookingItems);
                        var tnxBookingItem = db.tnx_BookingItem.Update(tnxBookingItemData);
                        await db.SaveChangesAsync();

                    }

                }
                else
                {
                    var tnxbookingitem = CreateBookingItem(BookingItems, patientId, workOrderId, transactionId);
                    var tnxBooking = db.tnx_BookingItem.Add(tnxbookingitem);
                    if (tnxbookingitem.isPackage == 1)
                    {
                        //var itemdetails = _MySql_Procedure_Services.GetPackageItem(BookingItems.itemId, BookingItems.centreId);
                        var itemdetails= (from iom in db.ItemObservationMapping
                                                      join im in db.itemMaster on iom.itemObservationId equals im.id
                                                      where iom.itemId == 3
                                                      join rtr in db.rateTypeWiseRateList on iom.itemObservationId equals rtr.itemid into rtrJoin
                                                      from rtr in rtrJoin.DefaultIfEmpty() // Left Join
                                                      join rtt in db.rateTypeTagging on rtr.rateTypeId equals rtt.rateTypeId into rttJoin
                                                      from rtt in rttJoin.DefaultIfEmpty() // Left Join
                                                      where rtt.centreId == 1 // Filter for centreId
                                                      select new
                                                      {
                                                          testcode = im.code,
                                                          itemid = im.id,
                                                          im.deptId,
                                                          departmentname = "", // Empty string as per SQL
                                                          investigationName = im.itemName,
                                                          im.itemType,
                                                          rtr.mrp,
                                                          rtr.rate,
                                                          rtr.discount,
                                                          netAmount = rtr.rate // netAmount equivalent to rtr.Rate in SQL
                                                      }).ToList();
                        if (itemdetails != null)
                        {
                            foreach (var item in itemdetails)
                            {
                                var createPackageItemDetail = createPackageItemData(BookingItems, patientId, workOrderId, transactionId, item);
                                db.tnx_BookingItem.Add(createPackageItemDetail);
                            }
                        }

                    }
                    await db.SaveChangesAsync();
                }

            }
            return new ServiceStatusResponseModel
            {
                Success = true,
                Data = tnxBookingItems
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

        byte[] Itnx_BookingPatientServices.GetPatientReceipt(string workorderid)
        {
            var Receiptdata = (from tb in db.tnx_Booking
                               join tbp in db.tnx_BookingPatient on tb.patientId equals tbp.patientId
                               join tbi in db.tnx_BookingItem on tb.workOrderId equals tbi.workOrderId
                               join tm in db.titleMaster on tb.title_id equals tm.id
                               join cm in db.centreMaster on tb.centreId equals cm.centreId
                               where tb.workOrderId == workorderid
                               select new
                               {
                                   tb.workOrderId, BookingDate = tb.bookingDate.ToString("yyyy-MMM-dd hh:mm tt"),tbp.address,tb.patientId,
                                   tm.title,tb.name,Age= tb.ageYear+ "Y " + tb.ageMonth + "M "+ tb.ageDay +"D" , tb.mrp, tb.grossAmount, tb.discount, tb.netAmount,
                                   tb.paidAmount, cm.companyName, CentreAddress = cm.address, CentreMobile = cm.mobileNo, tbp.mobileNo, tb.gender, tbi.barcodeNo,
                                   tbi.departmentName, tbi.investigationName, ItemMRP = tbi.mrp, ItemRate = tbi.rate, ItemDiscount = tbi.discount, ItemNetAmount = tbi.netAmount }).ToList();

            if (Receiptdata.Count>0)
            {
                QuestPDF.Settings.License = LicenseType.Community;
                var image1 = _configuration["FileBase64:CompanyLogo1"];
                var image1Bytes = Convert.FromBase64String(image1.Split(',')[1]);
                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);

                        page.MarginTop(0.5f, Unit.Centimetre);
                        page.Margin(0.5f, Unit.Centimetre);

                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontFamily("Arial"));
                        page.DefaultTextStyle(x => x.FontSize(20));

                        page.Header()
                        .Column(column =>
                        {
                            // Header row with GST, Company Name, and Mobile No.
                            column.Item()
                             .Table(table =>
                             {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                    });
                                    table.Cell().RowSpan(3).ColumnSpan(2).AlignBottom().Image(image1Bytes);
                                    table.Cell().ColumnSpan(2).AlignCenter().Text("").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().RowSpan(3).ColumnSpan(2).AlignBottom().Image(image1Bytes);
                                 table.Cell().ColumnSpan(2).AlignCenter().Text( Receiptdata[0].companyName ).Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(2).AlignCenter().Text(Receiptdata[0].CentreMobile).Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(6).AlignCenter().Text("MONEY RECEIPT / BILL").Style(TextStyle.Default.FontSize(10).Bold().Underline());
                                    table.Cell().ColumnSpan(6).BorderBottom(1.0f, Unit.Point);
                                });

                            // Product table header
                            column.Item()
                                .Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                    });

                                    table.Cell().Text("Patient Name").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(2).Text(Receiptdata[0].name).Style(TextStyle.Default.FontSize(10));
                                    table.Cell().Text("Age/Gender").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(2).Text( Receiptdata[0].Age.ToString() ).Style(TextStyle.Default.FontSize(10));
                                    table.Cell().Text("Address").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(2).Text(Receiptdata[0].address).Style(TextStyle.Default.FontSize(10));
                                    table.Cell().Text("Email Id").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(2).Text("").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().Text("Mobile").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(2).Text(Receiptdata[0].mobileNo).Style(TextStyle.Default.FontSize(10));
                                    table.Cell().Text("Centre").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(2).Text(Receiptdata[0].companyName).Style(TextStyle.Default.FontSize(10));
                                    //table.Cell().Text("Panel").Style(TextStyle.Default.FontSize(10));
                                    //table.Cell().ColumnSpan(2).Text("Imarsar testing 1").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().Text("Visit No").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(2).Text(Receiptdata[0].workOrderId).Style(TextStyle.Default.FontSize(10));
                                    table.Cell().Text("UHID").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(2).Text(Receiptdata[0].patientId.ToString()).Style(TextStyle.Default.FontSize(10));
                                    table.Cell().Text("Registration Date").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(2).Text(Receiptdata[0].BookingDate).Style(TextStyle.Default.FontSize(10));
                                    table.Cell().Text("Barcode No").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(2).Text(Receiptdata[0].barcodeNo).Style(TextStyle.Default.FontSize(10));
                                   // table.Cell().Text("").Style(TextStyle.Default.FontSize(10));
                                   // table.Cell().ColumnSpan(2).Text("").Style(TextStyle.Default.FontSize(10));
                                });
                            column.Item()
                                .Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.ConstantColumn(1.0f, Unit.Centimetre);
                                        columns.ConstantColumn(8.0f, Unit.Centimetre);
                                        columns.ConstantColumn(4.5f, Unit.Centimetre);
                                        columns.ConstantColumn(2.5f, Unit.Centimetre);
                                        columns.ConstantColumn(2.0f, Unit.Centimetre);
                                        columns.ConstantColumn(1.5f, Unit.Centimetre);
                                    });
                                    table.Cell().BorderBottom(0.8f, Unit.Point).BorderTop(0.8f, Unit.Point).Text("#").Style(TextStyle.Default.FontSize(10).Bold());
                                    table.Cell().BorderBottom(0.8f, Unit.Point).BorderTop(0.8f, Unit.Point).Text("Service Name").Style(TextStyle.Default.FontSize(10).Bold());
                                    table.Cell().BorderBottom(0.8f, Unit.Point).BorderTop(0.8f, Unit.Point).Text("DepartMent").Style(TextStyle.Default.FontSize(10).Bold());
                                    table.Cell().BorderBottom(0.8f, Unit.Point).BorderTop(0.8f, Unit.Point).Text("Rate").Style(TextStyle.Default.FontSize(10).Bold());
                                    table.Cell().BorderBottom(0.8f, Unit.Point).BorderTop(0.8f, Unit.Point).Text("Discount").Style(TextStyle.Default.FontSize(10).Bold());
                                    table.Cell().BorderBottom(0.8f, Unit.Point).BorderTop(0.8f, Unit.Point).Text("Total").Style(TextStyle.Default.FontSize(10).Bold());

                                    int rowNumber = 1; // Row counter for serial numbers
                                    foreach (var item in Receiptdata)
                                    {
                                        table.Cell().Text(rowNumber.ToString()).Style(TextStyle.Default.FontSize(10)); // Serial number
                                        table.Cell().Text(item.investigationName).Style(TextStyle.Default.FontSize(10)); // Service Name
                                        table.Cell().Text(item.departmentName).Style(TextStyle.Default.FontSize(10));   // Department
                                        table.Cell().Text(item.ItemRate.ToString()).Style(TextStyle.Default.FontSize(10)); // Rate
                                        table.Cell().Text(item.ItemDiscount.ToString()).Style(TextStyle.Default.FontSize(10)); // Discount
                                        table.Cell().Text(item.ItemNetAmount.ToString()).Style(TextStyle.Default.FontSize(10)); // Total
                                        rowNumber++; // Increment the serial number
                                    }

                                    // table.Cell().ColumnSpan(4).BorderTop(0.8f,Unit.Point).BorderBottom(0.8f, Unit.Point).Text("Settelment Table").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(3).Element(innerTable =>
                                 {
                                        innerTable.Table(subTable =>
                                        {
                                            subTable.ColumnsDefinition(columns =>
                                            {
                                                columns.RelativeColumn();
                                                columns.RelativeColumn();
                                                columns.RelativeColumn();
                                                columns.RelativeColumn();
                                                columns.RelativeColumn();
                                                columns.RelativeColumn();
                                            });

                                            // Header for the new embedded table
                                            subTable.Cell().BorderBottom(0.8f, Unit.Point).BorderTop(0.8f, Unit.Point).Text("Settelment").Style(TextStyle.Default.FontSize(10).Bold());
                                            subTable.Cell().BorderBottom(0.8f, Unit.Point).BorderTop(0.8f, Unit.Point).Text("Payment").Style(TextStyle.Default.FontSize(10).Bold());
                                            subTable.Cell().BorderBottom(0.8f, Unit.Point).BorderTop(0.8f, Unit.Point).Text("Reciept No.").Style(TextStyle.Default.FontSize(10).Bold());
                                            subTable.Cell().BorderBottom(0.8f, Unit.Point).BorderTop(0.8f, Unit.Point).Text("Mode").Style(TextStyle.Default.FontSize(10).Bold());
                                            subTable.Cell().BorderBottom(0.8f, Unit.Point).BorderTop(0.8f, Unit.Point).Text("Amount").Style(TextStyle.Default.FontSize(10).Bold());
                                            subTable.Cell().BorderBottom(0.8f, Unit.Point).BorderTop(0.8f, Unit.Point).Text("Received By").Style(TextStyle.Default.FontSize(10).Bold());

                                            // Example data rows
                                            for (int i = 0; i < 2; i++) // Loop for adding rows
                                            {
                                                subTable.Cell().Text("Data " + (i + 1)).Style(TextStyle.Default.FontSize(10));
                                                subTable.Cell().Text("Data " + (i + 1)).Style(TextStyle.Default.FontSize(10));
                                                subTable.Cell().Text("Data " + (i + 1)).Style(TextStyle.Default.FontSize(10));
                                                subTable.Cell().Text("Data " + (i + 1)).Style(TextStyle.Default.FontSize(10));
                                                subTable.Cell().Text("Data " + (i + 1)).Style(TextStyle.Default.FontSize(10));
                                                subTable.Cell().Text("Data " + (i + 1)).Style(TextStyle.Default.FontSize(10));
                                            }
                                        });
                                    });

                                    table.Cell().ColumnSpan(3).BorderTop(0.8f, Unit.Point).Text("").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(4).BorderTop(0.8f, Unit.Point).Text("").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(2).BorderTop(0.8f, Unit.Point).Text("Gross Amount" + Receiptdata[0].grossAmount).AlignRight().Style(TextStyle.Default.FontSize(10));

                                    table.Cell().ColumnSpan(4).Text("").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(2).Text("Discount: "+ Receiptdata[0].discount).AlignRight().Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(4).Text("").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(2).Text("Net Amount: "+ Receiptdata[0].netAmount).AlignRight().Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(4).BorderBottom(0.8f, Unit.Point).Text("Recieved with Thanks: "+ AmountToWord((decimal)Receiptdata[0].paidAmount)).Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(2).BorderBottom(0.8f, Unit.Point).Text("Paid Amount: "+ Receiptdata[0].paidAmount).AlignRight().Style(TextStyle.Default.FontSize(10));
                                });

                        });
                        page.Footer()
                           .Column(column =>
                           {
                               column.Item()
                                   .AlignCenter()
                                   .Text(text =>
                                   {
                                       text.DefaultTextStyle(x => x.FontSize(8));
                                       text.CurrentPageNumber();
                                       text.Span(" of ");
                                       text.TotalPages();
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

        public byte[] GetPatientMRPBill(string workorderid)
        {
            var Receiptdata = (from tb in db.tnx_Booking
                               join tbp in db.tnx_BookingPatient on tb.patientId equals tbp.patientId
                               join tbi in db.tnx_BookingItem on tb.workOrderId equals tbi.workOrderId
                               join tm in db.titleMaster on tb.title_id equals tm.id
                               join cm in db.centreMaster on tb.centreId equals cm.centreId
                               where tb.workOrderId == workorderid
                               select new
                               {
                                   tb.workOrderId,BookingDate = tb.bookingDate.ToString("yyyy-MMM-dd hh:mm tt"),DeliveryDate= tbi.deliveryDate.ToString("yyyy-MMM-dd hh:mm tt"),tbp.address,
                                   tb.patientId,tm.title,tb.name,Age = tb.ageYear + "Y " + tb.ageMonth + "M " + tb.ageDay + "D",tb.mrp,tb.grossAmount,tb.discount,tb.netAmount,tb.paidAmount,
                                   cm.companyName,CentreAddress = cm.address,CentreMobile = cm.mobileNo,tbp.mobileNo,tb.gender,tbi.barcodeNo,tbi.departmentName,tbi.investigationName,
                                   ItemMRP = tbi.mrp,ItemRate = tbi.rate,ItemDiscount = tbi.discount,ItemNetAmount = tbi.netAmount }).ToList();

            if (Receiptdata.Count > 0)
            {
                QuestPDF.Settings.License = LicenseType.Community;
                var image1 = _configuration["FileBase64:CompanyLogo1"];
                var image1Bytes = Convert.FromBase64String(image1.Split(',')[1]);
                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);

                        page.MarginTop(0.5f, Unit.Centimetre);
                        page.Margin(0.5f, Unit.Centimetre);

                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontFamily("Arial"));
                        page.DefaultTextStyle(x => x.FontSize(20));

                        var data = "";

                        page.Header()
                        .Column(column =>
                        {
                            // Header row with GST, Company Name, and Mobile No.
                            column.Item()
                                 .Table(table =>
                                 {
                                     table.ColumnsDefinition(columns =>
                                     {
                                         columns.RelativeColumn();
                                         columns.RelativeColumn();
                                         columns.RelativeColumn();
                                         columns.RelativeColumn();
                                         columns.RelativeColumn();
                                         columns.RelativeColumn();
                                     });
                                     table.Cell().RowSpan(3).ColumnSpan(2).AlignBottom().Image(image1Bytes);
                                     table.Cell().ColumnSpan(2).AlignCenter().Text("").Style(TextStyle.Default.FontSize(10));
                                     table.Cell().RowSpan(3).ColumnSpan(2).AlignBottom().Image(image1Bytes);
                                     table.Cell().ColumnSpan(2).AlignCenter().Text(Receiptdata[0].companyName).Style(TextStyle.Default.FontSize(10));
                                     table.Cell().ColumnSpan(2).AlignCenter().Text(Receiptdata[0].CentreMobile).Style(TextStyle.Default.FontSize(10));
                                     table.Cell().ColumnSpan(6).AlignCenter().Text("MONEY RECEIPT / BILL").Style(TextStyle.Default.FontSize(10).Bold().Underline());
                                     table.Cell().ColumnSpan(6).BorderBottom(1.0f, Unit.Point);
                                 });

                            // Product table header
                            column.Item()
                                .Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                    });
                                    table.Cell().Text("Patient Name").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(2).Text(Receiptdata[0].name).Style(TextStyle.Default.FontSize(10));
                                    table.Cell().Text("Age/Gender").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(2).Text(Receiptdata[0].Age.ToString()).Style(TextStyle.Default.FontSize(10));
                                    table.Cell().Text("Address").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(2).Text(Receiptdata[0].address).Style(TextStyle.Default.FontSize(10));
                                    table.Cell().Text("Email Id").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(2).Text("").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().Text("Mobile").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(2).Text(Receiptdata[0].mobileNo).Style(TextStyle.Default.FontSize(10));
                                    table.Cell().Text("Centre").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(2).Text(Receiptdata[0].companyName).Style(TextStyle.Default.FontSize(10));
                                    table.Cell().Text("Visit No").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(2).Text(Receiptdata[0].workOrderId).Style(TextStyle.Default.FontSize(10));
                                    table.Cell().Text("UHID").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(2).Text(Receiptdata[0].patientId.ToString()).Style(TextStyle.Default.FontSize(10));
                                    table.Cell().Text("Registration Date").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(2).Text(Receiptdata[0].BookingDate).Style(TextStyle.Default.FontSize(10));
                                    table.Cell().Text("Barcode No").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(2).Text(Receiptdata[0].barcodeNo).Style(TextStyle.Default.FontSize(10));
                                });
                            column.Item()
                                .Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.ConstantColumn(1.0f, Unit.Centimetre);
                                        columns.ConstantColumn(8.5f, Unit.Centimetre);
                                        columns.ConstantColumn(3.0f, Unit.Centimetre);
                                        columns.ConstantColumn(4.5f, Unit.Centimetre);
                                        columns.ConstantColumn(2.5f, Unit.Centimetre);
                                    });
                                    table.Cell().BorderBottom(0.8f, Unit.Point).BorderTop(0.8f, Unit.Point).Text("#").Style(TextStyle.Default.FontSize(10).Bold());
                                    table.Cell().BorderBottom(0.8f, Unit.Point).BorderTop(0.8f, Unit.Point).Text("Service Name").Style(TextStyle.Default.FontSize(10).Bold());
                                    table.Cell().BorderBottom(0.8f, Unit.Point).BorderTop(0.8f, Unit.Point).Text("Department").Style(TextStyle.Default.FontSize(10).Bold());
                                    table.Cell().BorderBottom(0.8f, Unit.Point).BorderTop(0.8f, Unit.Point).Text("Delivery Date").Style(TextStyle.Default.FontSize(10).Bold());
                                    table.Cell().BorderBottom(0.8f, Unit.Point).BorderTop(0.8f, Unit.Point).Text("Total").Style(TextStyle.Default.FontSize(10).Bold()).AlignRight();

                                    int rowNumber = 1; // Row counter for serial numbers
                                    foreach (var item in Receiptdata)
                                    {
                                        table.Cell().Text(rowNumber.ToString()).Style(TextStyle.Default.FontSize(10)); // Serial number
                                        table.Cell().Text(item.investigationName).Style(TextStyle.Default.FontSize(10)); // Service Name
                                        table.Cell().Text(item.departmentName).Style(TextStyle.Default.FontSize(10));   // Department
                                        table.Cell().Text(item.DeliveryDate).Style(TextStyle.Default.FontSize(10));   // DeliveryDate
                                        table.Cell().Text(item.ItemMRP.ToString()).Style(TextStyle.Default.FontSize(10)).AlignRight(); // Total
                                        rowNumber++; // Increment the serial number
                                    }
                                    table.Cell().ColumnSpan(3).BorderTop(0.8f, Unit.Point).Text("").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(2).BorderTop(0.8f, Unit.Point).Text("Gross Amount: " + Receiptdata[0].mrp).AlignRight().Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(3).Text("").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(2).Text("Net Amount: "+ Receiptdata[0].mrp).AlignRight().Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(5).BorderBottom(1.0f, Unit.Point).Text("").Style(TextStyle.Default.FontSize(6));
                                    table.Cell().ColumnSpan(5).Text("Recieved with Thanks: " + AmountToWord((decimal)Receiptdata[0].mrp).ToString()).Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(5).Text("#Report delivery time on delivery date: Evening 07:00 PM to 07:45 PM. and 04:00 PM to 05:00 PM Sundays").Style(TextStyle.Default.FontSize(10).Bold());
                                    table.Cell().ColumnSpan(5).Text("This is a computer generated documnent,does not require any signature.").Style(TextStyle.Default.FontSize(10));
                                });

                        });
                        page.Footer()
                           .Column(column =>
                           {
                               column.Item()
                                   .AlignCenter()
                                   .Text(text =>
                                   {
                                       text.DefaultTextStyle(x => x.FontSize(8));
                                       text.CurrentPageNumber();
                                       text.Span(" of ");
                                       text.TotalPages();
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



        private static readonly string[] ones = new string[] { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
        private static readonly string[] tens = new string[] { "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
        private static readonly string[] thousands = new string[] { "", "Thousand", "Million", "Billion" };

        // Converts a numeric value to words
        public static string AmountToWord(decimal number)
        {
            if (number == 0)
                return "Zero only";

            string words = "";
            int partCounter = 0;
            while (number > 0)
            {
                int part = (int)(number % 1000);
                if (part > 0)
                {
                    string partWords = ConvertPart(part);
                    words = partWords + (thousands[partCounter] != "" ? " " + thousands[partCounter] : "") + " " + words;
                }
                number /= 1000;
                partCounter++;
            }

            return words.Trim() + " only";
        }

        // Helper method to convert each part (less than 1000) to words
        private static string ConvertPart(int number)
        {
            if (number == 0) return "";

            if (number < 20)
                return ones[number];

            if (number < 100)
                return tens[number / 10] + (number % 10 != 0 ? " " + ones[number % 10] : "");

            return ones[number / 100] + " Hundred" + (number % 100 != 0 ? " " + ConvertPart(number % 100) : "");
        }
    }
}
