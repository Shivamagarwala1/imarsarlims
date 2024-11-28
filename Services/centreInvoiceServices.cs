using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Account;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;
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

        ServiceStatusResponseModel IcentreInvoiceServices.SearchInvoiceData(DateTime FromDate, DateTime Todate, List<int> CentreIds)
        {
           var InvoiceData=  db.tnx_BookingItem
            .Where(item => item.invoiceNo == ""
                        && item.createdDateTime >= FromDate
                        && item.createdDateTime <= Todate
                        && CentreIds.Contains(item.centreId))
            .GroupBy(item => item.centreId)
            .Select(group => new
            {
                CentreId = group.Key,
                Rate = group.Sum(item => item.netAmount),
                FromDate = group.Min(item => item.createdDateTime).Date,
                ToDate = group.Max(item => item.createdDateTime).Date
            })
            .ToList();

            return new ServiceStatusResponseModel
            {
                Success = true,
                Data = InvoiceData
            };
        }
    }
}
