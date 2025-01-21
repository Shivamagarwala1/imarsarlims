using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using iMARSARLIMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class itemCommentMasterController : BaseController<itemCommentMaster>
    {
        private readonly ContextClass db;
        private readonly ICommentInterpatationservices _CommentInterpatationservices;


        public itemCommentMasterController(ContextClass context, ILogger<BaseController<itemCommentMaster>> logger, ICommentInterpatationservices CommentInterpatationservices) : base(context, logger)
        {
            db = context;
            this._CommentInterpatationservices = CommentInterpatationservices;
        }
        protected override IQueryable<itemCommentMaster> DbSet => db.itemCommentMaster.AsNoTracking().OrderBy(o => o.id);
        [HttpPost("SaveCommentMaster")]
        public async Task<ServiceStatusResponseModel> SaveCommentMaster(itemCommentMaster Comment)
        {
            try
            {
                var result = await _CommentInterpatationservices.SaveCommentMaster(Comment);
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
        [HttpPost("updateCommentStatus")]
        public async Task<ServiceStatusResponseModel> updateCommentStatus(int CommentId, byte Status, int UserId)
        {
            try
            {
                var result = await _CommentInterpatationservices.updateCommentStatus(CommentId, Status, UserId);
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
