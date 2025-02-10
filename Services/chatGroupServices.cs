using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace iMARSARLIMS.Services
{
    public class chatGroupServices : IchatGroupServices
    {
        private readonly ContextClass db;
        private readonly IConfiguration _configuration;
        public chatGroupServices(ContextClass context, ILogger<BaseController<chatGroupMaster>> logger, IConfiguration configuration)
        {

            db = context;
            this._configuration = configuration;
        }

        public async Task<chatGroupMaster> CreateChatGroupAsync(chatGroupMaster chatGroup)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var chatGroupMasterData = CreateChatGroupMasterData(chatGroup);
                    var chatGroupMaster = db.chatGroupMaster.Add(chatGroupMasterData);
                    await db.SaveChangesAsync();
                    var groupMasterId = chatGroupMaster.Entity.groupMasterId;
                    foreach (var chatGroupMasterEmployee in chatGroup.addChatGroupMasterEmployee)
                    {
                        var chatGroupMasterEmployeeData = CreateChatGroupMasterEmployeeData(chatGroupMasterEmployee, groupMasterId);
                        db.chatGroupMasterEmployee.Add(chatGroupMasterEmployeeData);
                        await db.SaveChangesAsync();
                    }
                    foreach (var chatMessage in chatGroup.addChatMessage)
                    {
                        var chatMessageData = CreateChatMessageData(chatMessage, groupMasterId);
                        db.chatMessage.Add(chatMessageData);
                        await db.SaveChangesAsync();
                    }
                    await transaction.CommitAsync();
                    var returnData = await db.chatGroupMaster.Include(g => g.addChatGroupMasterEmployee).Include(g => g.addChatMessage).FirstOrDefaultAsync(g => g.groupMasterId == groupMasterId);
                    return returnData;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return null;
                }
            }
        }
        private chatGroupMaster CreateChatGroupMasterData(chatGroupMaster chatGroupMaster)
        {
            return new chatGroupMaster
            {
                groupMasterName = chatGroupMaster.groupMasterName,
                isActive = chatGroupMaster.isActive,
                createdById = chatGroupMaster.createdById,
                createdDateTime = chatGroupMaster.createdDateTime,
                updateById = chatGroupMaster.updateById,
                updateDateTime = chatGroupMaster.updateDateTime,
            };
        }
        private chatGroupMasterEmployee CreateChatGroupMasterEmployeeData(chatGroupMasterEmployee chatGroupMasterEmployee, int groupMasterId)
        {
            return new chatGroupMasterEmployee
            {
                //groupMasterEmployeeId = chatGroupMasterEmployee.groupMasterEmployeeId,
                groupMasterId = groupMasterId,
                empId = chatGroupMasterEmployee.empId,
                isActive = chatGroupMasterEmployee.isActive,
                createdById = chatGroupMasterEmployee.createdById,
                createdDateTime = chatGroupMasterEmployee.createdDateTime,
                updateById = chatGroupMasterEmployee.updateById,
                updateDateTime = chatGroupMasterEmployee.updateDateTime,
            };
        }
        private chatMessage CreateChatMessageData(chatMessage chatMessage, int groupMasterId)
        {
            return new chatMessage
            {
                //messageId = chatMessage.messageId,
                content = chatMessage.content,
                isSeen = chatMessage.isSeen,
                empId = chatMessage.empId,
                groupMasterId = groupMasterId,
                isActive = chatMessage.isActive,
                createdById = chatMessage.createdById,
                createdDateTime = chatMessage.createdDateTime,
                updateById = chatMessage.updateById,
                updateDateTime = chatMessage.updateDateTime,
            };
        }
        public async Task<chatGroupMaster> CheckGroupMessegesAsync(int groupMasterId)
        {
            var returnData = await db.chatGroupMaster.Include(g => g.addChatGroupMasterEmployee).Include(g => g.addChatMessage).FirstOrDefaultAsync(g => g.groupMasterId == groupMasterId);
            return returnData;
        }

    }
}
