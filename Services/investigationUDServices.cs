using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Services
{
    public class investigationUDServices : IinvestigationUDServices
    {
        private readonly ContextClass db;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        public investigationUDServices(ContextClass context, ILogger<BaseController<CentreCertificate>> logger, IConfiguration configuration, HttpClient httpClient)
        {
            db = context;

            this._configuration = configuration;
            this._httpClient = httpClient;
        }
        async Task<ServiceStatusResponseModel> IinvestigationUDServices.RemoveInvestigationFormat(int Id)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var data= db.InvestigationMasterUD.Where(i=>i.id==Id).FirstOrDefault();
                    db.InvestigationMasterUD.Remove(data);
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "Format Removed Successful"
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = ex.Message
                    };
                }
            }
        }

        async Task<ServiceStatusResponseModel> IinvestigationUDServices.UpdateInvestigationFormat(InvestigationMasterUD FormatData)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    if (FormatData.id == 0)
                    {
                        db.InvestigationMasterUD.Add(FormatData);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Saved Successssful"
                        };
                    }
                    else
                    {
                        db.InvestigationMasterUD.Update(FormatData);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Updated Successssful"
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = ex.Message
                    };
                }
            }
        }

        async Task<ServiceStatusResponseModel> IinvestigationUDServices.UploadDocument(IFormFile Document)
        {
            string extension = Path.GetExtension(Document.FileName);
            if (extension != ".pdf" && extension != ".Pdf" && extension != ".PDF")
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
                string uploadPath = Path.Combine(primaryFolder, "ReportFormat");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                string fileName = Guid.NewGuid().ToString() + extension;
                string filePath = Path.Combine(uploadPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Document.CopyToAsync(stream);
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
