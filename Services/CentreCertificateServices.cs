using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using iText.IO.Image;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace iMARSARLIMS.Services
{
    public class CentreCertificateServices: ICentreCertificateServices
    {
        private readonly ContextClass db;

        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        public CentreCertificateServices(ContextClass context, ILogger<BaseController<CentreCertificate>> logger, IConfiguration configuration, HttpClient httpClient)
        {
            db = context;

            this._configuration = configuration;
            this._httpClient = httpClient;
        }

        public byte[] DownloadCertificate(int Id)
        {
            var data = (from cm in db.centreMaster
                        join cc in db.CentreCertificate on cm.centreId equals cc.centreId
                        where cc.id == Id
                        select new
                        {
                            certificateID = cc.id,
                            centreName = cm.companyName,
                            cm.address,
                            CertificateImage = string.IsNullOrEmpty(cm.CertificateImage) ?
                                               (db.centreMaster.Where(c => c.centreId == 1).Select(c => c.CertificateImage).FirstOrDefault()) :
                                               cm.CertificateImage,

                            FromDate =  cc.certificateDate.ToString("dd-MMM-yyyy"),
                            ToDate = cc.certificateDate.AddYears(1).AddDays(-1).ToString("dd-MMM-yyyy")
                        }).ToList();


            if (data.Count != 0)
            {
                QuestPDF.Settings.License = LicenseType.Community;

                var document = Document.Create(container =>
                {

                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);

                        page.MarginTop(7.1f, Unit.Centimetre);

                        page.MarginLeft(0.5f, Unit.Centimetre);
                        page.MarginRight(0.5f, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.Foreground();

                        page.Background().Image(File.ReadAllBytes(data[0].CertificateImage));
                        page.DefaultTextStyle(x => x.FontFamily("TimesNewRoman"));
                        page.DefaultTextStyle(x => x.FontSize(10));
                        page.Content()
                        .Column(column =>
                        {

                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(11.0f,Unit.Centimetre); // 1st column
                                    columns.ConstantColumn(8.0f, Unit.Centimetre); // 2nd column
                                                                                   // If you want to add more columns, you can define them here.
                                });

                                // Adjusted column spans to match the defined columns.
                                table.Cell().ColumnSpan(2).Height(0.5f, Unit.Centimetre).Text(data[0].centreName).Style(TextStyle.Default.FontSize(10).Bold()).AlignCenter();
                                table.Cell().ColumnSpan(2).Height(0.8f, Unit.Centimetre).Text("").Style(TextStyle.Default.FontSize(10).Bold()).AlignCenter();
                                table.Cell().ColumnSpan(2).Height(0.5f, Unit.Centimetre).Text(data[0].address).Style(TextStyle.Default.FontSize(10).Bold()).AlignCenter();
                                table.Cell().ColumnSpan(2).Height(2.7f, Unit.Centimetre).Text("").Style(TextStyle.Default.FontSize(10).Bold()).AlignCenter();
                                table.Cell().Height(0.5f, Unit.Centimetre).Text("" + data[0].FromDate).Style(TextStyle.Default.FontSize(10).Bold()).AlignRight();
                                table.Cell().Height(0.5f, Unit.Centimetre).Text("      " + data[0].ToDate).Style(TextStyle.Default.FontSize(10).Bold()).AlignLeft();


                            });

                        });

                        page.Footer().Height(2.5f, Unit.Centimetre)
                           .Column(column =>
                           {
                               column.Item().Text("");
                           });
                    });

                });
                byte[] pdfBytes = document.GeneratePdf();
                return pdfBytes;
            }
            else
            {
                byte[] pdfbyte = new byte[0]; // Fixed initialization of empty byte array
                return pdfbyte;
            }

        }

        public async Task<ServiceStatusResponseModel> GetAggreement(int centreId)
        {
            try
            {
                var data = await (from cm in db.centreMaster
                                  join cc in db.CentreCertificate on cm.centreId equals cc.centreId into ccGroup
                                  from cc in ccGroup.DefaultIfEmpty() // This is the left join
                                  where cm.centreId == centreId
                                  select new
                                  {
                                      certificateID = cc != null ? cc.id : 0,  
                                      cm.centreId,
                                      centreName = cm.companyName,
                                      Aggrement = !string.IsNullOrEmpty(cm.Aggreement)? cm.Aggreement:"",
                                      certificateDate = cc != null ? cc.certificateDate.ToString("yyyy-MMM-dd") : "" // Handle null case for certificateDate
                                  }).ToListAsync();

                if (data != null)
                {
                    return new ServiceStatusResponseModel { Success = true, Data = data };
                }
                else
                {
                    return new ServiceStatusResponseModel { Success = false, Message = "No data Found" };
                }
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

        async Task<ServiceStatusResponseModel> ICentreCertificateServices.GenerateCertificate(CentreCertificate Certificate)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    if (Certificate != null)
                    {
                        db.CentreCertificate.Add(Certificate);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Certificated Genrated Successful"
                        };
                    }
                    else
                    {
                        return new ServiceStatusResponseModel { Success = false, Message = "No data to genrate certificate" };
                    }
                }
                catch (Exception ex)
                {
                    return new ServiceStatusResponseModel { Success = false, Message = ex.Message };

                }
            }
        }

        async Task<ServiceStatusResponseModel> ICentreCertificateServices.UploadCertificate(int CentreId, string Certificate)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var data = db.centreMaster.Where(c => c.centreId == CentreId).FirstOrDefault();
                    if (data != null)
                    {
                        data.CertificateImage = Certificate;
                       
                        db.centreMaster.Update(data);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel { Success = true, Message = "Saved Successfull" };
                    }
                    else
                    {
                        return new ServiceStatusResponseModel { Success = true, Message = "Centre Not Found" };
                    }
                }
                catch (Exception ex)
                {
                    return new ServiceStatusResponseModel { Success = true, Message = ex.Message };

                }
            }
        }

        async Task<ServiceStatusResponseModel> ICentreCertificateServices.UploadAggreement(int CentreId, string AggrimenstDocumnet)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var data = db.centreMaster.Where(c => c.centreId == CentreId).FirstOrDefault();
                    if (data != null)
                    {
                        
                        data.Aggreement = AggrimenstDocumnet;
                        db.centreMaster.Update(data);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel { Success = true, Message = "Saved Successfull" };
                    }
                    else
                    {
                        return new ServiceStatusResponseModel { Success = true, Message = "Centre Not Found" };
                    }
                }
                catch (Exception ex)
                {
                    return new ServiceStatusResponseModel { Success = true, Message = ex.Message };

                }
            }
        }

        async Task<ServiceStatusResponseModel> ICentreCertificateServices.UploadDocument(IFormFile file)
        {
            string extension = Path.GetExtension(file.FileName);
            if (extension != ".pdf" && extension != ".Pdf" && extension != ".PDF" && extension != ".jpg" && extension != ".png" && extension != ".JPG" && extension != ".PNG" && extension != ".JPEG" && extension != ".jpeg")
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = "No valid file extension Found"
                };
            }
            try
            {
                string primaryFolder = _configuration["DocumentPath:PrimaryFolder"];
                if (!Directory.Exists(primaryFolder))
                {
                    Directory.CreateDirectory(primaryFolder);
                }
                string uploadPath = Path.Combine(primaryFolder, "CentreAggrementCertificate");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                string fileName = Guid.NewGuid().ToString() + extension;
                string filePath = Path.Combine(uploadPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Message = "File uploaded and saved successfully.",
                    Data = new { FilePath = filePath }
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
