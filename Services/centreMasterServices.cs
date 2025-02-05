using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
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

                    var centreId = 0;
                    var message1 = "";
                    if (centremaster.centreId == 0)
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
                        var CentreMasterData = CreateCentreDetails(centremaster);
                        var CentreData = await db.centreMaster.AddAsync(CentreMasterData);
                        await db.SaveChangesAsync();
                        centreId = CentreData.Entity.centreId;
                        if (centremaster.processingLab == 0)
                        {
                            var Centre = await db.centreMaster.FirstOrDefaultAsync(cm => cm.centreId == centreId);
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
                        if (centremaster.centretype == "Franchisee" || centremaster.centretype == "Sub-Franchisee")
                        {
                            var roleId = 0;
                            if (centremaster.centretype == "Franchisee")
                                roleId = 8;
                            if (centremaster.centretype == "Sub-Franchisee")
                                roleId = 9;

                            var maxempid = db.empMaster.Select(empMaster => empMaster.empId).Max();
                            var empcode = string.Concat("IMS", maxempid.ToString("D2"));
                            var EmployeeRegData = CreateEmployee(centremaster, centreId, roleId, empcode);
                            var EmployeeData = db.empMaster.Add(EmployeeRegData);
                            await db.SaveChangesAsync();
                            var employeeId = EmployeeData.Entity.empId;
                            var centreAccess = SaveEmpCentreAccess(employeeId, centreId, (int)centremaster.createdById, centremaster.createdDateTime);
                            db.empCenterAccess.Add(centreAccess);
                            await db.SaveChangesAsync();
                            var RoleAccess = SaveEmpRoleAccess(employeeId, roleId, (int)centremaster.createdById, centremaster.createdDateTime);
                            db.empRoleAccess.Add(RoleAccess);
                            await db.SaveChangesAsync();
                        }
                        await SaveEmpCentreAccessData(centremaster.addEmpCenterAccess, centreId);
                        message1 = "Saved Successful";
                    }


                    else
                    {
                        var CentreMaster = db.centreMaster.FirstOrDefault(cm => cm.centreId == centremaster.centreId);
                        if (CentreMaster != null)
                        {
                            UpdateCentreDetails(CentreMaster, centremaster);
                        }
                        var CentreMasterData = db.centreMaster.Update(CentreMaster);
                        await db.SaveChangesAsync();
                        centreId = CentreMasterData.Entity.centreId;
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
                        message1 = "Updated Successful";
                    }
                    await transaction.CommitAsync();

                    var result = db.centreMaster.Where(c => c.centreId == centreId).ToList();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Data = result,
                        Message = message1

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
        private centreMaster CreateCentreDetails(centreMaster centremaster)
        {
            return new centreMaster
            {
                centreId = centremaster.centreId,
                centretype = centremaster.centretype,
                centretypeid = centremaster.centretypeid,
                centrecode = centremaster.centrecode,
                companyName = centremaster.companyName,
                mobileNo = centremaster.mobileNo,
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
                ZoneId = centremaster.ZoneId,
                state = centremaster.state,
                DistrictId = centremaster.DistrictId,
                cityId = centremaster.cityId,
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
                createdDateTime = centremaster.createdDateTime,
                ac = centremaster.ac,
                clientmrp = centremaster.clientmrp,
                documentType = centremaster.documentType,
                Document = centremaster.Document,
                receptionarea = centremaster.receptionarea,
                waitingarea = centremaster.waitingarea,
                watercooler = centremaster.waitingarea
            };
        }

        private empMaster CreateEmployee(centreMaster centremaster, int centreId, int roleId, string EmpCode)
        {

            return new empMaster
            {
                title = "Mr.",
                empCode = EmpCode,
                fName = centremaster.companyName,
                lName = "",
                address = centremaster.address,
                pinCode = centremaster.pinCode,
                email = centremaster.email,
                mobileNo = centremaster.mobileNo,
                userName = centremaster.centrecode,
                password = centremaster.mobileNo,
                createdById = centremaster.createdById,
                bloodGroup = "b+",
                createdDateTime = centremaster.createdDateTime,
                autoCreated = 1,
                centreId = centreId,
                defaultrole = roleId,
                city = 0,
                state = centremaster.state,
                defaultcentre = centreId,
                fileName = "",
                isActive = centremaster.isActive


            };

        }
        private empCenterAccess SaveEmpCentreAccess(int employeeId, int centreId, int createdById, DateTime createdDateTime)
        {
            return new empCenterAccess
            {
                id = 0,
                empId = employeeId,
                centreId = centreId,
                isActive = 1,
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
                isActive = 1,
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
                isActive = 1,
                createdById = createdById,
                createdDateTime = createdDateTime
            };
        }

        private rateTypeTagging CreateRateTypeTagging(int rateTypeId, int centreId)
        {
            return new rateTypeTagging
            {
                id = 0,
                rateTypeId = rateTypeId,
                centreId = centreId
            };
        }
        private static centreMaster UpdateCentreDetails(centreMaster CentreMaster, centreMaster centremaster)
        {
            CentreMaster.centrecode = centremaster.centrecode;
            CentreMaster.companyName = centremaster.companyName;
            CentreMaster.mobileNo = centremaster.mobileNo;
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
            CentreMaster.ZoneId = centremaster.ZoneId;
            CentreMaster.state = centremaster.state;
            CentreMaster.DistrictId = centremaster.DistrictId;
            CentreMaster.cityId = centremaster.cityId;
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
            CentreMaster.ac = centremaster.ac;
            CentreMaster.clientmrp = centremaster.clientmrp;
            CentreMaster.documentType = centremaster.documentType;
            CentreMaster.Document = centremaster.Document;
            CentreMaster.receptionarea = centremaster.receptionarea;
            CentreMaster.waitingarea = centremaster.waitingarea;
            CentreMaster.watercooler = centremaster.waitingarea;
            CentreMaster.updateById = centremaster.updateById;
            CentreMaster.updateDateTime = centremaster.updateDateTime;

            return CentreMaster;
        }

        async Task<ServiceStatusResponseModel> IcentreMasterServices.UpdateCentreStatus(int CentreId, byte status, int UserId)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var CentreData = await db.centreMaster.Where(c => c.centreId == CentreId).FirstOrDefaultAsync();
                    if (CentreData != null)
                    {
                        CentreData.isActive = status;
                        CentreData.updateById = UserId;
                        CentreData.updateDateTime = DateTime.Now;

                        db.centreMaster.UpdateRange(CentreData);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = " Updated Successful"
                        };
                    }
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "Centre Not found on this Id"
                    };
                }
                catch (Exception ex)
                {
                    transaction.RollbackAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = ex.Message
                    };
                }
            }
        }

        private async Task<ServiceStatusResponseModel> SaveEmpCentreAccessData(IEnumerable<empCenterAccess> empcentreaccess, int Centreid)
        {
            if (empcentreaccess != null)
            {
                var empCentreDataList = empcentreaccess.Select(empcentre => CreateEmpCentreData(empcentre, Centreid)).ToList();
                if (empCentreDataList.Any())
                {
                    db.empCenterAccess.AddRange(empCentreDataList);
                    await db.SaveChangesAsync();
                }
            }
            return new ServiceStatusResponseModel
            {
                Success = true,
                Data = empcentreaccess
            };

        }

        private empCenterAccess CreateEmpCentreData(empCenterAccess empcentreaccess, int CentreId)
        {
            return new empCenterAccess
            {
                id = empcentreaccess.id,
                empId = empcentreaccess.empId,
                centreId = CentreId,
                isActive = empcentreaccess.isActive,
                createdById = empcentreaccess.createdById,
                createdDateTime = empcentreaccess.createdDateTime
            };

        }
        async Task<ServiceStatusResponseModel> IcentreMasterServices.GetParentCentre()
        {
            try
            {
                var ParentCentre = await db.centreMaster
                    .Where(c => c.isActive == 1 && c.centretypeid == 3)
                    .Select(c => new
                    {
                        c.companyName,
                        c.centreId
                    })
                    .ToListAsync();
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = ParentCentre
                };
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Data = null,
                    Message = ex.Message
                };
            }
        }
        async Task<ServiceStatusResponseModel> IcentreMasterServices.GetProcesiongLab()
        {
            try
            {
                var ParentCentre = await db.centreMaster
                    .Where(c => c.isActive == 1 && (c.centretypeid == 1 || c.centretypeid == 2))
                    .Select(c => new
                    {
                        c.companyName,
                        c.centreId
                    })
                    .ToListAsync();

                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = ParentCentre
                };
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Data = null,
                    Message = ex.Message
                };
            }
        }
        async Task<ServiceStatusResponseModel> IcentreMasterServices.GetMRPRateType()
        {
            try
            {
                var ParentCentre = await db.rateTypeMaster
                    .Where(c => c.isActive == 1 && c.id == 1)
                    .Select(c => new
                    {
                        c.rateName,
                        c.id
                    })
                    .ToListAsync();
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = ParentCentre
                };
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Data = null,
                    Message = ex.Message
                };
            }
        }
        async Task<ServiceStatusResponseModel> IcentreMasterServices.GetRateType(int CentreType, int ParentCentreId)
        {
            try
            {
                // 1   Refrence Lab,2   SateLite Lab,3   Franchisee,4   Sub - Franchisee
                List<CentreModel> ParentCentre;
                if (CentreType == 4)
                {
                    ParentCentre = await (from rtt in db.rateTypeTagging
                                          join rtm in db.rateTypeMaster on rtt.rateTypeId equals rtm.rateTypeId
                                          where rtt.centreId == ParentCentreId
                                          select new CentreModel
                                          {
                                              RateName = rtm.rateName,
                                              Id = rtm.id
                                          }).ToListAsync();
                }
                else
                {
                    ParentCentre = await db.rateTypeMaster
                        .Where(c => c.isActive == 1)
                        .Select(c => new CentreModel
                        {
                            RateName = c.rateName,
                            Id = c.id
                        })
                        .ToListAsync();
                }

                if (CentreType == 4 || CentreType == 3)
                {
                    ParentCentre.Add(new CentreModel { RateName = "Self", Id = 0 });
                    ParentCentre = ParentCentre.OrderBy(p => p.Id).ToList();
                }
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = ParentCentre
                };
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Data = null,
                    Message = ex.Message
                };
            }
        }
        public class CentreModel
        {
            public string RateName { get; set; } // For cases where rateName is required
            public int? Id { get; set; } // For cases where rateTypeId (id) is required
        }

        public async Task<ServiceStatusResponseModel> GetCentreData(int centreId)
        {
            var centreMasterData = await db.centreMaster.Where(c => c.centreId == centreId).FirstOrDefaultAsync();

            if (centreMasterData == null)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = "Centre not found."
                };
            }
            var centreEmpAccess = await db.empCenterAccess.Where(ec => ec.centreId == centreId).Select(ec => ec.empId).ToListAsync();

            var result = new
            {
                CentreData = centreMasterData,
                EmployeeAccess = centreEmpAccess
            };

            return new ServiceStatusResponseModel
            {
                Success = true,
                Data = result
            };
        }

        async Task<ServiceStatusResponseModel> IcentreMasterServices.SaveLetterHead(ReportLetterHead LetterHead)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var data = db.centreMaster.Where(c => c.centreId == LetterHead.CentreId).FirstOrDefault();
                    UpdateLetterHeadDetail(data, LetterHead);
                    db.centreMaster.Update(data);
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "Saved Successful"
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

        private void UpdateLetterHeadDetail(centreMaster Centre, ReportLetterHead LetterHead)
        {
            Centre.reporrtHeaderHeightY = LetterHead.reporrtHeaderHeightY;
            Centre.patientYHeader = LetterHead.patientYHeader;
            Centre.barcodeXPosition = LetterHead.barcodeXPosition;
            Centre.barcodeYPosition = LetterHead.barcodeYPosition;
            Centre.QRCodeXPosition = LetterHead.QRCodeXPosition;
            Centre.QRCodeYPosition = LetterHead.QRCodeYPosition;
            Centre.isQRheader = LetterHead.isQRheader;
            Centre.isBarcodeHeader = LetterHead.isBarcodeHeader;
            Centre.footerHeight = LetterHead.footerHeight;
            Centre.NABLxPosition = LetterHead.NABLxPosition;
            Centre.NABLyPosition = LetterHead.NABLyPosition;
            Centre.docSignYPosition = LetterHead.docSignYPosition;
            Centre.receiptHeaderY = LetterHead.receiptHeaderY;
            Centre.reportHeader = LetterHead.reportHeader;
            Centre.reciptHeader = LetterHead.reciptHeader;
            Centre.reciptFooter = LetterHead.reciptFooter;
        }
    }
}
