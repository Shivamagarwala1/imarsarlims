using iMARSARLIMS.Interface.Store;
using iMARSARLIMS.Model.Store;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.StoreController
{
    [Route("api/[controller]")]
    [ApiController]
    public class indentIssueDetailController : BaseController<indentIssueDetail>
    {
        private readonly ContextClass db;
        private readonly IindentServices _indentServices;

        public indentIssueDetailController(ContextClass context, ILogger<BaseController<indentIssueDetail>> logger, IindentServices indentServices) : base(context, logger)
        {
            db = context;
            this._indentServices = indentServices;
        }
        protected override IQueryable<indentIssueDetail> DbSet => db.indentIssueDetail.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("IssueIndent")]
        public async Task<ServiceStatusResponseModel> IssueIndent(List<indentIssueDetail> issueDetails)
        {
            try
            {
                var result = await _indentServices.IssueIndent(issueDetails);
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
