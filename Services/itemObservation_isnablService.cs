using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Migrations;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Services
{
    public class itemObservation_isnablService: IitemObservation_isnablService
    {
        private readonly ContextClass db;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        public itemObservation_isnablService(ContextClass context, ILogger<BaseController<itemObservation_isnabl>> logger, IConfiguration configuration, HttpClient httpClient)
        {
            db = context;
            this._configuration = configuration;
            this._httpClient = httpClient;
        }

        async Task<ServiceStatusResponseModel> IitemObservation_isnablService.GetNablData(int CentreId, int itemId)
        {
            try
            {
                var data = (from ion in db.itemObservation_isnabl
                            join im in db.itemMaster on ion.itemid equals im.itemId
                            join io in db.itemObservationMaster on ion.observationId equals io.id
                            join cm in db.centreMaster on ion.centreId equals cm.centreId
                            where ion.centreId == CentreId && ion.itemid == itemId
                            select new
                            {
                                ion.id,
                                ion.itemid,
                                centrename = cm.companyName,
                                im.itemName,
                                ObservationName= io.labObservationName,
                                ion.NablLogo
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

        async Task<ServiceStatusResponseModel> IitemObservation_isnablService.RemoveNabl(int id)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var data= db.itemObservation_isnabl.Where(i=>i.id==id).FirstOrDefault();
                    db.itemObservation_isnabl.Remove(data);
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "updated Successful"
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

        async Task<ServiceStatusResponseModel> IitemObservation_isnablService.SaveUpdateNabl(NablRequestModel Nabldata)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                   
                    var oldData = db.itemObservation_isnabl.Where(i=>i.itemid== Nabldata.itemid && Nabldata.observationId.Contains(i.observationId) && i.centreId== Nabldata.centreId).ToList();
                    db.itemObservation_isnabl.RemoveRange(oldData);
                    await db.SaveChangesAsync();
                    var newData = Nabldata.observationId.Select(observation => createNablData(observation, Nabldata)).ToList();
                    db.itemObservation_isnabl.AddRange(newData);
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "Saved Successful"
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

        private itemObservation_isnabl createNablData(int observationid,NablRequestModel model)
        {
            return new itemObservation_isnabl
            {
                id=0,
                observationId=observationid,
                itemid=model.itemid,
                centreId=model.centreId,
                IsDefaultLogo=model.IsDefaultLogo,
                isActive=model.isactive,
                createdById=model.createdById,
                createdDateTime=DateTime.Now,
                NablLogo= model.NablLogo
            };
        }
        async Task<ServiceStatusResponseModel> IitemObservation_isnablService.UploadNablLogo(IFormFile NablLogo, int centreId)
        {
            string extension = Path.GetExtension(NablLogo.FileName);
            if (extension != ".jpg" && extension != ".JPG" && extension != ".png" && extension != ".PNG" && extension != ".jpeg" && extension != ".JPEG")
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
                string uploadPath = Path.Combine(primaryFolder, "Image");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                string fileName = "NablImage_"+ centreId + extension;
                
                string filePath = Path.Combine(uploadPath, fileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await NablLogo.CopyToAsync(stream);
                }
                using (var transaction = await db.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var centredata= db.centreMaster.Where(c=>c.centreId==centreId).FirstOrDefault();
                        if (centredata != null)
                        {
                            centredata.NablImage = filePath;
                            db.centreMaster.Update(centredata);
                            await db.SaveChangesAsync();
                            await transaction.CommitAsync();
                        }
                        else
                        {
                            return new ServiceStatusResponseModel
                            {
                                Success = false,
                                Message = "Center id Not Found"
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

                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Message = "image uploaded and saved successfully.",
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
