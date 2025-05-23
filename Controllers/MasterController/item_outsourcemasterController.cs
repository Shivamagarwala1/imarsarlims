﻿using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class item_outsourcemasterController : BaseController<item_outsourcemaster>
    {
        private readonly ContextClass db;
        private readonly IitemOutSourceServices _itemOutSourceServices;

        public item_outsourcemasterController(ContextClass context, ILogger<BaseController<item_outsourcemaster>> logger, IitemOutSourceServices itemOutSourceServices) : base(context, logger)
        {
            db = context;
            this._itemOutSourceServices = itemOutSourceServices;
        }
        protected override IQueryable<item_outsourcemaster> DbSet => db.item_outsourcemaster.AsNoTracking().OrderBy(o => o.id);


        [HttpPost("SaveOutSourceMapping")]
        public async Task<ServiceStatusResponseModel> SaveOutSourceMapping(List<item_outsourcemaster> OutSourceMapping)
        {
            try
            {
                var result = await _itemOutSourceServices.SaveOutSourceMapping(OutSourceMapping);
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
        [HttpGet("GetOutSourceMapping")]
        public async Task<ServiceStatusResponseModel> GetOutSourceMapping(int BookingCentre, int OutSourceLab, int DeptId)
        {
            try
            {
                var result = await _itemOutSourceServices.GetOutSourceMapping(BookingCentre, OutSourceLab, DeptId);
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
        [HttpPost("RemoveOutSourceMapping")]
        public async Task<ServiceStatusResponseModel> RemoveOutSourceMapping(int id)
        {
            try
            {
                var result = await _itemOutSourceServices.RemoveOutSourceMapping(id);
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

    }
}
