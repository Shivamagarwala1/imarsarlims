using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MySqlX.XDevAPI.Common;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace iMARSARLIMS.Services
{
    public class doctorReferalServices : IdoctorReferalServices
    {
        private readonly ContextClass db;
        public doctorReferalServices(ContextClass context, ILogger<BaseController<doctorReferalMaster>> logger)
        {
            db = context;
        }

        async Task<ServiceStatusResponseModel> IdoctorReferalServices.ReferDoctorData()
        {
            try
            {
                var doctorData = await (from dr in db.doctorReferalMaster
                                        join cm in db.centreMaster on dr.centreID equals cm.centreId
                                        select new
                                        {
                                            dr.doctorId,
                                            dr.centreID,
                                            dr.title,
                                            dr.degreeName,
                                            dr.doctorName,
                                            dr.doctorCode,
                                            dr.imaRegistartionNo,
                                            dr.address1,
                                            dr.address2,
                                            dr.mobileNo,
                                            dr.isActive
                                        }).ToListAsync();
                if (doctorData != null)
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Data = doctorData
                    };
                }
                else
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = "Data not found"
                    };
                }

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

        async Task<ServiceStatusResponseModel> IdoctorReferalServices.UpdateReferDoctorStatus(int DoctorId, byte status, int UserId)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var doctorData= db.doctorReferalMaster.Where(d=> d.doctorId== DoctorId).FirstOrDefault();
                    if (doctorData != null)
                    {
                        doctorData.isActive = status;
                        doctorData.updateById = UserId;
                        doctorData.updateDateTime= DateTime.Now;
                        db.doctorReferalMaster.Update(doctorData);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Updated Successful"
                        };
                    }
                    else
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "Doctor Not Found"
                        };
                    }

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

        async Task<ServiceStatusResponseModel> IdoctorReferalServices.SaveUpdateReferDoctor(doctorReferalMaster refDoc)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var msg = "";
                    if(refDoc.doctorId== 0)
                    {
                        db.doctorReferalMaster.Add(refDoc);
                        msg = "Saved Successful"; 
                    }
                    else
                    {
                        db.doctorReferalMaster.Update(refDoc);
                        msg = "Updated Successful";
                    }
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = msg
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

        async Task<ServiceStatusResponseModel> IdoctorReferalServices.DoctorBussinessReport(string DoctorId, string centreID, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                List<int> DoctorIds = DoctorId.Split(',').Select(int.Parse).ToList();
                List<int> CentreIds = centreID.Split(',').Select(int.Parse).ToList();
                var Query = from tb in db.tnx_Booking
                                  join dr in db.doctorReferalMaster on tb.refID1 equals dr.doctorId
                                  where tb.bookingDate >= FromDate && tb.bookingDate <= ToDate
                                  select new
                                  {
                                      BookingDate = tb.bookingDate.ToString("yyyy-MMM-dd hh:mm tt"),
                                      patientName = tb.name,
                                      tb.workOrderId,
                                      dr.doctorName,
                                      dr.doctorId,
                                      tb.grossAmount,
                                      tb.centreId,
                                      tb.discount,
                                      tb.netAmount,
                                      tb.paidAmount,
                                  };
                if (DoctorIds.Count > 0)
                {
                    Query = Query.Where(q => DoctorIds.Contains(q.doctorId));
                }
                if (CentreIds.Count > 0)
                {
                    Query = Query.Where(q => CentreIds.Contains(q.centreId));
                }
                var data= await Query.ToListAsync();
                if (data.Count > 0)
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Data = data
                    };
                }
                else
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = "No Data Found"
                    };
                }

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

        async Task<ServiceStatusResponseModel> IdoctorReferalServices.DoctorBussinessReportSummury(string DoctorId, string centreID, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                List<int> DoctorIds = DoctorId.Split(',').Select(int.Parse).ToList();
                List<int> CentreIds = centreID.Split(',').Select(int.Parse).ToList();
                var Query = from tb in db.tnx_Booking
                            join dr in db.doctorReferalMaster on tb.refID1 equals dr.doctorId
                            where tb.bookingDate >= FromDate && tb.bookingDate <= ToDate && CentreIds.Contains(tb.centreId)
                            group tb by new { dr.doctorId, dr.doctorName } into g
                            select new
                            {
                                DoctorId = g.Key.doctorId,
                                DoctorName = g.Key.doctorName,
                                TotalGrossAmount = g.Sum(x => x.grossAmount),
                                TotalDiscount = g.Sum(x => x.discount),
                                TotalNetAmount = g.Sum(x => x.netAmount),
                                TotalPaidAmount = g.Sum(x => x.paidAmount),
                            };

                if (DoctorIds.Count > 0)
                {
                    Query = Query.Where(q => DoctorIds.Contains(q.DoctorId));
                }
                

                var data = await Query.ToListAsync();

                if (data.Count > 0)
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Data = data
                    };
                }
                else
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = "No Data Found"
                    };
                }
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

        public byte[] DoctorBussinessReportPdf(string DoctorId, string centreID, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                List<int> DoctorIds = DoctorId.Split(',').Select(int.Parse).ToList();
                List<int> CentreIds = centreID.Split(',').Select(int.Parse).ToList();
                // Query for data
                var Query = from tb in db.tnx_Booking
                            join dr in db.doctorReferalMaster on tb.refID1 equals dr.doctorId
                            where tb.bookingDate >= FromDate && tb.bookingDate <= ToDate && CentreIds.Contains(tb.centreId)
                            select new
                            {
                                BookingDate = tb.bookingDate.ToString("yyyy-MMM-dd hh:mm tt"),
                                patientName = tb.name,
                                tb.workOrderId,
                                dr.doctorName,
                                dr.doctorId,
                                tb.grossAmount,
                                tb.centreId,
                                tb.discount,
                                tb.netAmount,
                                tb.paidAmount,
                            };
                if (DoctorIds.Count > 0)
                {
                    Query = Query.Where(q => DoctorIds.Contains(q.doctorId));
                }
               
                var data = Query.ToList();

                // If no data found, return an empty PDF
                if (data.Count == 0)
                {
                    return new byte[0]; // Zero-byte return when no data exists
                }

                QuestPDF.Settings.License = LicenseType.Community;

                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(0.5f, Unit.Centimetre);
                        page.PageColor(Colors.White);

                        // Page Header
                        page.Header().Column(column =>
                        {
                            column.Item().Text("Collection Report").Style(TextStyle.Default.FontSize(16).Bold());
                        });

                        // Table Layout
                        page.Content().Table(table =>
                        {
                            // Define the columns
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(1f, Unit.Centimetre); // # column
                                columns.ConstantColumn(2f, Unit.Centimetre); // Visit ID column
                                columns.RelativeColumn();  // Patient Name
                                columns.ConstantColumn(2f, Unit.Centimetre);  // Gross column
                                columns.ConstantColumn(2f, Unit.Centimetre);  // Discount column
                                columns.ConstantColumn(2f, Unit.Centimetre);
                                columns.ConstantColumn(2f, Unit.Centimetre);// Net column
                            });

                            // Add table header
                            table.Cell().BorderBottom(0.5f, Unit.Point).BorderTop(0.5f, Unit.Point).Text("#").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().BorderBottom(0.5f, Unit.Point).BorderTop(0.5f, Unit.Point).Text("VisitId").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().BorderBottom(0.5f, Unit.Point).BorderTop(0.5f, Unit.Point).Text("Patient Name").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().BorderBottom(0.5f, Unit.Point).BorderTop(0.5f, Unit.Point).Text("Gross").Style(TextStyle.Default.FontSize(10).Bold()).AlignRight();
                            table.Cell().BorderBottom(0.5f, Unit.Point).BorderTop(0.5f, Unit.Point).Text("Discount").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().BorderBottom(0.5f, Unit.Point).BorderTop(0.5f, Unit.Point).Text("Net").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().BorderBottom(0.5f, Unit.Point).BorderTop(0.5f, Unit.Point).Text("Paid").Style(TextStyle.Default.FontSize(10).Bold());

                            // Populate table rows
                            int rowNumber = 1;
                            int DoctorId = 0;
                            foreach (var item in data)
                            {
                                if(DoctorId!=item.doctorId)
                                {
                                    table.Cell().ColumnSpan(7).Border(0.5f,Unit.Point).Text(item.doctorName).Style(TextStyle.Default.FontSize(10)).AlignCenter();  // Visit ID
                                    DoctorId = item.doctorId;
                                }
                                table.Cell().Text(rowNumber.ToString()).Style(TextStyle.Default.FontSize(10));  // Serial number
                                table.Cell().Text(item.workOrderId).Style(TextStyle.Default.FontSize(10));  // Visit ID
                                table.Cell().Text(item.patientName).Style(TextStyle.Default.FontSize(10));  // Patient Name
                                table.Cell().Text(item.grossAmount.ToString("0.00")).Style(TextStyle.Default.FontSize(10)).AlignRight();  // Gross
                                table.Cell().Text(item.discount.ToString("0.00")).Style(TextStyle.Default.FontSize(10));  // Discount
                                table.Cell().Text(item.netAmount.ToString("0.00")).Style(TextStyle.Default.FontSize(10));  // Net
                                table.Cell().Text(item.paidAmount.ToString("0.00")).Style(TextStyle.Default.FontSize(10));  // Net

                                rowNumber++;
                            }
                        });

                        // Page Footer
                        page.Footer().Column(column =>
                        {
                            column.Item().AlignCenter().Text(text =>
                            {
                                text.DefaultTextStyle(x => x.FontSize(8));
                                text.CurrentPageNumber();
                                text.Span(" of ");
                                text.TotalPages();
                            });
                        });
                    });
                });

                // Generate the PDF byte array
                byte[] pdfBytes = document.GeneratePdf();

                // Save the PDF to file for debugging (optional)
                File.WriteAllBytes("discount_report.pdf", pdfBytes);

                return pdfBytes;  // Return the generated PDF
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                // Return empty byte array in case of error
                return new byte[0];
            }
        }

        public byte[] DoctorBussinessReportSummuryPdf(string DoctorId, string centreID, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                List<int> DoctorIds = DoctorId.Split(',').Select(int.Parse).ToList();
                List<int> CentreIds = centreID.Split(',').Select(int.Parse).ToList();
                // Query for data
                var Query = from tb in db.tnx_Booking
                            join dr in db.doctorReferalMaster on tb.refID1 equals dr.doctorId
                            where tb.bookingDate >= FromDate && tb.bookingDate <= ToDate && CentreIds.Contains(tb.centreId)
                            group tb by new { dr.doctorId, dr.doctorName } into g
                           
                            select new
                            {
                                DoctorId = g.Key.doctorId,
                                DoctorName = g.Key.doctorName,
                                TotalGrossAmount = g.Sum(x => x.grossAmount),
                                TotalDiscount = g.Sum(x => x.discount),
                                TotalNetAmount = g.Sum(x => x.netAmount),
                                TotalPaidAmount = g.Sum(x => x.paidAmount),
                            };

                if (DoctorIds.Count > 0)
                {
                    Query = Query.Where(q => DoctorIds.Contains(q.DoctorId));
                }
               

                var data = Query.ToList();

                // If no data found, return an empty PDF
                if (data.Count == 0)
                {
                    return new byte[0]; // Zero-byte return when no data exists
                }

                QuestPDF.Settings.License = LicenseType.Community;

                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(0.5f, Unit.Centimetre);
                        page.PageColor(Colors.White);

                        // Page Header
                        page.Header().Column(column =>
                        {
                            column.Item().Text("Doctor Collection Report Summury").Style(TextStyle.Default.FontSize(16).Bold()).AlignCenter();
                        });

                        // Table Layout
                        page.Content().Table(table =>
                        {
                            // Define the columns
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(1f, Unit.Centimetre); // # column
                                columns.ConstantColumn(2f, Unit.Centimetre); // Visit ID column
                                columns.RelativeColumn();  // Patient Name
                                columns.ConstantColumn(2f, Unit.Centimetre);  // Gross column
                                columns.ConstantColumn(2f, Unit.Centimetre);  // Discount column
                                columns.ConstantColumn(2f, Unit.Centimetre);  // Net column
                            });

                            // Add table header
                            table.Cell().BorderBottom(0.5f, Unit.Point).BorderTop(0.5f, Unit.Point).Text("#").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().BorderBottom(0.5f, Unit.Point).BorderTop(0.5f, Unit.Point).Text("DoctorName").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().BorderBottom(0.5f, Unit.Point).BorderTop(0.5f, Unit.Point).Text("Gross").Style(TextStyle.Default.FontSize(10).Bold()).AlignRight();
                            table.Cell().BorderBottom(0.5f, Unit.Point).BorderTop(0.5f, Unit.Point).Text("Discount").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().BorderBottom(0.5f, Unit.Point).BorderTop(0.5f, Unit.Point).Text("Net").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().BorderBottom(0.5f, Unit.Point).BorderTop(0.5f, Unit.Point).Text("Paid").Style(TextStyle.Default.FontSize(10).Bold());

                            
                            int rowNumber = 1;
                            foreach (var item in data)
                            {
                                table.Cell().Text(rowNumber.ToString()).Style(TextStyle.Default.FontSize(10));  // Serial number
                                table.Cell().Text(item.DoctorName).Style(TextStyle.Default.FontSize(10));  // Visit ID
                                table.Cell().Text(item.TotalGrossAmount.ToString("0.00")).Style(TextStyle.Default.FontSize(10));  // Patient Name
                                table.Cell().Text(item.TotalDiscount.ToString("0.00")).Style(TextStyle.Default.FontSize(10)).AlignRight();  // Gross
                                table.Cell().Text(item.TotalNetAmount.ToString("0.00")).Style(TextStyle.Default.FontSize(10));  // Discount
                                table.Cell().Text(item.TotalPaidAmount.ToString("0.00")).Style(TextStyle.Default.FontSize(10));  // Net
                                rowNumber++;
                            }
                        });

                        // Page Footer
                        page.Footer().Column(column =>
                        {
                            column.Item().AlignCenter().Text(text =>
                            {
                                text.DefaultTextStyle(x => x.FontSize(8));
                                text.CurrentPageNumber();
                                text.Span(" of ");
                                text.TotalPages();
                            });
                        });
                    });
                });

                // Generate the PDF byte array
                byte[] pdfBytes = document.GeneratePdf();

                // Save the PDF to file for debugging (optional)
                File.WriteAllBytes("discount_report.pdf", pdfBytes);

                return pdfBytes;  // Return the generated PDF
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                // Return empty byte array in case of error
                return new byte[0];
            }
        }


        public byte[] DoctorBussinessReportExcel(string DoctorId, string centreID, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                List<int> DoctorIds = DoctorId.Split(',').Select(int.Parse).ToList();
                List<int> CentreIds = centreID.Split(',').Select(int.Parse).ToList();
                // Query for data
                var Query = from tb in db.tnx_Booking
                            join dr in db.doctorReferalMaster on tb.refID1 equals dr.doctorId
                            where tb.bookingDate >= FromDate && tb.bookingDate <= ToDate
                            select new
                            {
                                BookingDate = tb.bookingDate.ToString("yyyy-MMM-dd hh:mm tt"),
                                patientName = tb.name,
                                tb.workOrderId,
                                dr.doctorName,
                                dr.doctorId,
                                tb.grossAmount,
                                tb.centreId,
                                tb.discount,
                                tb.netAmount,
                                tb.paidAmount,
                            };
                if (DoctorIds.Count > 0)
                {
                    Query = Query.Where(q => DoctorIds.Contains(q.doctorId));
                }
                if (CentreIds.Count > 0)
                {
                    Query = Query.Where(q => CentreIds.Contains(q.centreId));
                }
                var data = Query.ToList();

                var excelByte = MyFunction.ExportToExcel(data, "LedgerReportExcel");
                return excelByte;
            }
            catch (Exception ex)
            {
                return new byte[0];
            }
        }

        public byte[] DoctorBussinessReportSummuryExcel(string DoctorId, string centreID, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                List<int> DoctorIds = DoctorId.Split(',').Select(int.Parse).ToList();
                List<int> CentreIds = centreID.Split(',').Select(int.Parse).ToList();
                // Query for data
                var Query = from tb in db.tnx_Booking
                            join dr in db.doctorReferalMaster on tb.refID1 equals dr.doctorId
                            where tb.bookingDate >= FromDate && tb.bookingDate <= ToDate &&  CentreIds.Contains(tb.centreId)
                            group tb by new { dr.doctorId, dr.doctorName } into g
                            select new
                            {
                                DoctorId = g.Key.doctorId,
                                DoctorName = g.Key.doctorName,
                                TotalGrossAmount = g.Sum(x => x.grossAmount),
                                TotalDiscount = g.Sum(x => x.discount),
                                TotalNetAmount = g.Sum(x => x.netAmount),
                                TotalPaidAmount = g.Sum(x => x.paidAmount),
                            };

                if (DoctorIds.Count > 0)
                {
                    Query = Query.Where(q => DoctorIds.Contains(q.DoctorId));
                }
               

                var data = Query.ToList();
                var excelByte = MyFunction.ExportToExcel(data, "LedgerReportExcel");
                return excelByte;


            }
            catch (Exception ex)
            {
                return new byte[0];
            }
        }
    }
}
