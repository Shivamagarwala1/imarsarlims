using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class documentTypeMasterController : BaseController<documentTypeMaster>
    {
        private readonly ContextClass db;
        private readonly IdocumentTypeMasterServices  _documentTypeMasterServices;

        public documentTypeMasterController(ContextClass context, ILogger<BaseController<documentTypeMaster>> logger, IdocumentTypeMasterServices documentTypeMasterServices) : base(context, logger)
        {
            db = context;
            this._documentTypeMasterServices = documentTypeMasterServices;
        }
        protected override IQueryable<documentTypeMaster> DbSet => db.documentTypeMaster.AsNoTracking().OrderBy(o => o.id);
        [HttpPost("SaveUpdateDocumentType")]
        public async Task<ServiceStatusResponseModel> SaveUpdateDocumentType(documentTypeMaster DocumnetType)
        {
            try
            {
                var result = await _documentTypeMasterServices.SaveUpdateDocumentType(DocumnetType);
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

        [HttpPost("UpdateDocumnetTypeStatus")]
        public async Task<ServiceStatusResponseModel> UpdateDocumnetTypeStatus(int id, byte status, int userId)
        {
            try
            {
                var result = await _documentTypeMasterServices.UpdateDocumnetTypeStatus(id, status, userId);
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
