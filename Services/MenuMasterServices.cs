using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Services
{
    public class MenuMasterServices : IMenuMasterServices
    {
        private readonly ContextClass db;
        public MenuMasterServices(ContextClass context, ILogger<BaseController<observationReferenceRanges>> logger)
        {
            db = context;
        }
        async Task<ServiceStatusResponseModel> IMenuMasterServices.SaveMenu(menuMaster MenuMaster)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var status = "";
                    if (MenuMaster.id == 0)
                    {
                        var menudata = CreateMenuData(MenuMaster);
                        db.menuMaster.Add(menudata);
                        status = "Menu Saved Succcessful";
                    }
                    else
                    {
                        var OldMenuData = db.menuMaster.FirstOrDefault(o => o.id == MenuMaster.id);
                        if (OldMenuData != null)
                        {
                            updateMenuData(MenuMaster, OldMenuData);
                            db.menuMaster.Update(OldMenuData);
                            status = "Menu Saved Succcessful";
                        }
                        else
                        {
                            return new ServiceStatusResponseModel
                            {
                                Success = false,
                                Message = " No Menu Data Found for update"
                            };
                        }
                    }
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = status
                    };

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = ex.Message,
                    };
                }
            }
        }

        private menuMaster CreateMenuData(menuMaster MenuMaster)
        {
            return new menuMaster
            {
                id = MenuMaster.id,
                menuName = MenuMaster.menuName,
                navigationUrl = MenuMaster.navigationUrl,
                parentId = MenuMaster.parentId,
                displaySequence = MenuMaster.displaySequence,
                isHide = MenuMaster.isHide,
                isActive = MenuMaster.isActive,
                createdById = MenuMaster.createdById,
                createdDateTime = MenuMaster.createdDateTime
            };
        }

        private void updateMenuData(menuMaster MenuMaster, menuMaster OldData)
        {
            OldData.menuName = MenuMaster.menuName;
            OldData.navigationUrl = MenuMaster.navigationUrl;
            OldData.parentId = MenuMaster.parentId;
            OldData.displaySequence = MenuMaster.displaySequence;
            OldData.isHide = MenuMaster.isHide;
            OldData.isActive = MenuMaster.isActive;
            OldData.updateById = MenuMaster.updateById;
            OldData.updateDateTime = MenuMaster.updateDateTime;
        }

        async Task<ServiceStatusResponseModel> IMenuMasterServices.UpdateMenuStatus(int menuId, byte Status)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var menudata= await db.menuMaster.Where(m=> m.id== menuId).FirstOrDefaultAsync();
                    menudata.isActive= Status;
                    db.menuMaster.Update(menudata);
                    await db.SaveChangesAsync();
                    transaction.Commit();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "Update successful"
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

        public async Task<ServiceStatusResponseModel> GetAllMenu(ODataQueryOptions<menuMaster> queryOptions)
        {
            try
            {
                var rawData = db.menuMaster.AsQueryable();

                var filteredData = (IQueryable<menuMaster>)queryOptions.Filter
                    .ApplyTo(rawData, new ODataQuerySettings
                    {
                        HandleNullPropagation = HandleNullPropagationOption.False
                    });

                var totalCount = await filteredData.CountAsync();

                var paginatedData = queryOptions.ApplyTo(filteredData, new ODataQuerySettings
                {
                    HandleNullPropagation = HandleNullPropagationOption.False
                }).Cast<menuMaster>();

                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = paginatedData,
                    Count = totalCount
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
