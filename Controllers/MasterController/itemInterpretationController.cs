using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class itemInterpretationController : BaseController<itemInterpretation>
    {
        private readonly ContextClass db;
        private readonly ICommentInterpatationservices _CommentInterpatationservices;

        public itemInterpretationController(ContextClass context, ILogger<BaseController<itemInterpretation>> logger, ICommentInterpatationservices CommentInterpatationservices) : base(context, logger)
        {
            db = context;
            this._CommentInterpatationservices= CommentInterpatationservices;
        }
        protected override IQueryable<itemInterpretation> DbSet => db.itemInterpretation.AsNoTracking().OrderBy(o => o.id);


        [HttpPost("SaveInterpatation")]
        public async Task<ServiceStatusResponseModel> SaveInterpatation(itemInterpretation Interpretation)
        {
            try
            {
                var result = await _CommentInterpatationservices.SaveInterpatation(Interpretation);
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
        [HttpPost("updateInterpatationStatus")]
        public async Task<ServiceStatusResponseModel> updateInterpatationStatus(int InterpatationId, byte Status, int UserId)
        {
            try
            {
                var result = await _CommentInterpatationservices.updateInterpatationStatus(InterpatationId, Status, UserId);
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
