using System.Linq;
using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
                    var msg = "Saved Successful";
                    if (itemmaster.itemId == 0)
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
                        var itemId = ItemData.Entity.itemId;
                        if (itemmaster.itemType == 1)
                        {
                            var itemObservation = CreateItemObservation(itemmaster);
                            var itemObservationData = db.itemObservationMaster.Add(itemObservation);
                            await db.SaveChangesAsync();
                            var observationId = itemObservationData.Entity.id;
                            var ItemObservationMapping = CreateItemObservationMapping(itemId, observationId);
                            db.ItemObservationMapping.Add(ItemObservationMapping);
                            await db.SaveChangesAsync();
                        }
                        await CreateItemSampletype(itemId, itemmaster.AddSampletype);
                    }
                    else
                    {
                        var ItemMaster = db.itemMaster.FirstOrDefault(im => im.itemId == itemmaster.itemId);
                        if (ItemMaster != null)
                        {
                            UpdateItemMaster(ItemMaster, itemmaster);
                        }
                        db.itemMaster.Update(ItemMaster);
                        await db.SaveChangesAsync();
                        await UpdateSampletypedata(itemmaster.AddSampletype, itemmaster.itemId);
                        msg = "Updated Successful";
                    }
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
                        Message = ex.InnerException?.Message ?? "An error occurred."
                    };
                }
            }
        }
        private itemMaster CreateItem(itemMaster itemmaster)
        {
            return new itemMaster
            {
                itemId = itemmaster.itemId,
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
                DocumentId = itemmaster.DocumentId,
                isSpecialItem = itemmaster.isSpecialItem,
                isAllergyTest = itemmaster.isAllergyTest,
                displaySequence = itemmaster.displaySequence,
                consentForm = itemmaster.consentForm,
                isActive = itemmaster.isActive,
                createdById = itemmaster.createdById,
                createdDateTime = itemmaster.createdDateTime
            };
        }
        private itemObservationMaster CreateItemObservation(itemMaster itemmaster)
        {
            return new itemObservationMaster
            {
                id = 0,
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
                isActive= itemmaster.isActive,
                createdById= itemmaster.createdById,
                createdDateTime= itemmaster.createdDateTime,
                displaySequence = itemmaster.displaySequence,
            };
        }
        private ItemObservationMapping CreateItemObservationMapping(int itemId, int observationId)
        {
            return new ItemObservationMapping
            {
                id = 0,
                itemId = itemId,
                observationID = observationId,
                itemType = 1,
                isTest = 1,
                isPackage = 0,
                isProfile = 0,
                isBold = 0,
                isCritical = 0,
                isHeader = 0,
                formula = "",
                dlcCheck = 0,
                showInReport = 1,
                createdById = 0,
                createdDateTime = DateTime.Now
            };
        }
        private async Task<ServiceStatusResponseModel> CreateItemSampletype(int itemId, IEnumerable<itemSampleTypeMapping> sampletypeList)
        {
            if (sampletypeList != null)
            {
                var sampleDataList = sampletypeList.Select(sampletype => CreateSampleTypeData(sampletype, itemId)).ToList();
                if (sampleDataList.Any())
                {
                    db.itemSampleTypeMapping.AddRange(sampleDataList);
                    await db.SaveChangesAsync();
                }
            }
            return new ServiceStatusResponseModel
            {
                Success = true,
            };
        }
        private itemSampleTypeMapping CreateSampleTypeData(itemSampleTypeMapping sampletype, int itemId)
        {
            return new itemSampleTypeMapping
            {
                id = 0,
                itemId = itemId,
                sampleTypeId = sampletype.sampleTypeId,
                sampleTypeName = sampletype.sampleTypeName,
                isActive = sampletype.isActive,
                isDefault = sampletype.isDefault,
                createdById = sampletype.createdById,
                createdDateTime = sampletype.createdDateTime,
            };
        }
        private async Task<ServiceStatusResponseModel> UpdateSampletypedata(IEnumerable<itemSampleTypeMapping> sampletypedata, int ItemId)
        {
            var sampleTypeData = db.itemSampleTypeMapping.Where(e => e.itemId == ItemId).ToList();
            db.itemSampleTypeMapping.RemoveRange(sampleTypeData);
            await db.SaveChangesAsync();
            if (sampletypedata != null)
            {
                var SamplemappingList = sampletypedata.Select(sample => CreateSampleTypeData(sample, ItemId)).ToList();
                if (SamplemappingList.Any())
                {
                    db.itemSampleTypeMapping.AddRange(SamplemappingList);
                    await db.SaveChangesAsync();
                }
            }
            return new ServiceStatusResponseModel
            {
                Success = true,
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
            ItemMaster.DocumentId = itemmaster.DocumentId;
            ItemMaster.isSpecialItem = itemmaster.isSpecialItem;
            ItemMaster.isAllergyTest = itemmaster.isAllergyTest;
            ItemMaster.displaySequence = itemmaster.displaySequence;
            ItemMaster.consentForm = itemmaster.consentForm;
        }
        async Task<ServiceStatusResponseModel> IitemMasterServices.updateItemStatus(int ItemId, byte Status, int UserId)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var itemData = await db.itemMaster.Where(I => I.itemId == ItemId).FirstOrDefaultAsync();
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
                        itemData.isActive = Status;
                        itemData.updateById = UserId;
                        itemData.updateDateTime = DateTime.Now;
                        db.itemMaster.Update(itemData);
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

        async Task<ServiceStatusResponseModel> IitemMasterServices.GetItemMasterAll()
        {
            try
            {
                var itemdata = await (from im in db.itemMaster
                                      join dm in db.labDepartment on im.deptId equals dm.id
                                      join stm in db.sampletype_master on im.defaultsampletype equals stm.id
                                      select new
                                      {
                                          im.itemId,
                                          itemCode= im.code,
                                          itemType = im.itemType == 1 ? "Test" : im.itemType == 2 ? "Profile" : "Package",
                                          im.itemName,
                                          dm.deptName,
                                          stm.sampleTypeName,
                                          Reporttype = im.reportType == 1 ? "Numeric" : im.reportType == 2 ? "TextReport" : im.reportType == 3 ? "Radiology" : im.reportType == 4 ? "Microbiology" : im.reportType == 5 ? "Historeport" : "Not Required",
                                          im.isActive
                                      }).ToListAsync();
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = itemdata
                };
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
        async Task<ServiceStatusResponseModel> IitemMasterServices.GetItemObservation(int itemtype)
        {
            try
            {
                if (itemtype == 1 || itemtype == 2)
                {
                    var ObservationData = await db.itemObservationMaster
                          .Where(l => l.isActive == 1)
                          .Select(l => new
                          {
                              l.id,
                              l.labObservationName
                          }).ToListAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Data = ObservationData
                    };
                }
                else
                {
                    var ObservationData = await db.itemMaster
                         .Where(l => l.isActive == 1 && (l.itemType == 1 || l.itemType == 2))
                         .Select(l => new
                         {
                             id = l.itemId,
                             labObservationName = l.itemName
                         }).ToListAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Data = ObservationData
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Data = ex.Message
                };
            }
        }

        async Task<ServiceStatusResponseModel> IitemMasterServices.GetMappedItem(int itemtype, int itemid)
        {

            try
            {
                if (itemtype == 1 || itemtype == 2)
                {
                    var ObservationData = await (
                             from iom in db.ItemObservationMapping
                             join lom in db.itemObservationMaster on iom.observationID equals lom.id
                             where iom.itemId == itemid
                             orderby iom.printOrder
                             select new
                             {
                                 iom.id,
                                 observationID=lom.id,
                                 lom.labObservationName,
                                 iom.dlcCheck,
                                 iom.isBold,
                                 iom.isHeader,
                                 iom.isCritical,
                                 iom.showInReport,
                                 iom.printSeparate,
                                 iom.printOrder
                             }).ToListAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Data = ObservationData
                    };
                }
                else
                {
                    var ObservationData = await (
                            from iom in db.ItemObservationMapping
                            join im in db.itemMaster on iom.observationID equals im.itemId
                            where iom.itemId == itemid
                            orderby iom.printOrder
                            select new
                            {
                                iom.id,
                                observationID= im.itemId,
                                labObservationName= im.itemName,
                                iom.dlcCheck,
                                iom.isBold,
                                iom.isHeader,
                                iom.isCritical,
                                iom.showInReport,
                                iom.printSeparate,
                                iom.printOrder
                            }).ToListAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Data = ObservationData
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Data = ex.Message
                };
            }
        }

        async Task<ServiceStatusResponseModel> IitemMasterServices.RemoveMapping(int Id)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var itemData = await db.ItemObservationMapping.Where(I => I.id == Id).FirstOrDefaultAsync();
                    if (itemData == null)
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "Please Use Correct Id"
                        };
                    }
                    else
                    {
                        db.ItemObservationMapping.Remove(itemData);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Removed Successful"
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

        async Task<ServiceStatusResponseModel> IitemMasterServices.GetItemForTemplate()
        {
            var reporttype = new List<int> { 2, 3,4,5 };
            try
            {
                var itemdata = await (from im in db.itemMaster
                                      where reporttype.Contains(im.reportType ?? 0) // Use .Contains() for checking multiple values
                                      select new
                                      {
                                          im.itemId,
                                          im.itemName,
                                      }).ToListAsync();

                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = itemdata
                };
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,  // Change to false to indicate failure
                    Message = ex.Message
                };
            }
        }

        async Task<ServiceStatusResponseModel> IitemMasterServices.EvaluateTest(int itemid1, int itemid2, int itemid3)
        {
            var data1 = (from iom in db.ItemObservationMapping
                        join im in db.itemMaster on iom.observationID equals im.itemId
                        where iom.itemId== itemid1
                        select new
                        {
                            ItemId = iom.itemId,
                            TestName = im.itemName,
                            Rate = (from rt in db.rateTypeWiseRateList
                                    where rt.itemid == itemid1 && rt.rateTypeId == 1
                                    select rt.rate).FirstOrDefault()
                        }).ToList();

            var data2 = (from iom in db.ItemObservationMapping
                        join im in db.itemMaster on iom.observationID equals im.itemId
                        where iom.itemId == itemid2
                        select new
                        {
                            ItemId = iom.itemId,
                            TestName = im.itemName,
                            Rate = (from rt in db.rateTypeWiseRateList
                                    where rt.itemid == itemid2 && rt.rateTypeId == 1
                                    select rt.rate).FirstOrDefault()
                        }).ToList();

            var data3 = (from iom in db.ItemObservationMapping
                         join im in db.itemMaster on iom.observationID equals im.itemId
                         where iom.itemId == itemid3
                         select new
                         {
                             ItemId = iom.itemId,
                             TestName = im.itemName,
                             Rate = (from rt in db.rateTypeWiseRateList
                                     where rt.itemid == itemid3 && rt.rateTypeId == 1
                                     select rt.rate).FirstOrDefault()
                         }).ToList();

            var groupedData = new
            {
                item1 = data1,
                item2 = data2,
                item3 = data3
            };

            return new ServiceStatusResponseModel
            {
                Success = true,
                Data = groupedData
            };

        }

        public byte[] DownloadDOS()
        {
            try
            {
                var query = from im in db.itemMaster
                join rt in db.rateTypeWiseRateList on im.itemId equals rt.itemid
                where  rt.rateTypeId==1
            select new
            {
                im.itemName,
                rt.mrp, rt.rate
            };

                

                var result = query.ToList();


                // If no data found, return an empty PDF
                if (result == null)
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
                                columns.RelativeColumn();// Patient Name
                                columns.RelativeColumn();  // Booking Date
                                
                            });

                            // Add table header
                            table.Cell().Text("#").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().Text("Item Name").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().Text("Mrp").Style(TextStyle.Default.FontSize(10).Bold());
                            table.Cell().Text("Rate").Style(TextStyle.Default.FontSize(10).Bold());


                            // Populate table rows
                            int rowNumber = 1;
                            var itemname = "";
                            foreach (var item in result)
                            {
                                
                                table.Cell().Text(""+ rowNumber).Style(TextStyle.Default.FontSize(10));
                                table.Cell().Text(item.itemName.ToString()).Style(TextStyle.Default.FontSize(10));
                                table.Cell().Text(item.mrp.ToString()).Style(TextStyle.Default.FontSize(10));
                                table.Cell().Text(item.rate.ToString()).Style(TextStyle.Default.FontSize(10));

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
                File.WriteAllBytes("collection_report.pdf", pdfBytes);

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

        async Task<ServiceStatusResponseModel> IitemMasterServices.GetDOS()
        {
            try
            {
                var query = await ( from im in db.itemMaster
                            join rt in db.rateTypeWiseRateList on im.itemId equals rt.itemid
                            where rt.rateTypeId == 1
                            select new
                            {
                                im.itemName,
                                rt.mrp,
                                rt.rate
                            }).ToListAsync();
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = query
                };
            }
            catch(Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }
    }
}
