using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Account;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace iMARSARLIMS.Services
{
    public class centreInvoiceServices : IcentreInvoiceServices
    {
        private readonly ContextClass db;
        public centreInvoiceServices(ContextClass context, ILogger<BaseController<centreInvoice>> logger)
        {
            db = context;
        }
        async Task<ServiceStatusResponseModel> IcentreInvoiceServices.CreateInvoice(centreInvoiceRequestModel CentreInvoice)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    if (CentreInvoice.id == 0) // Creating a new invoice
                    {
                        var invoiceMasterData = CreateInvoiceData(CentreInvoice);
                        var InvoiceData = db.centreInvoice.Add(invoiceMasterData);
                        await db.SaveChangesAsync();

                        var InvoiceNo = InvoiceData.Entity.id;
                        if (InvoiceNo > 0)
                        {
                            // Get the related booking items
                            var invoiceItemData = db.tnx_BookingItem
                                .Where(b => CentreInvoice.LabNo.Contains(b.transactionId.ToString()) && string.IsNullOrEmpty(b.invoiceNo))
                                .ToList();

                            if (invoiceItemData.Any())
                            {
                                // Update the items
                                foreach (var item in invoiceItemData)
                                {
                                    UpdateItemMasterCreateInvoice(item, CentreInvoice, InvoiceNo);
                                }

                                db.tnx_BookingItem.UpdateRange(invoiceItemData); // Corrected Update method
                                await db.SaveChangesAsync();
                            }

                            await transaction.CommitAsync(); // Commit the transaction
                        }

                        return new ServiceStatusResponseModel
                        {
                            Data = invoiceMasterData,
                            Success = true
                        };
                    }
                    else // Updating an existing invoice
                    {
                        if(CentreInvoice.cancelReason=="")
                        {
                            return new ServiceStatusResponseModel
                            {
                                Success = false,
                                Message ="Invoice CancelReason Required"
                            };
                        }
                        var invoiceData = db.centreInvoice.FirstOrDefault(i => i.id == CentreInvoice.id);
                        if (invoiceData != null)
                        {
                            updateInvoiceData(invoiceData, CentreInvoice.cancelByID, CentreInvoice.cancelReason);

                            var invoiceItemData = db.tnx_BookingItem
                                .Where(b => CentreInvoice.LabNo.Contains(b.transactionId.ToString()) && string.IsNullOrEmpty(b.invoiceNo))
                                .ToList();

                            if (invoiceItemData.Any())
                            {
                                foreach (var item in invoiceItemData)
                                {
                                    UpdateItemMasterCancelInvoice(item, CentreInvoice, CentreInvoice.id);
                                }

                                db.tnx_BookingItem.UpdateRange(invoiceItemData); // Corrected Update method
                                await db.SaveChangesAsync();
                            }

                            await transaction.CommitAsync(); // Commit the transaction
                        }

                        return new ServiceStatusResponseModel
                        {
                            Data = invoiceData,
                            Success = true
                        };
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(); // Rollback on exception

                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = ex.Message
                    };
                }
            }
        }

        private centreInvoice CreateInvoiceData(centreInvoiceRequestModel CentreInvoice)
        {
            return new centreInvoice
            {
                id = CentreInvoice.id,
                invoiceNo = CentreInvoice.invoiceNo,
                centreid = CentreInvoice.centreid,
                fromDate = CentreInvoice.fromDate,
                toDate = CentreInvoice.toDate,
                rate = CentreInvoice.rate,
                createDate = CentreInvoice.createDate,
                createdBy = CentreInvoice.createdBy
            };
        }

        private void UpdateItemMasterCreateInvoice(tnx_BookingItem InvoiceItem, centreInvoiceRequestModel CentreInvoiceRequestModel, int invoiceNo)
        {
            InvoiceItem.invoiceNo = invoiceNo.ToString(); // Ensure correct type handling
            InvoiceItem.invoiceAmount = CentreInvoiceRequestModel.rate;
            InvoiceItem.invoiceDate = CentreInvoiceRequestModel.InvoiceDate;
            InvoiceItem.createdById = CentreInvoiceRequestModel.createdBy;
            InvoiceItem.isInvoiceCreated = 1;
        }

        private void updateInvoiceData(centreInvoice CentreInvoice, int CancelById, string CancelReason)
        {
            CentreInvoice.cancelByID = CancelById;
            CentreInvoice.cancelReason = CancelReason;
            CentreInvoice.isCancel = 1;
            CentreInvoice.cancelDate = DateTime.Now;
        }

        private void UpdateItemMasterCancelInvoice(tnx_BookingItem InvoiceItem, centreInvoiceRequestModel CentreInvoiceRequestModel, int invoiceNo)
        {
            InvoiceItem.invoiceNo = "";
            InvoiceItem.invoiceAmount = 0;
            InvoiceItem.isInvoiceCreated = 0;
        }

       async Task<ServiceStatusResponseModel> IcentreInvoiceServices.SearchInvoiceData(DateTime FromDate, DateTime Todate, List<int> CentreIds)
        {
            try
            {
                var invoiceData = await (from tbi in db.tnx_BookingItem
                                         join TB in db.tnx_Booking on tbi.transactionId equals TB.transactionId
                                         join cm in db.centreMaster on TB.centreId equals cm.centreId
                                         where tbi.createdDateTime >= FromDate && tbi.createdDateTime <= Todate
                                             && CentreIds.Contains(cm.parentCentreID)
                                             && (tbi.isPackage == 1 || tbi.isSampleCollected == "Y")
                                             && string.IsNullOrEmpty(tbi.invoiceNo)
                                             && tbi.isRemoveItem == 0
                                         group new { tbi, TB, cm } by cm.parentCentreID into g
                                         select new
                                         {
                                             FromDate = g.Min(t => t.tbi.createdDateTime).ToString("dd-MMM-yyyy"),
                                             ToDate = g.Max(t => t.tbi.createdDateTime).ToString("dd-MMM-yyyy"),
                                             CreditAmount = g.Sum(t => t.tbi.rate),
                                             TransactionId = g.FirstOrDefault().tbi.transactionId, // Get the transactionId of the first item in the group
                                             CompanyID = g.FirstOrDefault().cm.centreId,           // Get the centreId of the first item in the group
                                             Discount = g.Sum(t => t.tbi.discount),
                                             NetAmount = g.Sum(t => t.tbi.netAmount),
                                             Centre = g.FirstOrDefault().cm.companyName // Get the CompanyName of the first item in the group
                                         }).ToListAsync();



                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = invoiceData
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

        async Task<ServiceStatusResponseModel> IcentreInvoiceServices.GetLastInvoiceData(List<int> CentreId)
        {
            try
            {
                var result = await (from cm in db.centreMaster
                                    join ci in db.centreInvoice on cm.centreId equals ci.centreid
                                    join em in db.empMaster on ci.createdBy equals em.empId
                                    where CentreId.Contains(cm.centreId)
                                    orderby ci.InvoiceDate descending 
                                    group new { ci, cm, em } by cm.centreId into grouped
                                    select new
                                    {
                                        centreId = grouped.Key,
                                        invoiceNo = grouped.FirstOrDefault().ci.id,
                                        CompanyName = grouped.FirstOrDefault().cm.companyName,
                                        invoiceDate = grouped.FirstOrDefault().ci.InvoiceDate,
                                        FromDate = grouped.FirstOrDefault().ci.fromDate,
                                        ToDate = grouped.FirstOrDefault().ci.toDate,
                                        Rate = grouped.FirstOrDefault().ci.rate,
                                        CreatedBy = string.Concat(grouped.FirstOrDefault().em.fName, " ", grouped.FirstOrDefault().em.lName)
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
    }
}
