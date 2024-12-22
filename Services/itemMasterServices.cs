using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Services
{
    public class itemMasterServices : IitemMasterServices
    {
        private readonly ContextClass db;
        public itemMasterServices(ContextClass context, ILogger<BaseController<empMaster>> logger)
        {
            db = context;
        }
        async Task<ServiceStatusResponseModel> IitemMasterServices.SaveItemMaster(itemMaster itemmaster)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    if (itemmaster.id == 0)
                    {
                        if (itemmaster.itemName == "")
                        {
                            return new ServiceStatusResponseModel
                            {
                                Success = false,
                                Message = "itemName Can't Be blank"
                            };
                        }
                        else
                        {
                            var exists = await db.itemMaster.AnyAsync(cm => cm.itemName == itemmaster.itemName);
                            if (exists)
                            {
                                return new ServiceStatusResponseModel
                                {
                                    Success = false,
                                    Message = "Duplicate itemName"
                                };
                            }
                        }
                        var ItemMasterData = CreateItem(itemmaster);
                        var ItemData = db.itemMaster.Add(ItemMasterData);
                        await db.SaveChangesAsync();
                        var itemId = ItemData.Entity.id;
                        if (itemmaster.itemType == 1)
                        {
                            var itemObservation = CreateItemObservation(itemmaster);
                            var itemObservationData = db.itemObservationMaster.Add(itemObservation);
                            await db.SaveChangesAsync();
                            var observationId = itemObservationData.Entity.id;
                            var ItemObservationMapping = CreateItemObservationMapping(itemId, observationId);
                            db.ItemObservationMapping.Add(ItemObservationMapping);
                        }
                        var itemSampletype = CreateItemSampletype(itemId, sampletype: (int)itemmaster.defaultsampletype);
                        db.itemSampleTypeMapping.Add(itemSampletype);
                        await db.SaveChangesAsync();

                    }
                    else
                    {
                        var ItemMaster = db.itemMaster.FirstOrDefault(im => im.id == itemmaster.id);
                        if (ItemMaster != null)
                        {
                            UpdateItemMaster(ItemMaster, itemmaster);
                        }
                        db.itemMaster.Update(ItemMaster);
                        await db.SaveChangesAsync();

                    }
                    await transaction.CommitAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "Saved Successful"
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = ex.InnerException?.Message ?? "An error occurred."
                    };
                }
            }
        }

        private itemMaster CreateItem(itemMaster itemmaster)
        {
            return new itemMaster
            {
                id = itemmaster.id,
                itemName = itemmaster.itemName,
                dispalyName = itemmaster.dispalyName,
                testMethod = itemmaster.testMethod,
                deptId = itemmaster.deptId,
                code = itemmaster.code,
                sortName = itemmaster.sortName,
                allowDiscont = itemmaster.allowDiscont,
                allowShare = itemmaster.allowShare,
                allowReporting = itemmaster.allowReporting,
                itemType = itemmaster.itemType,
                isOutsource = itemmaster.isOutsource,
                lmpRequire = itemmaster.lmpRequire,
                reportType = itemmaster.reportType,
                gender = itemmaster.gender,
                sampleVolume = itemmaster.sampleVolume,
                containerColor = itemmaster.containerColor,
                testRemarks = itemmaster.testRemarks,
                defaultsampletype = itemmaster.defaultsampletype,
                agegroup = itemmaster.agegroup,
                samplelogisticstemp = itemmaster.samplelogisticstemp,
                printsamplename = itemmaster.printsamplename,
                showinpatientreport = itemmaster.showinpatientreport,
                showinonlinereport = itemmaster.showinonlinereport,
                autosaveautoapprove = itemmaster.autosaveautoapprove,
                printseperate = itemmaster.printseperate,
                isorganism = itemmaster.isorganism,
                culturereport = itemmaster.culturereport,
                ismic = itemmaster.ismic,
                showOnWebsite = itemmaster.showOnWebsite,
                isSpecialItem = itemmaster.isSpecialItem,
                isAllergyTest = itemmaster.isAllergyTest,
                displaySequence = itemmaster.displaySequence,
                consentForm = itemmaster.consentForm,
                isActive=itemmaster.isActive,
                createdById = itemmaster.createdById,
                createdDateTime = itemmaster.createdDateTime
            };

        }

        private itemObservationMaster CreateItemObservation(itemMaster itemmaster)
        {
            return new itemObservationMaster
            {
                id = itemmaster.id,
                labObservationName = itemmaster.itemName,
                dlcCheck = 0,
                gender = itemmaster.gender,
                printSeparate = itemmaster.printseperate,
                shortName = itemmaster.sortName,
                roundUp = 0,
                method = itemmaster.testMethod,
                suffix = "",
                formula = "",
                observationWiseInterpretation = "",
                resultRequired = 1,
                collectionRequire = 1,
                displaySequence = itemmaster.displaySequence,
            };
        }

        private ItemObservationMapping CreateItemObservationMapping(int itemId, int observationId)
        {
            return new ItemObservationMapping
            {
                id = 0,
                itemId = itemId,
                itemObservationId = observationId,
                itemType = 1,
                isTest = 1,
                isPackage = 0,
                isProfile = 0,
                isBold = 0,
                isCritical = 0,
                isHeader = 0,
                formula = "",
                dlcCheck = 0,
                showInReport = 1
            };
        }

        private itemSampleTypeMapping CreateItemSampletype(int itemId, int sampletype)
        {
            var sampleTypeName = (from sm in db.sampletype_master
                                  where sm.id == sampletype
                                  select sm.sampleTypeName).ToString();

            return new itemSampleTypeMapping
            {
                id = 0,
                sampleTypeId = sampletype,
                sampleTypeName = sampleTypeName,
                isDefault = 1
            };
        }

        private void UpdateItemMaster(itemMaster ItemMaster, itemMaster itemmaster)
        {
            ItemMaster.itemName = itemmaster.itemName;
            ItemMaster.dispalyName = itemmaster.dispalyName;
            ItemMaster.testMethod = itemmaster.testMethod;
            ItemMaster.deptId = itemmaster.deptId;
            ItemMaster.code = itemmaster.code;
            ItemMaster.sortName = itemmaster.sortName;
            ItemMaster.allowDiscont = itemmaster.allowDiscont;
            ItemMaster.allowShare = itemmaster.allowShare;
            ItemMaster.allowReporting = itemmaster.allowReporting;
            ItemMaster.itemType = itemmaster.itemType;
            ItemMaster.isOutsource = itemmaster.isOutsource;
            ItemMaster.isActive = itemmaster.isActive;
            ItemMaster.lmpRequire = itemmaster.lmpRequire;
            ItemMaster.reportType = itemmaster.reportType;
            ItemMaster.gender = itemmaster.gender;
            ItemMaster.sampleVolume = itemmaster.sampleVolume;
            ItemMaster.containerColor = itemmaster.containerColor;
            ItemMaster.testRemarks = itemmaster.testRemarks;
            ItemMaster.defaultsampletype = itemmaster.defaultsampletype;
            ItemMaster.agegroup = itemmaster.agegroup;
            ItemMaster.samplelogisticstemp = itemmaster.samplelogisticstemp;
            ItemMaster.printsamplename = itemmaster.printsamplename;
            ItemMaster.showinpatientreport = itemmaster.showinpatientreport;
            ItemMaster.showinonlinereport = itemmaster.showinonlinereport;
            ItemMaster.autosaveautoapprove = itemmaster.autosaveautoapprove;
            ItemMaster.printseperate = itemmaster.printseperate;
            ItemMaster.isorganism = itemmaster.isorganism;
            ItemMaster.culturereport = itemmaster.culturereport;
            ItemMaster.ismic = itemmaster.ismic;
            ItemMaster.showOnWebsite = itemmaster.showOnWebsite;
            ItemMaster.isSpecialItem = itemmaster.isSpecialItem;
            ItemMaster.isAllergyTest = itemmaster.isAllergyTest;
            ItemMaster.displaySequence = itemmaster.displaySequence;
            ItemMaster.consentForm = itemmaster.consentForm;
        }
    }
}
