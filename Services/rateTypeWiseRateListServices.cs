using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace iMARSARLIMS.Services
{
    public class rateTypeWiseRateListServices : IrateTypeWiseRateListServices
    {
        private readonly ContextClass db;
        private readonly MySql_Procedure_Services _MySql_Procedure_Services;

        public rateTypeWiseRateListServices(ContextClass context, ILogger<BaseController<rateTypeWiseRateList>> logger, MySql_Procedure_Services mySql_Procedure_Services)
        {
            db = context;
            this._MySql_Procedure_Services = mySql_Procedure_Services;
        }

        async Task<ServiceStatusResponseModel> IrateTypeWiseRateListServices.SaveRateList(List<rateTypeWiseRateList> RateTypeWiseRateList)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var itemIds = RateTypeWiseRateList.Select(x => x.itemid).Distinct().ToList();
                    var rateTypeId= RateTypeWiseRateList.Select(x=>x.rateTypeId).FirstOrDefault();
                    var oldratelistDelete = db.rateTypeWiseRateList.Where(r => r.rateTypeId== rateTypeId && itemIds.Contains(r.itemid)).ToList();
                    db.rateTypeWiseRateList.RemoveRange(oldratelistDelete);
                    await db.SaveChangesAsync();
                    var ratelistdata = RateTypeWiseRateList.Select(CreateRatelistData).ToList();
                    db.rateTypeWiseRateList.AddRange(ratelistdata);
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
                        Message = ex.GetBaseException().Message
                    };

                }
            }
        }

        private rateTypeWiseRateList CreateRatelistData(rateTypeWiseRateList ratelist)
        {
            return new rateTypeWiseRateList
            {
                id = ratelist.id,
                deptId = ratelist.deptId,
                rateTypeId = ratelist.rateTypeId,
                mrp = ratelist.mrp,
                discount = ratelist.discount,
                rate = ratelist.rate,
                itemid = ratelist.itemid,
                itemCode = ratelist.itemCode,
                createdById = ratelist.createdById,
                createdDateTime = ratelist.createdDateTime,
                transferRemarks = ratelist.transferRemarks,
                transferDate = ratelist.transferDate
            };
        }

        async Task<ServiceStatusResponseModel> IrateTypeWiseRateListServices.SaveRateListFromExcel(IFormFile ratelistexcel)
        {
            string extension = Path.GetExtension(ratelistexcel.FileName);
            if (extension != ".xlsx" && extension != ".xls")
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = "No valid file extension Found"
                };
            }

            try
            {
                var rateListData = new List<object>(); // Collection to store all rows of data

                using (var stream = new MemoryStream())
                {
                    await ratelistexcel.CopyToAsync(stream);
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;

                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];

                        for (int row = 2; row <= worksheet.Dimension.End.Row; row++) // Start from row 2 assuming row 1 is the header
                        {
                            var deptId = GetNumericData(worksheet.Cells[row, 1], row, "DeptId"); // Validate column 1 (DeptId)
                            var deptName = worksheet.Cells[row, 2].Value?.ToString();  // Validate column 2 (DeptName)
                            var itemId = GetNumericData(worksheet.Cells[row, 3], row, "ItemId");  // Validate column 3 (ItemId)
                            var itemCode = worksheet.Cells[row, 4].Value?.ToString();  // Validate column 4 (ItemCode)
                            var itemName = worksheet.Cells[row, 5].Value?.ToString();  // Validate column 5 (ItemName)
                            var mrp = GetDoubleData(worksheet.Cells[row, 6], row, "MRP");  // Validate column 6 (MRP)
                            var rate = GetDoubleData(worksheet.Cells[row, 7], row, "Rate");  // Validate column 7 (Rate)

                            // Accumulate the row's data
                            var excelData = new
                            {
                                deptId,
                                DepartmentName = deptName,
                                ItemId = itemId,
                                itemCode = itemCode,
                                InvestigationName = itemName,
                                mrp,
                                rate
                            };

                            rateListData.Add(excelData); // Add the current row to the collection
                        }
                    }
                }

                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = rateListData // Return the full list of data
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

        //private async Task<ServiceStatusResponseModel> SaveRateListExcel(List<rateTypeWiseRateList> rateListData)
        //{
        //    using (var transaction = await db.Database.BeginTransactionAsync())
        //    {
        //        try
        //        {
        //            var itemIds = rateListData.Select(x => x.itemid).Distinct().ToList();
        //            var rateTypeId = rateListData.Select(x => x.rateTypeId).Distinct();

        //            var oldratelistDelete = db.rateTypeWiseRateList.Where(r => rateTypeId.Contains(r.rateTypeId) && itemIds.Contains(r.itemid)).ToList();
        //            db.rateTypeWiseRateList.RemoveRange(oldratelistDelete);
        //            await db.SaveChangesAsync();
        //            var ratelistdata = rateListData.Select(CreateRatelistData).ToList();
        //            db.rateTypeWiseRateList.AddRange(ratelistdata);
        //            await db.SaveChangesAsync();
        //            await transaction.CommitAsync();
        //            return new ServiceStatusResponseModel
        //            {
        //                Success = true,
        //                Message = "Saved SuccessFul"
        //            };
        //        }
        //        catch (Exception ex)
        //        {
        //            await transaction.RollbackAsync();
        //            return new ServiceStatusResponseModel
        //            {
        //                Success = false,
        //                Message = ex.GetBaseException().Message
        //            };

        //        }
        //    }
        //}

        private int GetNumericData(ExcelRange cell, int row, string columnName)
        {
            if (cell.Value == null)
                throw new Exception($"Invalid value in row {row}, column {columnName}. Please enter a valid numeric value.");
            string strValue = cell.Value.ToString().Trim();
            // Check if the value is a valid integer
            if (int.TryParse(strValue, out int result))
            {
                return result;
            }
            throw new Exception($"Invalid integer value in row {row}, column {columnName}. Please enter a valid numeric value.");
        }
        private double GetDoubleData(ExcelRange cell, int row, string columnName)
        {
            if (cell.Value == null)
                throw new Exception($"Invalid value in row {row}, column {columnName}. Please enter a valid numeric value.");
            string strValue = cell.Value.ToString().Trim();
            if (double.TryParse(strValue, out double result))
            {
                if (double.IsNaN(result) || double.IsInfinity(result))
                {
                    throw new Exception($"Invalid double value in row {row}, column {columnName}. The value cannot be NaN or Infinity.");
                }
                return result;
            }
            throw new Exception($"Invalid double value in row {row}, column {columnName}. Please enter a valid numeric value.");
        }

        async Task<ServiceStatusResponseModel> IrateTypeWiseRateListServices.SaveRateListitemWise(List<rateTypeWiseRateList> RateTypeWiseRateList)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var RatetypeIds = RateTypeWiseRateList.Select(x => x.rateTypeId).Distinct().ToList();
                    var itemId = RateTypeWiseRateList.Select(x => x.itemid).FirstOrDefault();
                    var oldratelistDelete = db.rateTypeWiseRateList.Where(r => RatetypeIds.Contains( r.rateTypeId) && r.itemid== itemId).ToList();
                    db.rateTypeWiseRateList.RemoveRange(oldratelistDelete);
                    await db.SaveChangesAsync();
                    var ratelistdata = RateTypeWiseRateList.Select(CreateRatelistData).ToList();
                    db.rateTypeWiseRateList.AddRange(ratelistdata);
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
                        Message = ex.GetBaseException().Message
                    };

                }
            }
        }

        public  byte[] GetRateListExcel(int RatetypeId)
        {
            var excelData = (from im in db.itemMaster
                             join ld in db.labDepartment on im.deptId equals ld.id
                             join rtt in db.rateTypeWiseRateList on im.itemId equals rtt.itemid into rttGroup
                             from rtt in rttGroup.DefaultIfEmpty()
                             join rt in db.rateTypeMaster on rtt.rateTypeId equals rt.id
                             where rtt == null || rtt.rateTypeId == 1
                             select new
                             {
                                 DepartMentId = im.deptId,
                                 DepartmentName = ld.deptName,
                                 ItemId = im.itemId,
                                 ItemCode = im.code,
                                 InvestigationName = im.itemName,
                                 MRP = rtt.mrp,
                                 Rate = rtt.rate
                             }).ToList();
            var excelByte = MyFunction.ExportToExcel(excelData, "LedgerReportExcel");
            return excelByte;
        }

        async Task<ServiceStatusResponseModel> IrateTypeWiseRateListServices.GetRateTypeRateListData(int ratetypeid, int deptId, int itemid)
        {
            try
            {
                var RateData = from im in db.itemMaster
                               join rtt in db.rateTypeWiseRateList
                               on im.itemId equals rtt.itemid into rttGroup
                               from rtt in rttGroup.DefaultIfEmpty()
                               where im.deptId == deptId && (rtt == null || rtt.rateTypeId == ratetypeid)
                               select new
                               {
                                   im.itemId,
                                   itemCode = im.code,
                                   im.itemName,
                                   mrp = rtt != null ? rtt.mrp : 0,
                                   rate = rtt != null ? rtt.rate : 0
                               };
                if(itemid>0)
                {
                    RateData = RateData.Where(q => q.itemId == itemid);
                }

                var ratelist= RateData.ToList();
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = RateData
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

        async Task<ServiceStatusResponseModel> IrateTypeWiseRateListServices.GetItemrateListData(int itemid)
        {
            try
            {
                var result = await _MySql_Procedure_Services.ratelistDataItemWise(itemid);
                return result;
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

        async Task<ServiceStatusResponseModel> IrateTypeWiseRateListServices.GetRateType(int employeeId)
        {
            try
            {
                var centreids = db.empCenterAccess.Where(e => e.empId == employeeId).Select(e => e.centreId).ToList();
                var ratetype=await  ( from rt in db.rateTypeMaster
                                join rtt in db.rateTypeTagging on rt.id equals rtt.rateTypeId
                                where centreids.Contains(rtt.centreId )
                                select new
                                {
                                    rt.id,
                                    rt.rateName
                                }).ToListAsync();
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = ratetype
                };
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                { Success = false, Message = ex.Message };
            }
        }
    }
}
