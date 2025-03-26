using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Account;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace iMARSARLIMS.Services
{
    public class centreInvoiceServices : IcentreInvoiceServices
    {
        private readonly ContextClass db;
        private readonly IConfiguration _configuration;
        public centreInvoiceServices(ContextClass context, ILogger<BaseController<centreInvoice>> logger, IConfiguration configuration)
        {
            db = context;
            this._configuration = configuration;
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
                            var invoiceItemData = db.tnx_BookingItem
                                .Where(b => CentreInvoice.LabNo.Contains(b.transactionId.ToString()) && string.IsNullOrEmpty(b.invoiceNo))
                                .ToList();

                            if (invoiceItemData.Any())
                            {
                                foreach (var item in invoiceItemData)
                                {
                                    UpdateItemMasterCreateInvoice(item, CentreInvoice, InvoiceNo);
                                }
                                db.tnx_BookingItem.UpdateRange(invoiceItemData); // Corrected Update method
                                await db.SaveChangesAsync();
                            }
                            await transaction.CommitAsync();
                        }

                        return new ServiceStatusResponseModel
                        {
                            Data = invoiceMasterData,
                            Success = true
                        };
                    }
                    else // Updating an existing invoice
                    {
                        if (CentreInvoice.cancelReason == "")
                        {
                            return new ServiceStatusResponseModel
                            {
                                Success = false,
                                Message = "Invoice CancelReason Required"
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
                var result = await (from ci in db.centreInvoice
                                    join cm in db.centreMaster on ci.centreid equals cm.centreId
                                    where CentreId.Contains(cm.centreId)
                                    && ci.InvoiceDate == (
                                        db.centreInvoice
                                        .Where(ci2 => ci2.centreid == ci.centreid)
                                        .Max(ci2 => ci2.InvoiceDate))
                                    orderby ci.InvoiceDate descending
                                    select new
                                    {
                                        centreId = ci.centreid,
                                        invoiceNo = ci.id,
                                        CompanyName = cm.companyName,
                                        invoiceDate = ci.InvoiceDate,
                                        FromDate = ci.fromDate,
                                        ToDate = ci.toDate,
                                        Rate = ci.rate
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

        public byte[] PrintInvoice(string InvoiceNo)
        {
            var result = (from cm in db.centreMaster
                          join cp in db.centreInvoice on cm.centreId equals cp.centreid
                          join em in db.empMaster on cp.centreid equals em.centreId
                          where cp.invoiceNo == InvoiceNo
                          select new
                          {
                              CentreName = cm.companyName,

                              cm.centrecode,
                              fromDate = cp.fromDate.ToString("yyyy-MMM-dd"),
                              todate = cp.toDate.ToString("yyyy-MMM-dd"),
                              cp.invoiceNo,
                              InvoiceDate = cp.InvoiceDate.ToString("yyyy-MMM-dd"),
                              cp.rate,
                              Invoice = cp.InvoiceDate.ToString("MMMM-yyyyy"),
                          }).ToList();

            if (result.Count > 0)
            {
                QuestPDF.Settings.License = LicenseType.Community;
                var image1 = _configuration["FileBase64:CompanyLogo1"];
                var image1Bytes = Convert.FromBase64String(image1.Split(',')[1]);
                var document = Document.Create(container =>
                {

                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);

                        page.MarginTop(2.5f, Unit.Centimetre);
                        page.MarginLeft(0.5f, Unit.Centimetre);
                        page.MarginRight(0.5f, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.Foreground();
                        page.DefaultTextStyle(x => x.FontFamily("TimesNewRoman"));
                        page.DefaultTextStyle(x => x.FontSize(10));
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
                                  table.Cell().ColumnSpan(2).AlignCenter().Text(result[0].CentreName).Style(TextStyle.Default.FontSize(10));
                                  table.Cell().ColumnSpan(2).AlignCenter().Text("").Style(TextStyle.Default.FontSize(10));
                                  table.Cell().ColumnSpan(6).AlignCenter().Text("").Style(TextStyle.Default.FontSize(10).Bold().Underline());
                                  table.Cell().ColumnSpan(6).BorderBottom(1.0f, Unit.Point);
                              });
                             // Product table header


                             page.Content()
                        .Column(column =>
                        {
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(1.0f, Unit.Centimetre);
                                    columns.RelativeColumn();
                                    columns.ConstantColumn(2.0f, Unit.Centimetre);
                                    columns.ConstantColumn(2.0f, Unit.Centimetre);
                                });

                                table.Cell().ColumnSpan(2).Height(0.5f, Unit.Centimetre).Text("Invoice     " + result[0].Invoice).Style(TextStyle.Default.FontSize(10));
                                table.Cell().Height(0.5f, Unit.Centimetre).Text("Invoice No").Style(TextStyle.Default.FontSize(10));
                                table.Cell().Height(0.5f, Unit.Centimetre).Text("Invoice Date").Style(TextStyle.Default.FontSize(10));
                                table.Cell().ColumnSpan(2).Height(0.5f, Unit.Centimetre).Text(result[0].centrecode + "-" + result[0].CentreName).Style(TextStyle.Default.FontSize(10));
                                table.Cell().Height(0.5f, Unit.Centimetre).Text(result[0].invoiceNo).Style(TextStyle.Default.FontSize(10));
                                table.Cell().Height(0.5f, Unit.Centimetre).Text(result[0].InvoiceDate).Style(TextStyle.Default.FontSize(10));
                                table.Cell().ColumnSpan(4).Height(1.0f, Unit.Centimetre).Text("").Style(TextStyle.Default.FontSize(10));

                                table.Cell().ColumnSpan(2).Height(0.5f, Unit.Centimetre).Border(0.5f, Unit.Centimetre).Text("Particulers").Style(TextStyle.Default.FontSize(10));
                                table.Cell().ColumnSpan(2).Height(0.5f, Unit.Centimetre).Border(0.5f, Unit.Centimetre).Text("Amount (Rs.)").Style(TextStyle.Default.FontSize(10));

                                table.Cell().ColumnSpan(2).Height(0.5f, Unit.Centimetre).Border(0.5f, Unit.Centimetre).Text("Lab Service Charges for the From Date :" + result[0].fromDate + " to " + result[0].todate + " As per the Details Statement ").Style(TextStyle.Default.FontSize(10));
                                table.Cell().ColumnSpan(2).Height(0.5f, Unit.Centimetre).Border(0.5f, Unit.Centimetre).Text("" + result[0].rate).Style(TextStyle.Default.FontSize(10));


                                table.Cell().ColumnSpan(2).Height(0.5f, Unit.Centimetre).Border(0.5f, Unit.Centimetre).Text("Less Discount as per agreed terms ").Style(TextStyle.Default.FontSize(10));
                                table.Cell().ColumnSpan(2).Height(0.5f, Unit.Centimetre).Border(0.5f, Unit.Centimetre).Text(" 0 ").Style(TextStyle.Default.FontSize(10));


                                table.Cell().ColumnSpan(2).Height(0.5f, Unit.Centimetre).Border(0.5f, Unit.Centimetre).Text("Rupees In Words : " + MyFunction.AmountToWord((decimal)result[0].rate)).Style(TextStyle.Default.FontSize(10));
                                table.Cell().ColumnSpan(2).Height(0.5f, Unit.Centimetre).Border(0.5f, Unit.Centimetre).Text("" + result[0].rate).Style(TextStyle.Default.FontSize(10));

                                table.Cell().ColumnSpan(4).Height(0.5f, Unit.Centimetre).BorderLeft(0.5f, Unit.Centimetre).BorderRight(0.5f, Unit.Centimetre).BorderTop(0.5f, Unit.Centimetre).Text("Disclamers:").Style(TextStyle.Default.FontSize(10));
                                table.Cell().ColumnSpan(4).BorderLeft(0.5f, Unit.Centimetre).BorderRight(0.5f, Unit.Centimetre).Text("This is an electronically generated invoice, kindly acknowledge the invoce at the time of delivery. any discrepancies in the invoice should be notified to accounts department within 7 Days of the receipt of this invoice.").Style(TextStyle.Default.FontSize(10));
                                table.Cell().ColumnSpan(4).Height(0.5f, Unit.Centimetre).BorderLeft(0.5f, Unit.Centimetre).BorderRight(0.5f, Unit.Centimetre).Text("Contact Number: 011 - 44744388 ").Style(TextStyle.Default.FontSize(10));
                                table.Cell().ColumnSpan(4).Height(0.5f, Unit.Centimetre).BorderLeft(0.5f, Unit.Centimetre).BorderRight(0.5f, Unit.Centimetre).Text("Email: reddropdiagnostics@gmail.com ").Style(TextStyle.Default.FontSize(10));
                                table.Cell().ColumnSpan(4).Height(0.5f, Unit.Centimetre).BorderLeft(0.5f, Unit.Centimetre).BorderRight(0.5f, Unit.Centimetre).Text("Current A/C No.: 9971505728").Style(TextStyle.Default.FontSize(10));
                                table.Cell().ColumnSpan(4).Height(0.5f, Unit.Centimetre).BorderLeft(0.5f, Unit.Centimetre).BorderRight(0.5f, Unit.Centimetre).Text("").Style(TextStyle.Default.FontSize(10));
                                table.Cell().ColumnSpan(4).Height(0.5f, Unit.Centimetre).BorderLeft(0.5f, Unit.Centimetre).BorderRight(0.5f, Unit.Centimetre).Text("").Style(TextStyle.Default.FontSize(10));
                                table.Cell().ColumnSpan(4).Height(0.5f, Unit.Centimetre).BorderLeft(0.5f, Unit.Centimetre).BorderRight(0.5f, Unit.Centimetre).Text("").Style(TextStyle.Default.FontSize(10));
                                table.Cell().ColumnSpan(4).Height(0.5f, Unit.Centimetre).BorderLeft(0.5f, Unit.Centimetre).BorderRight(0.5f, Unit.Centimetre).Text("").Style(TextStyle.Default.FontSize(10));
                                table.Cell().ColumnSpan(4).Height(0.5f, Unit.Centimetre).BorderLeft(0.5f, Unit.Centimetre).BorderRight(0.5f, Unit.Centimetre).Text("").Style(TextStyle.Default.FontSize(10));
                                table.Cell().ColumnSpan(4).Height(0.5f, Unit.Centimetre).BorderLeft(0.5f, Unit.Centimetre).BorderRight(0.5f, Unit.Centimetre).Text("").Style(TextStyle.Default.FontSize(10));
                                table.Cell().ColumnSpan(4).Height(0.5f, Unit.Centimetre).BorderLeft(0.5f, Unit.Centimetre).BorderRight(0.5f, Unit.Centimetre).BorderBottom(0.5f, Unit.Centimetre).Text("").Style(TextStyle.Default.FontSize(10));

                            });
                        });

                             page.Footer().Height(1.5f, Unit.Centimetre)
                                .Column(column =>
                                {
                                    column.Item().Table(table =>
                                    {
                                        table.ColumnsDefinition(columns =>
                                        {
                                            columns.RelativeColumn();
                                            columns.RelativeColumn();
                                            columns.RelativeColumn();
                                        });
                                        table.Cell().AlignLeft().Text("");
                                        table.Cell().AlignLeft().Text("");
                                        table.Cell().AlignRight().AlignBottom().Text(text =>
                                        {
                                            text.DefaultTextStyle(x => x.FontSize(8));
                                            text.CurrentPageNumber();
                                            text.Span(" of ");
                                            text.TotalPages();
                                        });
                                    });
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

        public byte[] PrintInvoiceData(string InvoiceNo)
        {
            throw new NotImplementedException();
        }

        public byte[] PrintInvoiceDataExcel(string InvoiceNo)
        {
            var result = (from tb in db.tnx_Booking
                          join tbi in db.tnx_BookingItem on tb.workOrderId equals tbi.workOrderId
                          join dr in db.doctorReferalMaster on tb.refID1 equals dr.doctorId
                          where tb.invoiceNo == InvoiceNo
                          select new
                          {
                              BookingDate = tb.bookingDate.ToString("yyyy-MMM-dd"),
                              tb.workOrderId,
                              PatientName = tb.name,
                              ReferBy = dr.doctorName,
                              tbi.barcodeNo,
                              ItemName = tbi.investigationName,
                              rate = tbi.rate
                          }).ToList();
            var excelByte = MyFunction.ExportToExcel(result, "InvoiceDetail");
            return excelByte;
        }
    }
}
