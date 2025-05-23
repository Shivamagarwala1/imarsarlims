﻿using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class sampletype_masterController : BaseController<sampletype_master>
    {
        private readonly ContextClass db;
        private readonly IlabUniversalMasterServices _labUniversalMasterServices;

        public sampletype_masterController(ContextClass context, ILogger<BaseController<sampletype_master>> logger, IlabUniversalMasterServices labUniversalMasterServices) : base(context, logger)
        {
            db = context;
            this._labUniversalMasterServices = labUniversalMasterServices;
        }
        protected override IQueryable<sampletype_master> DbSet => db.sampletype_master.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("SaveUpdateSampletype")]
        public async Task<ServiceStatusResponseModel> SaveUpdateSampletype(sampletype_master SampleType)
        {
            try
            {
                var result = await _labUniversalMasterServices.SaveUpdateSampletype(SampleType);
                return result;
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

        [HttpPost("UpdateSampleTypeStatus")]
        public async Task<ServiceStatusResponseModel> UpdateSampleTypeStatus(int id, byte status, int userId)
        {
            try
            {
                var result = await _labUniversalMasterServices.UpdateSampleTypeStatus(id, status, userId);
                return result;
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
