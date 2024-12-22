using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Services
{

    public class itemObservationMappingServices : IitemObservationMappingServices
    {
        private readonly ContextClass db;

        public itemObservationMappingServices(ContextClass context, ILogger<BaseController<ItemObservationMapping>> logger)
        {

            db = context;

        }
        async public Task<ServiceStatusResponseModel> SaveObservationMapping(List<ItemObservationMapping> itemObservationMapping)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {

                    var newObservations = itemObservationMapping.Where(item => item.id == 0).Select(CreateObservationMapping).ToList();
                    if (newObservations.Any())
                    {
                        db.ItemObservationMapping.AddRange(newObservations);
                    }
                    var updatedObservations = itemObservationMapping.Where(item => item.id != 0).Select(itemObservationMappingItem =>
                        {
                            var itemObservationMappingold = db.ItemObservationMapping.FirstOrDefault(b => b.id == itemObservationMappingItem.id);
                            if (itemObservationMappingold != null)
                            {
                                UpdateObservationMapping(itemObservationMappingold, itemObservationMappingItem);
                                return itemObservationMappingold;
                            }
                            return null;
                        }).Where(item => item != null).ToList();

                    if (updatedObservations.Any())
                    {
                        db.ItemObservationMapping.UpdateRange(updatedObservations);
                    }

                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Data = itemObservationMapping
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Data = ex.Message
                    };
                }
            }
        }

        private ItemObservationMapping CreateObservationMapping(ItemObservationMapping itemObservationMapping)
        {
            return new ItemObservationMapping
            {
                id = itemObservationMapping.id,
                itemId = itemObservationMapping.itemId,
                itemObservationId = itemObservationMapping.itemObservationId,
                isTest = itemObservationMapping.isTest,
                isProfile = itemObservationMapping.isProfile,
                isPackage = itemObservationMapping.isPackage,
                itemType = itemObservationMapping.itemType,
                formula = itemObservationMapping.formula,
                dlcCheck = itemObservationMapping.dlcCheck,
                showInReport = itemObservationMapping.showInReport,
                isHeader = itemObservationMapping.isHeader,
                isBold = itemObservationMapping.isBold,
                isCritical = itemObservationMapping.isCritical,
                printSeparate = itemObservationMapping.printSeparate,
                mappedDate = itemObservationMapping.mappedDate


            };
        }
        private void UpdateObservationMapping(ItemObservationMapping ObservationMapping, ItemObservationMapping itemObservationMapping)
        {
            ObservationMapping.itemId = itemObservationMapping.itemId;
            ObservationMapping.itemObservationId = itemObservationMapping.itemObservationId;
            ObservationMapping.isTest = itemObservationMapping.isTest;
            ObservationMapping.isProfile = itemObservationMapping.isProfile;
            ObservationMapping.isPackage = itemObservationMapping.isPackage;
            ObservationMapping.itemType = itemObservationMapping.itemType;
            ObservationMapping.formula = itemObservationMapping.formula;
            ObservationMapping.dlcCheck = itemObservationMapping.dlcCheck;
            ObservationMapping.showInReport = itemObservationMapping.showInReport;
            ObservationMapping.isHeader = itemObservationMapping.isHeader;
            ObservationMapping.isBold = itemObservationMapping.isBold;
            ObservationMapping.isCritical = itemObservationMapping.isCritical;
            ObservationMapping.printSeparate = itemObservationMapping.printSeparate;
            ObservationMapping.mappedDate = itemObservationMapping.mappedDate;
        }
    }
}
