using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.Extensions.Logging;
using Microsoft.OData.UriParser;
using Org.BouncyCastle.Asn1.X509;
using System.Reflection;
using System.Runtime.CompilerServices;
using System;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Request_Model;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Services
{
    public class centreMasterServices : IcentreMasterServices
    {
        private readonly ContextClass db;

        public centreMasterServices(ContextClass context, ILogger<BaseController<empMaster>> logger)
        {
            db = context;
        }
        async Task<ServiceStatusResponseModel> IcentreMasterServices.SaveCentreDetail(centreMaster centremaster)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    if (centremaster.centrecode == "")
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "CentreCode Can't Be blank"
                        };
                    }
                    else
                    {
                        var exists = await db.centreMaster.AnyAsync(cm => cm.centrecode == centremaster.centrecode);
                        if (exists)
                        {
                            return new ServiceStatusResponseModel
                            {
                                Success = false,
                                Message = "Duplicate CentreCode"
                            };
                        }
                    }
                    if (centremaster.id == 0)
                    {
                        var CentreMasterData = CreateCentreDetails(centremaster);
                        var CentreData = await db.centreMaster.AddAsync(CentreMasterData);
                        await db.SaveChangesAsync();
                        var centreId = CentreData.Entity.id;
                        if (centremaster.processingLab == 0)
                        {
                            var Centre = await db.centreMaster.FirstOrDefaultAsync(cm => cm.id == centreId);
                            if (Centre != null)
                            {
                                Centre.processingLab = centreId;
                                await db.SaveChangesAsync();
                            }
                        }
                        if (centremaster.patientRate == 0)
                        {
                            var ratetypemaster = CreateRateTypeMaster(centremaster.companyName, centreId, (int)centremaster.createdById, centremaster.createdDateTime);
                            var ratetypedata = db.rateTypeMaster.Add(ratetypemaster);
                            await db.SaveChangesAsync();
                            var rateTypeId = ratetypedata.Entity.id;
                            var ratetypeTagging = CreateRateTypeTagging(rateTypeId, centreId);
                            db.rateTypeTagging.Add(ratetypeTagging);
                            await db.SaveChangesAsync();
                        }
                        else
                        {
                            var rateTypeId = centremaster.patientRate;
                            var ratetypeTagging = CreateRateTypeTagging(rateTypeId, centreId);
                            db.rateTypeTagging.Add(ratetypeTagging);
                            await db.SaveChangesAsync();
                        }
                        if (centremaster.centretype == "Franchisee" || centremaster.centretype == "SubFranchisee")
                        {
                            var roleId = 0;
                            if (centremaster.centretype == "Franchisee")
                                roleId = 2;
                            if (centremaster.centretype == "SubFranchisee")
                                roleId = 3;
                            var EmployeeRegData = CreateEmployee(centremaster, centreId, roleId);
                            var EmployeeData = db.empMaster.Add(EmployeeRegData);
                            await db.SaveChangesAsync();
                            var employeeId = EmployeeData.Entity.id;
                            var centreAccess = SaveEmpCentreAccess(employeeId, centreId, (int)centremaster.createdById, centremaster.createdDateTime);
                            db.empCenterAccess.Add(centreAccess);
                            await db.SaveChangesAsync();
                            var RoleAccess = SaveEmpRoleAccess(employeeId, roleId, (int)centremaster.createdById, centremaster.createdDateTime);
                            db.empRoleAccess.Add(RoleAccess);
                            await db.SaveChangesAsync();
                        }
                    }


                    else
                    {
                        var CentreMaster = db.centreMaster.FirstOrDefault(cm => cm.id == centremaster.id);
                        if (CentreMaster != null)
                        {
                            UpdateCentreDetails(CentreMaster, centremaster);
                        }
                        var CentreMasterData = db.centreMaster.Update(CentreMaster);
                        await db.SaveChangesAsync();
                        var centreId = CentreMasterData.Entity.id;
                        if (centremaster.centretype == "Franchisee" || centremaster.centretype == "SubFranchisee")
                        {
                            //  var roleId = 0;
                            //  if (centremaster.centretype == "Franchisee")
                            //      roleId = 2;
                            //  if (centremaster.centretype == "SubFranchisee")
                            //      roleId = 3;
                            //  var EmployeeRegData = CreateEmployee(centremaster, centreId, roleId);
                            //  var EmployeeData = db.empMaster.Add(EmployeeRegData);
                            //  await db.SaveChangesAsync();
                            //  var employeeId = EmployeeData.Entity.id;
                            //  var centreAccess = SaveEmpCentreAccess(employeeId, centreId, (int)centremaster.createdById, centremaster.createdDateTime);
                            //  var RoleAccess = SaveEmpRoleAccess(employeeId, roleId, (int)centremaster.createdById, centremaster.createdDateTime);

                        }

                    }
                    await transaction.CommitAsync();

                    var result = db.centreMaster.ToList();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Data = result
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
        private centreMaster CreateCentreDetails(centreMaster centremaster)
        {
            return new centreMaster
            {
                id = centremaster.id,
                centretype = centremaster.centretype,
                centrecode = centremaster.centrecode,
                companyName = centremaster.companyName,
                mobileNo = centremaster.mobileNo,
                landline = centremaster.landline,
                address = centremaster.address,
                pinCode = centremaster.pinCode,
                email = centremaster.email,
                ownerName = centremaster.ownerName,
                proId = centremaster.proId,
                reportEmail = centremaster.reportEmail,
                parentCentreID = centremaster.parentCentreID,
                processingLab = centremaster.processingLab,
                creditLimt = centremaster.creditLimt,
                allowDueReport = centremaster.allowDueReport,
                reportLock = centremaster.reportLock,
                bookingLock = centremaster.bookingLock,
                unlockTime = centremaster.unlockTime,
                smsAllow = centremaster.smsAllow,
                emailAllow = centremaster.emailAllow,
                paymentMode = centremaster.paymentMode,
                paymentModeId = centremaster.paymentModeId,
                reportHeader = centremaster.reportHeader,
                reciptHeader = centremaster.reciptHeader,
                reciptFooter = centremaster.reciptFooter,
                showISO = centremaster.showISO,
                showBackcover = centremaster.showBackcover,
                reportBackImage = centremaster.reportBackImage,
                reporrtHeaderHeightY = centremaster.reporrtHeaderHeightY,
                patientYHeader = centremaster.patientYHeader,
                barcodeXPosition = centremaster.barcodeXPosition,
                barcodeYPosition = centremaster.barcodeYPosition,
                QRCodeXPosition = centremaster.QRCodeXPosition,
                QRCodeYPosition = centremaster.QRCodeYPosition,
                isQRheader = centremaster.isQRheader,
                isBarcodeHeader = centremaster.isBarcodeHeader,
                footerHeight = centremaster.footerHeight,
                NABLxPosition = centremaster.NABLxPosition,
                NABLyPosition = centremaster.NABLyPosition,
                docSignYPosition = centremaster.docSignYPosition,
                receiptHeaderY = centremaster.receiptHeaderY,
                PAN = centremaster.PAN,
                adharNo = centremaster.adharNo,
                bankAccount = centremaster.bankAccount,
                IFSCCode = centremaster.IFSCCode,
                bankID = centremaster.bankID,
                salesExecutiveID = centremaster.salesExecutiveID,
                isDefault = centremaster.isDefault,
                isLab = centremaster.isLab,
                minBookingAmt = centremaster.minBookingAmt,
                lockedBy = centremaster.lockedBy,
                LockDate = centremaster.LockDate,
                unlockBy = centremaster.unlockBy,
                unlockDate = centremaster.unlockDate,
                state = centremaster.state,
                chequeNo = centremaster.chequeNo,
                bankName = centremaster.bankName,
                chequeAmount = centremaster.chequeAmount,
                creditPeridos = centremaster.creditPeridos,
                showClientCode = centremaster.showBackcover,
                patientRate = centremaster.patientRate,
                clientRate = centremaster.clientRate,
                isLock = centremaster.isLock,
                isPrePrintedBarcode = centremaster.isPrePrintedBarcode,
                isActive = centremaster.isActive,
                createdById = centremaster.createdById,
                createdDateTime = centremaster.createdDateTime
            };
        }

        private empMaster CreateEmployee(centreMaster centremaster, int centreId, int roleId)
        {
            return new empMaster
            {
                title = "",
                empCode = centremaster.centrecode,
                fName = centremaster.companyName,
                lName = "",
                address = centremaster.address,
                pinCode = centremaster.pinCode,
                email = centremaster.email,
                mobileNo = centremaster.mobileNo,
                landline = centremaster.landline,
                userName = centremaster.centrecode,
                password = centremaster.mobileNo,
                createdById = centremaster.createdById,
                bloodGroup= "b+",
                deptAccess= centremaster.centretype,
                createdDateTime = centremaster.createdDateTime,
                autoCreated = 1,
                centreId = centreId,
                defaultrole = roleId,
                city = 0,
                state = centremaster.state,
                defaultcentre = centreId

            };

        }
        private empCenterAccess SaveEmpCentreAccess(int employeeId, int centreId, int createdById, DateTime createdDateTime)
        {
            return new empCenterAccess
            {
                id = 0,
                empId = employeeId,
                centreId = centreId,
                isActive = true,
                createdById = createdById,
                createdDateTime = createdDateTime

            };

        }
        private empRoleAccess SaveEmpRoleAccess(int employeeId, int roleId, int createdById, DateTime createdDateTime)
        {
            return new empRoleAccess
            {
                id = 0,
                empId = employeeId,
                roleId = roleId,
                isActive = true,
                createdById = createdById,
                createdDateTime = createdDateTime

            };

        }

        private rateTypeMaster CreateRateTypeMaster(string companyName, int centreId, int createdById, DateTime createdDateTime)
        {
            return new rateTypeMaster
            {
                id = 0,
                rateName = companyName,
                rateTypeId = centreId,
                rateType = companyName,
                isActive = true,
                createdById = createdById,
                createdDateTime = createdDateTime
            };
        }

        private rateTypeTagging CreateRateTypeTagging(int rateTypeId, int centreId)
        {
            return new rateTypeTagging
            {
                id =0,
                rateTypeId= rateTypeId,
                centreId= centreId
            };
        }
        private static centreMaster UpdateCentreDetails(centreMaster CentreMaster, centreMaster centremaster)
        {
            CentreMaster.centretype = centremaster.centretype;
            CentreMaster.centrecode = centremaster.centrecode;
            CentreMaster.companyName = centremaster.companyName;
            CentreMaster.mobileNo = centremaster.mobileNo;
            CentreMaster.landline = centremaster.landline;
            CentreMaster.address = centremaster.address;
            CentreMaster.pinCode = centremaster.pinCode;
            CentreMaster.email = centremaster.email;
            CentreMaster.ownerName = centremaster.ownerName;
            CentreMaster.proId = centremaster.proId;
            CentreMaster.reportEmail = centremaster.reportEmail;
            CentreMaster.parentCentreID = centremaster.parentCentreID;
            CentreMaster.processingLab = centremaster.processingLab;
            CentreMaster.creditLimt = centremaster.creditLimt;
            CentreMaster.allowDueReport = centremaster.allowDueReport;
            CentreMaster.reportLock = centremaster.reportLock;
            CentreMaster.bookingLock = centremaster.bookingLock;
            CentreMaster.unlockTime = centremaster.unlockTime;
            CentreMaster.smsAllow = centremaster.smsAllow;
            CentreMaster.emailAllow = centremaster.emailAllow;
            CentreMaster.paymentMode = centremaster.paymentMode;
            CentreMaster.paymentModeId = centremaster.paymentModeId;
            CentreMaster.reportHeader = centremaster.reportHeader;
            CentreMaster.reciptHeader = centremaster.reciptHeader;
            CentreMaster.reciptFooter = centremaster.reciptFooter;
            CentreMaster.showISO = centremaster.showISO;
            CentreMaster.showBackcover = centremaster.showBackcover;
            CentreMaster.reportBackImage = centremaster.reportBackImage;
            CentreMaster.reporrtHeaderHeightY = centremaster.reporrtHeaderHeightY;
            CentreMaster.patientYHeader = centremaster.patientYHeader;
            CentreMaster.barcodeXPosition = centremaster.barcodeXPosition;
            CentreMaster.barcodeYPosition = centremaster.barcodeYPosition;
            CentreMaster.QRCodeXPosition = centremaster.QRCodeXPosition;
            CentreMaster.QRCodeYPosition = centremaster.QRCodeYPosition;
            CentreMaster.isQRheader = centremaster.isQRheader;
            CentreMaster.isBarcodeHeader = centremaster.isBarcodeHeader;
            CentreMaster.footerHeight = centremaster.footerHeight;
            CentreMaster.NABLxPosition = centremaster.NABLxPosition;
            CentreMaster.NABLyPosition = centremaster.NABLyPosition;
            CentreMaster.docSignYPosition = centremaster.docSignYPosition;
            CentreMaster.receiptHeaderY = centremaster.receiptHeaderY;
            CentreMaster.PAN = centremaster.PAN;
            CentreMaster.adharNo = centremaster.adharNo;
            CentreMaster.bankAccount = centremaster.bankAccount;
            CentreMaster.IFSCCode = centremaster.IFSCCode;
            CentreMaster.bankID = centremaster.bankID;
            CentreMaster.salesExecutiveID = centremaster.salesExecutiveID;
            CentreMaster.isDefault = centremaster.isDefault;
            CentreMaster.isLab = centremaster.isLab;
            CentreMaster.minBookingAmt = centremaster.minBookingAmt;
            CentreMaster.lockedBy = centremaster.lockedBy;
            CentreMaster.LockDate = centremaster.LockDate;
            CentreMaster.unlockBy = centremaster.unlockBy;
            CentreMaster.unlockDate = centremaster.unlockDate;
            CentreMaster.state = centremaster.state;
            CentreMaster.chequeNo = centremaster.chequeNo;
            CentreMaster.bankName = centremaster.bankName;
            CentreMaster.chequeAmount = centremaster.chequeAmount;
            CentreMaster.creditPeridos = centremaster.creditPeridos;
            CentreMaster.showClientCode = centremaster.showBackcover;
            CentreMaster.patientRate = centremaster.patientRate;
            CentreMaster.clientRate = centremaster.clientRate;
            CentreMaster.isLock = centremaster.isLock;
            CentreMaster.isPrePrintedBarcode = centremaster.isPrePrintedBarcode;
            CentreMaster.isActive = centremaster.isActive;
            CentreMaster.createdById = centremaster.createdById;
            CentreMaster.createdDateTime = centremaster.createdDateTime;

            return CentreMaster;
        }
    }
}
