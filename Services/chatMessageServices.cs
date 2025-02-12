using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using static Google.Cloud.Dialogflow.V2.StreamingRecognitionResult.Types;
using System.Text.RegularExpressions;
using iText.Commons.Utils;
using Microsoft.AspNetCore.StaticFiles;

namespace iMARSARLIMS.Services
{
    public class chatMessageServices : IchatMessageServices
    {
        private readonly ContextClass db;
        private readonly IConfiguration _configuration;
        public chatMessageServices(ContextClass context, ILogger<BaseController<chatMessageServices>> logger, IConfiguration configuration)
        {

            db = context;
            this._configuration = configuration;
        }
        public async Task<ActionResult<bool>> SendMessageAsync(IFormFile? file,chatMessage chatMessage)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    if (chatMessage.content != null)
                    {
                        string? fileUrl = string.Empty;
                        string? fileName = string.Empty;
                        int _maxFileSizeMB = _configuration.GetValue<int>("ChatFileSettings:MaxFileSizeMB"); 
                        // Check if a file is uploaded
                        if (file != null)
                        {
                            string primaryFolder = _configuration["DocumentPath:PrimaryFolder"];
                            if (!Directory.Exists(primaryFolder))
                            {
                                Directory.CreateDirectory(primaryFolder);
                            }
                            string uploadPath = Path.Combine(primaryFolder, _configuration.GetValue<string>("ChatFileSettings:UploadPath"));
                            if (!Directory.Exists(uploadPath))
                            {
                                Directory.CreateDirectory(uploadPath);
                            }
                            var uploadsFolder = Path.Combine(uploadPath, DateTime.UtcNow.ToString("yyyyMMdd"));
                            if (!Directory.Exists(uploadsFolder))
                                Directory.CreateDirectory(uploadsFolder);

                            fileName = $"{Guid.NewGuid()}_{file.FileName}";
                            var filePath = Path.Combine(uploadsFolder, fileName);

                            // Validate file size
                            if (file.Length > _maxFileSizeMB * 1024 * 1024)
                            {
                                throw new Exception($"File size exceeds the limit of {_maxFileSizeMB} MB.");
                            }

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            fileUrl = $"/{_configuration.GetValue<string>("ChatFileSettings:UploadPath")}/{DateTime.UtcNow.ToString("yyyyMMdd")}/{fileName}"; // URL for the frontend
                        }
                        var chatMessageData = CreateChatMessageData(chatMessage, fileName ?? string.Empty, fileUrl ?? string.Empty);
                        db.chatMessage.Add(chatMessageData);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }
        private chatMessage CreateChatMessageData(chatMessage chatMessage,string fileName, string fileUrl)
        {
            return new chatMessage
            {
                //messageId = chatMessage.messageId,
                content = chatMessage.content,
                isSeen = chatMessage.isSeen,
                empId = chatMessage.empId,
                groupMasterId = chatMessage.groupMasterId,
                isActive = chatMessage.isActive,
                createdById = chatMessage.createdById,
                createdDateTime = chatMessage.createdDateTime,
                updateById = chatMessage.updateById,
                updateDateTime = chatMessage.updateDateTime,
                fileName = fileName,
                fileUrl = fileUrl
            };
        }

        public async Task<ActionResult<IEnumerable<chatMessage>>> GetMessagesAsync(int? groupMasterId, int? createdById, int? empId)
        {
            try
            {
                IQueryable<chatMessage> query = db.chatMessage;

                if (groupMasterId.HasValue)
                {
                    // Fetch group messages
                    query = query.Where(m => m.groupMasterId == groupMasterId);
                }
                else if (createdById.HasValue && empId.HasValue)
                {
                    // Fetch direct messages between two employees
                    query = query.Where(m =>
                        ((m.createdById == createdById && m.empId == empId) ||
                         (m.createdById == empId && m.empId == createdById)));
                }

                var messages = await query.OrderByDescending(m => m.createdDateTime).ToListAsync();
                return messages;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<ActionResult<IEnumerable<dynamic>>> GetMessagesForEmployeeAsync(int? empId, int page, int pageSize)
        {
            try
            {
                // Fetch group IDs where the employee is a member
                var groupIds = await db.chatGroupMasterEmployee
                    .Where(g => g.empId == empId)
                    .Select(g => g.groupMasterId)
                    .ToListAsync();

                // Fetch messages:
                // 1. Group messages where the employee is part of the group
                // 2. Direct messages where the employee is either the sender or receiver
                var messages = await db.chatMessage
                    .Where(m =>
                        ( groupIds.Contains(m.groupMasterId.Value)) ||
                        ( m.createdById == empId || m.empId == empId))
                    .OrderByDescending(m => m.messageId) // Order by sent time
                    .Include(m => m.chatGroupMaster)
                    .Select(m => new
                    {
                        // Select properties from chatMessage
                        m.messageId,
                        m.content,
                        m.isSeen,
                        m.fileName,
                        m.createdDateTime,
                        //createdDateTime  = m.createdDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        m.empId,
                        employee = m.empMaster != null ? new
                        {
                            m.empMaster.userName,
                        } : null,
                        m.groupMasterId,
                        Group = m.chatGroupMaster != null ? new
                        {
                            m.chatGroupMaster.groupMasterName,
                        } : null
                    })
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                return messages;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<ActionResult<bool>> MarkMessageAsSeenAsync(int messageId)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    if (messageId != 0)
                    {
                        // Find the message by ID
                        var message = await db.chatMessage.FindAsync(messageId);
                        if (message == null) 
                            return false;

                        // Get the sender and receiver IDs from the message
                        int senderId = message.createdById ?? 0;
                        int receiverId = message.empId ?? 0;

                        // Mark all previous messages between these two users as seen
                        var previousMessages = await db.chatMessage
                            .Where(m =>
                                (m.createdById == senderId && m.empId == receiverId))
                            .Where(m => m.isSeen == false) // Only mark unseen messages
                            .ToListAsync();

                        foreach (var msg in previousMessages)
                        {
                            msg.isSeen = true;
                        }

                        // Save changes to the database
                        await db.SaveChangesAsync();

                        await transaction.CommitAsync();
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }
        
        public async Task<(byte[] FileData, string ContentType, string FileName)?> DownloadFileAsync(int messageId)
        {
            try
            {
                // Find the message by ID
                var message = await db.chatMessage.FindAsync(messageId);
                if (message == null)
                {
                    return null; // Return null if the message is not found
                }

                // Get primary folder path from configuration
                string primaryFolder = _configuration["DocumentPath:PrimaryFolder"];
                if (string.IsNullOrEmpty(primaryFolder))
                {
                    throw new Exception("Primary folder path is not configured.");
                }

                // Ensure the directory exists
                if (!Directory.Exists(primaryFolder))
                {
                    Directory.CreateDirectory(primaryFolder);
                }

                // Remove duplicate slashes from the file URL
                message.fileUrl = message.fileUrl.Replace("//", "/");

                // Combine the primary folder and file URL
                string fullFilePath = Path.Combine(primaryFolder, message.fileUrl.TrimStart('/'));
                if (!System.IO.File.Exists(fullFilePath))
                {
                    return null; // Return null if file doesn't exist
                }

                // Get file content type
                string contentType = "application/octet-stream"; // Default content type
                new FileExtensionContentTypeProvider().TryGetContentType(message.fileName, out contentType);

                // Read file as bytes
                var fileBytes = await System.IO.File.ReadAllBytesAsync(fullFilePath);

                return (fileBytes, contentType, message.fileName);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error downloading file: {ex.Message}");
            }
        }

    }
}
