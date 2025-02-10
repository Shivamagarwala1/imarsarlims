using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class chatMessageController : BaseController<chatMessage>
    {
        private readonly ContextClass db;
        private readonly IchatMessageServices _chatMessageServices;
        private readonly ILogger<BaseController<chatMessage>> _logger;
        private readonly IHubContext<ChatHub> _hubContext; // 

        public chatMessageController(ContextClass context, ILogger<BaseController<chatMessage>> logger,
            IchatMessageServices chatMessageServices, IHubContext<ChatHub> hubContext) : base(context, logger)
        {
            db = context;
            this._chatMessageServices = chatMessageServices;
            this._logger = logger;
            _hubContext = hubContext; // Initialize SignalR Hub
        }
        protected override IQueryable<chatMessage> DbSet => db.chatMessage.AsNoTracking().OrderByDescending(o => o.messageId);

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage(IFormFile? file,int createdById,int? groupMasterId,int? empId, string content)
        {
            var chatMessage = new chatMessage
            {
                createdById = createdById,
                groupMasterId = groupMasterId,
                empId = empId,
                content = content,
                createdDateTime = DateTime.Now,
                updateDateTime = DateTime.Now,
            };
            if (chatMessage == null || string.IsNullOrWhiteSpace(chatMessage.content))
            {
                return BadRequest("Message content is required.");
            }

            try
            {
                // Save the message with optional file
                var result = await _chatMessageServices.SendMessageAsync(file, chatMessage);

                if (!result.Value)
                {
                    return StatusCode(500, "Failed to send message.");
                }

                // Notify all clients about the new message
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", chatMessage.createdById, chatMessage.content);

                // Ensure groupMasterId and empId are not null before sending notifications
                if (chatMessage.groupMasterId.HasValue)
                {
                    await _hubContext.Clients.Group(chatMessage.groupMasterId.Value.ToString())
                        .SendAsync("ReceiveMessages", chatMessage.content);
                }
                else if (chatMessage.empId.HasValue)
                {
                    await _hubContext.Clients.User(chatMessage.empId.Value.ToString())
                        .SendAsync("ReceiveMessages", chatMessage.content);
                }

                return Ok(new { status = result.Value });
            }
            catch (Exception ex)
            {
                // Log the exception (if logging is set up)
                _logger.LogError(ex, "Error sending message");

                return StatusCode(500, "An error occurred while sending the message.");
            }
        }


        [HttpPost("GetMessages")]
        public async Task<ActionResult<IEnumerable<chatMessage>>> GetMessages(int? groupMasterId, int? createdById, int? empId)
        {
            try
            {
                var result = await _chatMessageServices.GetMessagesAsync(groupMasterId, createdById, empId);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("GetMessagesForEmployee")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetMessagesForEmployee(int empId, int page = 1, int pageSize = 50)
        {
            try
            {
                var result = await _chatMessageServices.GetMessagesForEmployeeAsync(empId, page, pageSize);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("{messageId}/seen")]
        public async Task<ActionResult<bool>> MarkMessageAsSeen(int messageId)
        {
            try
            {
                var result = await _chatMessageServices.MarkMessageAsSeenAsync(messageId);
                return Ok(new { status = result.Value });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("DownloadFile")]
        public async Task<IActionResult> DownloadFile(int messageId)
        {
            try
            {
                var fileResult = await _chatMessageServices.DownloadFileAsync(messageId);
                if (fileResult == null)
                {
                    return NotFound("File not found.");
                }

                return File(fileResult.Value.FileData, fileResult.Value.ContentType, fileResult.Value.FileName);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("test")]
        public ActionResult<chatMessage> Get()
        {
            return new chatMessage
            {
                createdDateTime = DateTime.Now
            };
        }

    }
}
