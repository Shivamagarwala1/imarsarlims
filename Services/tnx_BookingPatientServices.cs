using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;

namespace iMARSARLIMS.Services
{
    public class tnx_BookingPatientServices : Itnx_BookingPatientServices
    {
        private readonly ContextClass db;
        private readonly MySql_Function_Services _MySql_Function_Services;
        private readonly MySql_Procedure_Services _MySql_Procedure_Services;

        public int transactionId;

        public tnx_BookingPatientServices(ContextClass context, ILogger<BaseController<tnx_BookingPatient>> logger, MySql_Function_Services MySql_Function_Services, MySql_Procedure_Services mySql_Procedure_Services = null)
        {

            db = context;
            this._MySql_Function_Services = MySql_Function_Services;
            this._MySql_Procedure_Services = mySql_Procedure_Services;
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
                title = tnxBookingPatient.title,
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
                landLineNo = tnxBookingPatient.landLineNo,
                address = tnxBookingPatient.address,
                pinCode = tnxBookingPatient.pinCode,
                cityName = tnxBookingPatient.cityName,
                cityId = tnxBookingPatient.cityId,
                centreId = tnxBookingPatient.centreId,
                area = tnxBookingPatient.area,
                areaId = tnxBookingPatient.areaId,
                state = tnxBookingPatient.state,
                district = tnxBookingPatient.district,
                country = tnxBookingPatient.country,
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
                title = bookingData.title,
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
                captureImg = bookingData.captureImg,
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
            tnxBookingPatients.title = tnxBookingPatient.title;
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
            tnxBookingPatients.landLineNo = tnxBookingPatient.landLineNo;
            tnxBookingPatients.address = tnxBookingPatient.address;
            tnxBookingPatients.pinCode = tnxBookingPatient.pinCode;
            tnxBookingPatients.cityName = tnxBookingPatient.cityName;
            tnxBookingPatients.cityId = tnxBookingPatient.cityId;
            tnxBookingPatients.centreId = tnxBookingPatient.centreId;
            tnxBookingPatients.area = tnxBookingPatient.area;
            tnxBookingPatients.areaId = tnxBookingPatient.areaId;
            tnxBookingPatients.state = tnxBookingPatient.state;
            tnxBookingPatients.district = tnxBookingPatient.district;
            tnxBookingPatients.country = tnxBookingPatient.country;
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
            tnxBooking.title = bookingData.title;
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
            tnxBooking.captureImg = bookingData.captureImg;
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


    }
}
