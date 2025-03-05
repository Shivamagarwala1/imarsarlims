using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

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
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "Saved SuccessFul"
                    };
                }
                catch (Exception ex)
                {
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
                using (var stream = new MemoryStream())
                {
                    await ratelistexcel.CopyToAsync(stream);
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
                    var RateListData = new List<rateTypeWiseRateList>();
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                        {

                            var DeptId = GetNumericData(worksheet.Cells[row, 1], row, "DeptId");  // Validate column 1 (DeptId)
                            var Ratetypeid = GetNumericData(worksheet.Cells[row, 2], row, "Ratetypeid");  // Validate column 2 (Ratetypeid)
                            var MRP = GetDoubleData(worksheet.Cells[row, 3], row, "MRP");  // Validate column 3 (MRP)
                            var Discount = GetNumericData(worksheet.Cells[row, 4], row, "Discount");  // Validate column 4 (Discount)
                            var Rate = GetNumericData(worksheet.Cells[row, 5], row, "Rate");  // Validate column 5 (Rate)
                            var Itemid = GetNumericData(worksheet.Cells[row, 6], row, "Itemid");  // Validate column 6 (Itemid)
                            var ItemCode = worksheet.Cells[row, 7].Value?.ToString();  // Column 7 (ItemCode)
                            var rateTypeWiseRateList = new rateTypeWiseRateList
                            {
                                id = 0,
                                deptId = DeptId,
                                rateTypeId = Ratetypeid,
                                mrp = MRP,
                                rate = Rate,
                                discount = Discount,
                                itemid = Itemid,
                                itemCode = ItemCode,
                                createdById = 1,
                                createdDateTime = DateTime.Now
                            };
                            RateListData.Add(rateTypeWiseRateList);


                        }
                        var saveResult = await SaveRateListExcel(RateListData);
                    }
                }
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Message = "RateList Saved Successfull"
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

        private async Task<ServiceStatusResponseModel> SaveRateListExcel(List<rateTypeWiseRateList> rateListData)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var itemIds = rateListData.Select(x => x.itemid).Distinct().ToList();
                    var rateTypeId = rateListData.Select(x => x.rateTypeId).Distinct();

                    var oldratelistDelete = db.rateTypeWiseRateList.Where(r => rateTypeId.Contains(r.rateTypeId) && itemIds.Contains(r.itemid)).ToList();
                    db.rateTypeWiseRateList.RemoveRange(oldratelistDelete);
                    await db.SaveChangesAsync();
                    var ratelistdata = rateListData.Select(CreateRatelistData).ToList();
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
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = ex.GetBaseException().Message
                    };

                }
            }
        }

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
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "Saved SuccessFul"
                    };
                }
                catch (Exception ex)
                {
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
            var excelData = ( from rtt in db.rateTypeWiseRateList
                                    join ld in db.labDepartment on rtt.deptId equals ld.id
                                    join im in db.itemMaster on rtt.itemid equals im.itemId
                                    join rt in db.rateTypeMaster on rtt.rateTypeId equals rt.id
                                    where rtt.rateTypeId== RatetypeId
                                    select new
                                    {
                                       DepartMentId= rtt.deptId,
                                       DepartmentName= ld.deptName,
                                       ItemId= rtt.itemid,
                                       itemcode= im.code,
                                       InvestigationName= im.itemName,
                                       RateTypeId= rtt.rateTypeId,
                                       RateTypeName= rt.rateType,
                                       MRP= rtt.mrp,
                                       Rate= rtt.rate
                                    }).ToList();
                var excelByte = MyFunction.ExportToExcel(excelData, "LedgerReportExcel");
            return excelByte;
        }

        async Task<ServiceStatusResponseModel> IrateTypeWiseRateListServices.GetRateTypeRateListData(int ratetypeid, int deptId)
        {
            try
            {
                var RateData = (from im in db.itemMaster
                                join rtt in db.rateTypeWiseRateList
                                on im.itemId equals rtt.itemid into rttGroup
                                from rtt in rttGroup.DefaultIfEmpty()
                                where im.deptId == deptId && (rtt == null || rtt.rateTypeId == ratetypeid)
                                select new
                                {
                                    im.itemId,
                                    itemCode= im.code,
                                    im.itemName,
                                    mrp = rtt != null ? rtt.mrp : 0,
                                    rate = rtt != null ? rtt.rate : 0
                                }).ToList();
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
    }
}
