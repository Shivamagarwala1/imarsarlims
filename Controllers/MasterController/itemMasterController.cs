using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
using static Google.Cloud.Dialogflow.V2.Intent.Types.Message.Types.CarouselSelect.Types;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class itemMasterController : BaseController<itemMaster>
    {
        private readonly ContextClass db;
        private readonly IitemMasterServices _itemMasterServices;

        public itemMasterController(ContextClass context, ILogger<BaseController<itemMaster>> logger, IitemMasterServices itemMasterServices) : base(context, logger)
        {
            db = context;
            this._itemMasterServices = itemMasterServices;
        }
        protected override IQueryable<itemMaster> DbSet => db.itemMaster.AsNoTracking().OrderBy(o => o.itemId);

        [HttpPost("SaveItemMaster")]
        public async Task<ServiceStatusResponseModel> SaveItemMaster(itemMaster itemmaster)
        {
            try
            {
                var result = await _itemMasterServices.SaveItemMaster(itemmaster);
                return result;
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = ex.InnerException?.Message ?? "An error occurred."
                };
            }
        }
        [HttpPost("updateItemStatus")]
        public async Task<ServiceStatusResponseModel> updateItemStatus(int ItemId,byte Status, int UserId)
        {
            try
            {
                var result = await _itemMasterServices.updateItemStatus(ItemId,Status,UserId);
                return result;
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = ex.InnerException?.Message ?? "An error occurred."
                };
            }
        }
        [HttpGet("GetItemMaster")]
        public async Task<ServiceStatusResponseModel> GetItemMaster(int ItemId)
        {
            var result = await db.itemMaster.Where(p => p.itemId == ItemId)
                              .Include(p => p.AddSampletype).FirstOrDefaultAsync();
            if (result == null)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = "Please enter correct ItemId"
                };
            }
            return new ServiceStatusResponseModel
            {
                Success = true,
                Data = result
            };
        }

        [HttpGet("GetItemMasterAll")]
        public async Task<ServiceStatusResponseModel> GetItemMasterAll()
        {
            try
            {
                var result = await _itemMasterServices.GetItemMasterAll();
                return result;
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Message= ex.Message
                };
            }
        }

        [HttpGet("GetItemForTemplate")]
        public async Task<ServiceStatusResponseModel> GetItemForTemplate()
        {
            try
            {
                var result = await _itemMasterServices.GetItemForTemplate();
                return result;
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
        [HttpGet("GetProfileObservation")]
        public async Task<ServiceStatusResponseModel> GetProfileObservation(int itemId)
        {
            try
            {
                var result = (from om in db.itemObservationMaster
                              join omm in db.ItemObservationMapping on om.id equals omm.observationID
                              where omm.itemId == itemId
                              select new
                              {
                                  om.id,
                                  om.labObservationName
                              }).ToList();

                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = result
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

        [HttpGet("GetItemObservation")]
        public async Task<ServiceStatusResponseModel> GetItemObservation(int itemtype)
        {
            try
            {
                var result = await _itemMasterServices.GetItemObservation(itemtype);
                return result;
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

        [HttpGet("GetMappedItem")]
        public async Task<ServiceStatusResponseModel> GetMappedItem(int itemtype,int itemid)
        {
            try
            {
                var result = await _itemMasterServices.GetMappedItem(itemtype, itemid);
                return result;
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
        [HttpGet("RemoveMapping")]
        public async Task<ServiceStatusResponseModel> RemoveMapping(int Id)
        {
            try
            {
                var result = await _itemMasterServices.RemoveMapping(Id);
                return result;
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
        [HttpGet("EvaluateTest")]
        public async Task<ServiceStatusResponseModel> EvaluateTest(int itemid1, int itemid2,int itemid3)
        {
            try
            {
                var result = await _itemMasterServices.EvaluateTest(itemid1,itemid2,itemid3);
                return result;
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
}

