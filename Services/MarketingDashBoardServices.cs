using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Services
{
    public class MarketingDashBoardServices : IMarketingDashBoardServices
    {
        private readonly ContextClass db;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        public MarketingDashBoardServices(ContextClass context, ILogger<BaseController<MarketingDashBoard>> logger, IConfiguration configuration, HttpClient httpClient)
        {

            db = context;
            this._configuration = configuration;
            this._httpClient = httpClient;
        }

        async Task<ServiceStatusResponseModel> IMarketingDashBoardServices.GetDashBoardData(string type)
        {
            try
            {
                var data= await (from dm in db.MarketingDashBoard
                                 where dm.type==type
                                 select new
                                 {
                                     dm.id,
                                     dm.type,
                                     dm.Pdf,
                                     dm.Image,
                                     dm.isActive,
                                     dm.Description,
                                     dm.Subject
                                 }).ToListAsync();
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = data
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

        async Task<ServiceStatusResponseModel> IMarketingDashBoardServices.ViewMarketingDashboard(string type)
        {
            try
            {
                var data = (from dm in db.MarketingDashBoard
                                  where dm.isActive == 1 &&  dm.type == type
                                select new
                                  {
                                      dm.id,
                                      dm.type,
                                      dm.Pdf,
                                      dm.Image,
                                      dm.Description,
                                      dm.Subject
                                  }).AsEnumerable() // Execute the query and switch to in-memory
                            .Select(dm => new
                            {
                                dm.id,dm.type,dm.Subject,dm.Description,
                                Image = ConverttoBAse64(dm.Image),
                                dm.Pdf 
                                
                            }).ToList();
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = data
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

        private static string ConverttoBAse64(string imagepath)
        {
            if (imagepath != null && imagepath != "")
            {
                byte[] imageBytes = File.ReadAllBytes(imagepath);
                string image = Convert.ToBase64String(imageBytes);
                return image;
            }
            else
            {
                return "";
            }
        }
        async Task<ServiceStatusResponseModel> IMarketingDashBoardServices.DeactiveDashboardImage(int id, int userid, byte status)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var data = db.MarketingDashBoard.Where(m => m.id == id).FirstOrDefault();
                    if (data != null)
                    {
                        data.isActive = status;
                        data.updateDateTime = DateTime.Now;
                        data.updateById = userid;
                        db.MarketingDashBoard.Update(data);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "updated Successful"
                        };
                    }
                    else
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "No data to Update"
                        };
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
        }

        async Task<ServiceStatusResponseModel> IMarketingDashBoardServices.SaveUpdateDashboard(MarketingDashBoard DashboardData)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    if(DashboardData.id==0)
                    {
                        db.MarketingDashBoard.Add(DashboardData);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Saved Successful"
                        };
                    }
                    else
                    {
                        db.MarketingDashBoard.Update(DashboardData);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Updated Successful"
                        };
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
        }

        async Task<ServiceStatusResponseModel> IMarketingDashBoardServices.UploadDocument(IFormFile Document)
        {
            string extension = Path.GetExtension(Document.FileName);
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
                string uploadPath = Path.Combine(primaryFolder, "MarketingDashboard");
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
