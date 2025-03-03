using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestMethodMasterController : BaseController<TestMethodMaster>
    {
        private readonly ContextClass db;
        private readonly IlabUniversalMasterServices _labUniversalMasterServices;
        public TestMethodMasterController(ContextClass context, ILogger<BaseController<TestMethodMaster>> logger, IlabUniversalMasterServices labUniversalMasterServices) : base(context, logger)
        {
            db = context;
            this._labUniversalMasterServices = labUniversalMasterServices;
        }
        protected override IQueryable<TestMethodMaster> DbSet => db.TestMethodMaster.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("SaveUpdateTestMethod")]
        public async Task<ServiceStatusResponseModel> SaveUpdateTestMethod(TestMethodMaster testMethod)
        {
            try
            {
                var result = await _labUniversalMasterServices.SaveUpdateTestMethod(testMethod);
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

        [HttpPost("UpdateTestMethodStatus")]
        public async Task<ServiceStatusResponseModel> UpdateTestMethodStatus(int id, byte status, int userId)
        {
            try
            {
                var result = await _labUniversalMasterServices.UpdateTestMethodStatus(id, status, userId);
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
