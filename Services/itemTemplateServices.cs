using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Services
{
    public class itemTemplateServices : IitemTemplateServices
    {
        private readonly ContextClass db;
        public itemTemplateServices(ContextClass context, ILogger<BaseController<itemTemplate>> logger)
        {
            db = context;
        }
        async Task<ServiceStatusResponseModel> IitemTemplateServices.SaveUpdateTemplate(itemTemplate Template)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    if (Template == null)
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "Interpretation data incorrect"
                        };
                    }

                    string msg = "";
                    if (Template.id == 0)
                    {
                        db.itemTemplate.Add(Template);
                        msg = "Saved successfully";
                    }
                    else
                    {
                        var data = await db.itemTemplate.Where(i => i.id == Template.id).FirstOrDefaultAsync();
                        if (data != null)
                        {
                            UpdateTemplateData(data, Template);
                            db.itemTemplate.Update(data);
                            msg = "Updated successfully";
                        }
                        else
                        {
                            return new ServiceStatusResponseModel
                            {
                                Success = false,
                                Message = "Wrong Interpretation ID"
                            };
                        }
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
                    await transaction.RollbackAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = "An error occurred: " + ex.Message
                    };
                }
            }
        }
        public void UpdateTemplateData(itemTemplate oldtemplate, itemTemplate newtemplate)
        {
            oldtemplate.Template = newtemplate.Template;
            oldtemplate.itemId = newtemplate.itemId;
            oldtemplate.CentreId = newtemplate.CentreId;
            oldtemplate.Name = newtemplate.Name;
            oldtemplate.gender= newtemplate.gender;
            oldtemplate.isActive= newtemplate.isActive;
            oldtemplate.updateDateTime = DateTime.Now;
            oldtemplate.updateById = newtemplate.updateById;
        }

        async Task<ServiceStatusResponseModel> IitemTemplateServices.UpdateTemplateStatus(int id, byte status, int Userid)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var itemData = await db.itemTemplate.Where(I => I.id == id).FirstOrDefaultAsync();
                    if (itemData == null)
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "Please Use Correct Item Id"
                        };
                    }
                    else
                    {
                        itemData.isActive = status;
                        itemData.updateById = Userid;
                        itemData.updateDateTime = DateTime.Now;
                        db.itemTemplate.Update(itemData);
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
                        Message = ex.Message
                    };
                }
            }
        }

        public async Task<ServiceStatusResponseModel> GetTemplateData(int CentreID, int testid)
        {
            var data = await (from it in db.itemTemplate
                              join im in db.itemMaster on it.itemId equals im.itemId
                              join cm in db.centreMaster on it.CentreId equals cm.centreId
                              where it.CentreId == CentreID && it.itemId == testid
                              select new
                              {
                                  it.id,
                                  cm.companyName,
                                  im.itemName,
                                  it.Template,
                                  it.Name,
                                  it.gender,
                                  it.isActive
                              }).ToListAsync();

            return new ServiceStatusResponseModel
            {
                Success = true,
                Data = data
            };
        }
    }
}
