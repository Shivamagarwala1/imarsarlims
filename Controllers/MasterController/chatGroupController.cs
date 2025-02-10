using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class chatGroupController : BaseController<chatGroupMaster>
    {
        private readonly ContextClass db;
        private readonly IchatGroupServices _chatGroupServices;
        private readonly ILogger<BaseController<chatGroupMaster>> _logger;

        public chatGroupController(ContextClass context, ILogger<BaseController<chatGroupMaster>> logger, IchatGroupServices chatGroupServices) : base(context, logger)
        {
            db = context;
            this._chatGroupServices = chatGroupServices;
            this._logger = logger;
        }
        protected override IQueryable<chatGroupMaster> DbSet => db.chatGroupMaster.AsNoTracking().OrderBy(o => o.groupMasterId);


        [HttpPost("CreateChatGroup")]
        public async Task<ActionResult<chatGroupMaster>> CreateChatGroup(chatGroupMaster chatGroup)
        {

            try
            {
                var result = await _chatGroupServices.CreateChatGroupAsync(chatGroup);
                return CreatedAtAction(nameof(CreateChatGroup), new { id = result.groupMasterId }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("CheckGroupMesseges")]
        public async Task<ActionResult<chatGroupMaster>> CheckGroupMesseges(int groupMasterId)
        {

            try
            {
                var result = await _chatGroupServices.CheckGroupMessegesAsync(groupMasterId);
                return CreatedAtAction(nameof(CreateChatGroup), new { id = result.groupMasterId }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
