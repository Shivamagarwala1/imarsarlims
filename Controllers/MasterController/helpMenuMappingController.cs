﻿using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class helpMenuMappingController : BaseController<helpMenuMapping>
    {
        private readonly ContextClass db;
        private readonly IhelpMenuMasterServices _helpMenuMasterServices;
        public helpMenuMappingController(ContextClass context, ILogger<BaseController<helpMenuMapping>> logger, IhelpMenuMasterServices helpMenuMasterServices) : base(context, logger)
        {
            db = context;
            this._helpMenuMasterServices= helpMenuMasterServices;
        }
        protected override IQueryable<helpMenuMapping> DbSet => db.helpMenuMapping.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("SaveHelpMenuMapping")]
        public async Task<ServiceStatusResponseModel> SaveHelpMenuMapping(helpMenuMapping HelpMenu)
        {
            try
            {
                var result = await _helpMenuMasterServices.SaveHelpMenuMapping(HelpMenu);
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
        [HttpPost("RemoveHelpMenuMapping")]
        public async Task<ServiceStatusResponseModel> RemoveHelpMenuMapping(int helpId, int itemId, int observationId,int userId)
        {
            try
            {
                var result = await _helpMenuMasterServices.RemoveHelpMenuMapping(helpId,itemId,observationId,userId);
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

        [HttpPost("GetHelpMenuMapping")]
        public async Task<ServiceStatusResponseModel> GetHelpMenuMapping(int itemId, int observationId)
        {
            try
            {
                var result = await (from hmm in db.helpMenuMapping 
                              join hm in db.helpMenuMaster on hmm.helpId equals hm.id
                              where hmm.itemId== itemId && hmm.ObservationId  ==observationId
                              select new
                              {
                                  hm.id,
                                  hm.helpName,
                                  hmm.itemId,
                                  hmm.ObservationId

                              }).ToListAsync();
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
                    Success = false,
                    Message = ex.InnerException?.Message ?? "An error occurred."
                };
            }
        }

    }
}
