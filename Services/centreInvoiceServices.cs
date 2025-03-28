
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
                        var maxId = db.centreInvoice.Any() ? db.centreInvoice.Max(c => c.id) + 1 : 1;
                        var invoice = $"Imarsar/{DateTime.Now:yyyy-MM}/{maxId}";
                        var invoiceMasterData = CreateInvoiceData(CentreInvoice, invoice);
                        var InvoiceData = db.centreInvoice.Add(invoiceMasterData);
                        await db.SaveChangesAsync();

                        var invoiceno = db.centreInvoice.Select(c => c.id).Max() + 1;

                        var InvoiceNo = InvoiceData.Entity.id;
                        if (InvoiceNo > 0)
                        {
                            var transactionids = CentreInvoice.LabNo.Split(',').Select(int.Parse).ToList();
                            var invoiceItemData = db.tnx_BookingItem
                                .Where(b => transactionids.Contains(b.transactionId) && string.IsNullOrEmpty(b.invoiceNo))
                                .ToList();

                            if (invoiceItemData.Any())
                            {
                                foreach (var item in invoiceItemData)
                                {
                                    UpdateItemMasterCreateInvoice(item, CentreInvoice, invoice);
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
                                .Where(b => b.invoiceNo== CentreInvoice.invoiceNo)
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

        private centreInvoice CreateInvoiceData(centreInvoiceRequestModel CentreInvoice, string invoiceNo)
        {
            return new centreInvoice
            {
                id = CentreInvoice.id,
                invoiceNo = invoiceNo,
                InvoiceDate = CentreInvoice.InvoiceDate,
                centreid = CentreInvoice.centreid,
                fromDate = CentreInvoice.fromDate,
                toDate = CentreInvoice.toDate,
                rate = CentreInvoice.rate,
                createDate = CentreInvoice.createDate,
                createdBy = CentreInvoice.createdBy
            };
        }

        private void UpdateItemMasterCreateInvoice(tnx_BookingItem InvoiceItem, centreInvoiceRequestModel CentreInvoiceRequestModel, string invoiceNo)
        {
            InvoiceItem.invoiceNo = invoiceNo; // Ensure correct type handling
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
                                             TransactionId = string.Join(",", g.Select(t => t.tbi.transactionId)), // Fix applied
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
                              Invoice = cp.InvoiceDate.ToString("MMMM-yyyy"),
                          }).FirstOrDefault();

            if (result != null)
            {
                QuestPDF.Settings.License = LicenseType.Community;
                var image1 = _configuration["FileBase64:CompanyLogo1"];
                var image1Bytes = Convert.FromBase64String(image1.Split(',')[1]);

                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(2, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontFamily("TimesNewRoman").FontSize(10));

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
                                 table.Cell().ColumnSpan(2).AlignCenter().Text(result.CentreName).Style(TextStyle.Default.FontSize(10));
                                 table.Cell().ColumnSpan(2).AlignCenter().Text("").Style(TextStyle.Default.FontSize(10));
                                 table.Cell().ColumnSpan(6).AlignCenter().Text("").Style(TextStyle.Default.FontSize(10).Bold().Underline());
                                 table.Cell().ColumnSpan(6).BorderBottom(1.0f, Unit.Point);
                             });
                        });

                            page.Content()
                            .PaddingVertical(10)
                            .Column(column =>
                            {
                                column.Item().Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                    });
                                    table.Cell().ColumnSpan(4).Height(0.5f, Unit.Centimetre).Height(0.5f, Unit.Centimetre).Text("");

                                    table.Cell().ColumnSpan(2).Height(0.5f, Unit.Centimetre).Border(0.5f, Unit.Point).Text(" Invoice: " + result.Invoice).Bold();
                                    table.Cell().Height(0.5f, Unit.Centimetre).Border(0.5f, Unit.Point).Text(" Invoice No: ");
                                    table.Cell().Height(0.5f, Unit.Centimetre).Border(0.5f, Unit.Point).Text(" Invoice Date: ");
                                    table.Cell().ColumnSpan(2).Height(0.5f, Unit.Centimetre).Border(0.5f, Unit.Point).Text(result.CentreName).Bold();
                                    table.Cell().Height(0.5f, Unit.Centimetre).Border(0.5f, Unit.Point).Text(" " + result.invoiceNo);
                                    table.Cell().Height(0.5f, Unit.Centimetre).Border(0.5f, Unit.Point).Text(" " + result.InvoiceDate);
                                    table.Cell().ColumnSpan(4).Height(0.5f, Unit.Centimetre).Height(0.5f, Unit.Centimetre).Text("");
                                });

                                column.Item().Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn(3);
                                        columns.RelativeColumn(1);
                                    });



                                    table.Cell().Height(0.5f, Unit.Centimetre).Border(0.5f, Unit.Point).AlignCenter().Text("  Particulers");
                                    table.Cell().Height(0.5f, Unit.Centimetre).Border(0.5f, Unit.Point).AlignCenter().Text("  Amount (Rs.)");
                                    table.Cell().Height(0.5f, Unit.Centimetre).Border(0.5f, Unit.Point).Text(" Lab Service Charges for " + result.fromDate + " to " + result.todate);
                                    table.Cell().Height(0.5f, Unit.Centimetre).Border(0.5f, Unit.Point).AlignCenter().Text(result.rate.ToString("F2"));
                                    table.Cell().Height(0.5f, Unit.Centimetre).Border(0.5f, Unit.Point).Text(" Less Discount (if any):");
                                    table.Cell().Height(0.5f, Unit.Centimetre).Border(0.5f, Unit.Point).AlignCenter().Text("0");
                                    table.Cell().Height(0.5f, Unit.Centimetre).Border(0.5f, Unit.Point).Text(" Rupees In Words" + MyFunction.AmountToWord((decimal)result.rate));
                                    table.Cell().Height(0.5f, Unit.Centimetre).Border(0.5f, Unit.Point).AlignCenter().Text(result.rate.ToString("F2"));
                                    table.Cell().Height(1.5f, Unit.Centimetre).Text("");
                                    table.Cell().Height(1.5f, Unit.Centimetre).Text("");

                                });
                                column.Item().Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn(4);
                                    });
                                    table.Cell().Text("Disclaimer:").Style(TextStyle.Default.FontSize(10)).Bold();
                                    table.Cell().Text("This is an electronically generated invoice, kindly acknowledge the invoce at the time of delivery. any discrepancies in the invoice should be notified to accounts department within 7 Days of the receipt of this invoice.").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().Text("Contact Number: ").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().Text("Email.").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().Text("Current A/c: ").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().Text("Bank.").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().Text("RIFSG Code:").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().Text("GST No.").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().Text("PAN No.").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().Text("Paymnets: Cheque / Cash should be drawn in favour of .").Style(TextStyle.Default.FontSize(10));



                                });



                            });

                        page.Footer()
                            .AlignCenter()
                            .Text("*This is a computer generated Invoice and doesn’t require a signature/stamp.")
                            .FontSize(8).Bold();
                    });
                });

                return document.GeneratePdf();
            }

            return new byte[0];
        }

        public byte[] PrintInvoiceData(string InvoiceNo)
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

            var resultitem = (from tb in db.tnx_Booking
                              join tbi in db.tnx_BookingItem on tb.workOrderId equals tbi.workOrderId
                              join dr in db.doctorReferalMaster on tb.refID1 equals dr.doctorId
                              where tbi.invoiceNo == InvoiceNo
                              select new
                              {
                                  BookingDate = tb.bookingDate.ToString("yyyy-MMM-dd"),
                                  tb.workOrderId,
                                  PatientName = tb.name,
                                  ReferBy = dr.doctorName,
                                  tbi.barcodeNo,
                                  ItemName = tbi.investigationName,
                                  Mrp = tbi.mrp,
                                  rate = tbi.rate
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
                                   columns.RelativeColumn();
                                   columns.RelativeColumn();
                                   columns.RelativeColumn();
                                   columns.RelativeColumn();
                               });

                               table.Cell().ColumnSpan(4).Height(0.5f, Unit.Centimetre).Height(0.5f, Unit.Centimetre).Text("");
                               table.Cell().ColumnSpan(2).Height(0.5f, Unit.Centimetre).Border(0.5f, Unit.Point).Text(" Invoice: " + result[0].invoiceNo).Bold();
                               table.Cell().Height(0.5f, Unit.Centimetre).Border(0.5f, Unit.Point).Text(" Invoice No: ");
                               table.Cell().Height(0.5f, Unit.Centimetre).Border(0.5f, Unit.Point).Text(" Invoice Date: ");
                               table.Cell().ColumnSpan(2).Height(0.5f, Unit.Centimetre).Border(0.5f, Unit.Point).Text(result[0].CentreName).Bold();
                               table.Cell().Height(0.5f, Unit.Centimetre).Border(0.5f, Unit.Point).Text(" " + result[0].invoiceNo);
                               table.Cell().Height(0.5f, Unit.Centimetre).Border(0.5f, Unit.Point).Text(" " + result[0].InvoiceDate);
                               table.Cell().ColumnSpan(4).Height(0.5f, Unit.Centimetre).Height(0.5f, Unit.Centimetre).Text("");
                           });

                           column.Item().Table(table =>
                           {
                               table.ColumnsDefinition(columns =>
                               {
                                   columns.RelativeColumn(3);
                                   columns.RelativeColumn(1);
                               });



                               table.Cell().Height(0.5f, Unit.Centimetre).Border(0.5f, Unit.Point).AlignCenter().Text("Particulers");
                               table.Cell().Height(0.5f, Unit.Centimetre).Border(0.5f, Unit.Point).AlignCenter().Text("  Amount (Rs.)");
                               table.Cell().Height(0.5f, Unit.Centimetre).Border(0.5f, Unit.Point).Text(" Lab Service Charges for " + result[0].fromDate + " to " + result[0].todate);
                               table.Cell().Height(0.5f, Unit.Centimetre).Border(0.5f, Unit.Point).AlignCenter().Text(result[0].rate.ToString("F2"));
                               table.Cell().Height(0.5f, Unit.Centimetre).Border(0.5f, Unit.Point).Text(" Less Discount (if any):");
                               table.Cell().Height(0.5f, Unit.Centimetre).Border(0.5f, Unit.Point).AlignCenter().Text("0");
                               table.Cell().Height(0.5f, Unit.Centimetre).Border(0.5f, Unit.Point).Text(" Rupees In Words" + MyFunction.AmountToWord((decimal)result[0].rate));
                               table.Cell().Height(0.5f, Unit.Centimetre).Border(0.5f, Unit.Point).AlignCenter().Text(result[0].rate.ToString("F2"));
                               table.Cell().Height(1.5f, Unit.Centimetre).Text("");
                               table.Cell().Height(1.5f, Unit.Centimetre).Text("");

                           });
                           column.Item().Table(table =>
                           {
                               table.ColumnsDefinition(columns =>
                               {
                                   columns.RelativeColumn(4);
                               });
                     
                               table.Cell().Text("Disclaimer:").Style(TextStyle.Default.FontSize(10)).Bold();
                               table.Cell().Text("This is an electronically generated invoice, kindly acknowledge the invoce at the time of delivery. any discrepancies in the invoice should be notified to accounts department within 7 Days of the receipt of this invoice.").Style(TextStyle.Default.FontSize(10));
                               table.Cell().Text("Contact Number: ").Style(TextStyle.Default.FontSize(10));
                               table.Cell().Text("Email.").Style(TextStyle.Default.FontSize(10));
                               table.Cell().Text("Current A/c: ").Style(TextStyle.Default.FontSize(10));
                               table.Cell().Text("Bank.").Style(TextStyle.Default.FontSize(10));
                               table.Cell().Text("RIFSG Code:").Style(TextStyle.Default.FontSize(10));
                               table.Cell().Text("GST No.").Style(TextStyle.Default.FontSize(10));
                               table.Cell().Text("PAN No.").Style(TextStyle.Default.FontSize(10));
                               table.Cell().Text("Paymnets: Cheque / Cash should be drawn in favour of .").Style(TextStyle.Default.FontSize(10));


                           });
                           column.Item().PageBreak();
                           column.Item().Table(table =>
                           {
                               table.ColumnsDefinition(columns =>
                               {
                                   columns.ConstantColumn(2.0f, Unit.Centimetre);
                                   columns.ConstantColumn(3.0f, Unit.Centimetre);
                                   columns.RelativeColumn();
                                   columns.ConstantColumn(2.0f, Unit.Centimetre);
                                   columns.ConstantColumn(2.0f, Unit.Centimetre);
                                   columns.ConstantColumn(2.0f, Unit.Centimetre);
                               });

                               table.Cell().Height(0.5f, Unit.Centimetre).BorderBottom(0.5f, Unit.Point).BorderTop(0.5f, Unit.Point).Text("Visit Id").Style(TextStyle.Default.FontSize(10));
                               table.Cell().Height(0.5f, Unit.Centimetre).BorderBottom(0.5f, Unit.Point).BorderTop(0.5f, Unit.Point).Text("BookingDate").Style(TextStyle.Default.FontSize(10));
                               table.Cell().Height(0.5f, Unit.Centimetre).BorderBottom(0.5f, Unit.Point).BorderTop(0.5f, Unit.Point).Text("Investigstiom").Style(TextStyle.Default.FontSize(10));
                               table.Cell().Height(0.5f, Unit.Centimetre).BorderBottom(0.5f, Unit.Point).BorderTop(0.5f, Unit.Point).Text("BarCodeNo").Style(TextStyle.Default.FontSize(10));
                               table.Cell().Height(0.5f, Unit.Centimetre).BorderBottom(0.5f, Unit.Point).BorderTop(0.5f, Unit.Point).Text("MRP").Style(TextStyle.Default.FontSize(10));
                               table.Cell().Height(0.5f, Unit.Centimetre).BorderBottom(0.5f, Unit.Point).BorderTop(0.5f, Unit.Point).Text("Rate").Style(TextStyle.Default.FontSize(10));
                               foreach (var item in resultitem)
                               {
                                   table.Cell().Height(0.5f, Unit.Centimetre).Text(item.workOrderId).Style(TextStyle.Default.FontSize(10));
                                   table.Cell().Height(0.5f, Unit.Centimetre).Text(item.BookingDate).Style(TextStyle.Default.FontSize(10));
                                   table.Cell().Height(0.5f, Unit.Centimetre).Text(item.ItemName).Style(TextStyle.Default.FontSize(10));
                                   table.Cell().Height(0.5f, Unit.Centimetre).Text(item.barcodeNo).Style(TextStyle.Default.FontSize(10));
                                   table.Cell().Height(0.5f, Unit.Centimetre).Text(item.Mrp.ToString()).Style(TextStyle.Default.FontSize(10));
                                   table.Cell().Height(0.5f, Unit.Centimetre).Text(item.rate.ToString()).Style(TextStyle.Default.FontSize(10));
                               }

                           });
                       });

                            page.Footer()
                             .AlignCenter()
                             .Text("*This is a computer generated Invoice and doesn’t require a signature/stamp.")
                             .FontSize(8).Bold();
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

        public byte[] PrintInvoiceDataExcel(string InvoiceNo)
        {
            var result = (from tb in db.tnx_Booking
                          join tbi in db.tnx_BookingItem on tb.workOrderId equals tbi.workOrderId
                          join dr in db.doctorReferalMaster on tb.refID1 equals dr.doctorId
                          where tbi.invoiceNo == InvoiceNo
                          select new
                          {
                              BookingDate = tb.bookingDate.ToString("yyyy-MMM-dd"),
                              tb.workOrderId,
                              PatientName = tb.name,
                              ReferBy = dr.doctorName,
                              tbi.barcodeNo,
                              ItemName = tbi.investigationName,
                              Mrp = tbi.mrp,
                              rate = tbi.rate
                          }).ToList();
            var excelByte = MyFunction.ExportToExcel(result, "InvoiceDetail");
            return excelByte;
        }

        async Task<ServiceStatusResponseModel> IcentreInvoiceServices.GetInvoices(DateTime FromDate, DateTime Todate, List<int> CentreId)
        {

            try
            {
                var invoiceData = await (from ci in db.centreInvoice
                                         join cm in db.centreMaster on ci.centreid equals cm.centreId
                                         join em in db.empMaster on ci.createdBy equals em.empId
                                         where CentreId.Contains(cm.centreId)
                                         && ci.createDate >= FromDate && ci.createDate <= Todate
                                         select new
                                         {
                                             ci.id,
                                             ci.invoiceNo,
                                             fromDate = ci.fromDate.ToString("yyyy-MMM-dd hh:mm tt"),
                                             ToDate = ci.toDate.ToString("yyyy-MMM-dd hh:mm tt"),
                                             InvoiceDate = ci.InvoiceDate.ToString("yyyy-MMM-dd"),
                                             ci.rate,
                                             createDate = ci.createDate.Value.ToString("yyyy-MMM-dd hh:mm tt"),
                                             createdBy = string.Concat(em.fName, " ", em.lName),
                                             ci.isCancel
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
    }
}
