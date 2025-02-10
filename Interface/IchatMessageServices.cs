using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;

namespace iMARSARLIMS.Interface
{
    public interface IchatMessageServices
    {
        Task<ActionResult<bool>> SendMessageAsync(IFormFile? file ,chatMessage chatMessage);
        Task<ActionResult<IEnumerable<chatMessage>>> GetMessagesAsync(int? groupMasterId, int? createdById, int? empId);
        Task<ActionResult<IEnumerable<dynamic>>> GetMessagesForEmployeeAsync(int? empId, int page, int pageSize);
        Task<ActionResult<bool>> MarkMessageAsSeenAsync(int messageId);
        Task<(byte[] FileData, string ContentType, string FileName)?> DownloadFileAsync(int messageId);
    }
}
