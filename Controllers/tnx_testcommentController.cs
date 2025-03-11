using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Response_Model;
using iMARSARLIMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class tnx_testcommentController : BaseController<tnx_testcomment>
    {
        private readonly ContextClass db;
        private readonly Itnx_testcommentServices _tnx_testcommentService;

        public tnx_testcommentController(ContextClass context, ILogger<BaseController<tnx_testcomment>> logger, Itnx_testcommentServices tnx_testcommentServices) : base(context, logger)
        {
            db = context;
            this._tnx_testcommentService = tnx_testcommentServices;
        }
        protected override IQueryable<tnx_testcomment> DbSet => db.tnx_testcomment.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("SaveTestComment")]
        public async Task<ServiceStatusResponseModel> SaveTestComment(tnx_testcomment CommentData)
        {
            try
            {
                var result = await _tnx_testcommentService.SaveTestComment(CommentData);
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
