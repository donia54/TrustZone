using Microsoft.AspNetCore.SignalR;
using TrustZoneAPI.DTOs.Chat;
using TrustZoneAPI.DTOs.Users;
using TrustZoneAPI.Hubs;
using TrustZoneAPI.Models;
using TrustZoneAPI.Repositories;
using TrustZoneAPI.Services.Users;

namespace TrustZoneAPI.Services.Chat;

// Note: I need to organize UserId and CurrentUserId between UserSerivce and BaseController (When I reach there) Incha'Allah.
public interface IMessageService
{
    Task<ResponseResult<MessageDTO>> GetByIdAsync(int id);
    Task<ResponseResult<IEnumerable<MessageDTO>>> GetMessagesByConversationAsync(int conversationId, int page = 1);
    Task<ResponseResult> CreateAsync(CreateMessageDTO dto);
    Task<ResponseResult> UpdateAsync(int id, UpdateMessageDTO dto);
    Task<ResponseResult> DeleteAsync(int id, string CurrentUserId);
    Task<ResponseResult> MarkAsReadAsync(int messageId); // I think this should be internal only.
}

public class MessageService : IMessageService
{
    private readonly ITMessageRepository _repository;
    private readonly IConversationService _conversationService;
    private readonly IUserService _userService;
    private readonly ISignalRMessageSender _signalRMessage;
    public MessageService(ITMessageRepository messageRepository,IConversationService conversationService
        ,IUserService userService, ISignalRMessageSender signalRMessage)
    {
        _repository = messageRepository;
        _conversationService = conversationService;
        _userService = userService;
        _signalRMessage = signalRMessage;
    }

    public async Task<ResponseResult<MessageDTO>> GetByIdAsync(int id)
    {
        var message = await _repository.GetByIdAsync(id);
        return message == null
            ? ResponseResult<MessageDTO>.NotFound("Message not found.")
            : ResponseResult<MessageDTO>.Success(_ConvertToDTO(message));
    }

    public async Task<ResponseResult<IEnumerable<MessageDTO>>> GetMessagesByConversationAsync(int conversationId, int page = 1)
    {
        var IsAuthorized = await _conversationService.IsCurrentUserInConversation(conversationId); // I think this is not organizer well.
        if (!IsAuthorized)
            return ResponseResult<IEnumerable<MessageDTO>>.Error("You are not authorized to view this conversation or it is not exists.", 403);
        
        var messages = await _repository.GetMessagesByConversationAsync(conversationId, page);
        if (!messages.Any())
            return ResponseResult<IEnumerable<MessageDTO>>.NotFound("No messages found for this conversation yet.");

        var messageDtos = messages.Select(_ConvertToDTO);
        return ResponseResult<IEnumerable<MessageDTO>>.Success(messageDtos);
    }

    public async Task<ResponseResult> CreateAsync(CreateMessageDTO dto)
    {
        // Note ye Donia: ConversationId preffered to be Guid or string as Guid.ToString(), not int.

        var currentUserId = _userService.GetCurrentUserId(); // I think this is not the best way.


            int currentConversationId = 0;
            var conversation = await _conversationService.GetConversationBetweenUsersAsync(currentUserId, dto.User2Id);
           
            if(conversation.Data != null)
            currentConversationId = conversation.Data.Id;
            if (currentConversationId == 0)
            {
                var result = await _conversationService.CreateAsync(dto.User2Id);
                if (!result.IsSuccess)
                    return ResponseResult.Error($"{result.ErrorMessage}", result.StatusCode);
                currentConversationId = result.Data;
            }
        //var IsConversationExists = await _conversationService.IsConversationExists(currentConversationId);
        //if (!IsConversationExists)
        //    return ResponseResult.Error("Conversation not found.", 404);

        var IsAllowed = await _conversationService.IsCurrentUserInConversation(currentConversationId);
        if(!IsAllowed)
            return ResponseResult.Error("You are not authorized to view this conversation or it is not exists.", 403);

        var message = new TMessage
        {
            ConversationId = currentConversationId,
            SenderId = currentUserId,
            Content = dto.Content,
            IsRead = false
        };

        var success = await _repository.AddAsync(message);

        if(success)
        {
            var receiverId = dto.User2Id;
            var senderId = currentUserId;
            await _signalRMessage.SendToClient(receiverId, senderId, dto.Content);

            return ResponseResult.Created();
        }
        else
        {
            return ResponseResult.Error("Failed to create message.", 500);
        }


    }

    public async Task<ResponseResult> UpdateAsync(int id, UpdateMessageDTO dto)
    {
        var message = await _repository.GetByIdAsync(id);
        if (message == null) 
            return ResponseResult.NotFound("Message not found.");

        if (_userService.IsCurrentUser(message.SenderId))
            return ResponseResult.NotFound("You are not allowed to update this message.");

        message.Content = dto.Content;
        var success = await _repository.UpdateAsync(message);
        return success 
            ? ResponseResult.Success() 
            : ResponseResult.Error("Failed to update message.", 500);
    }

    public async Task<ResponseResult> DeleteAsync(int id,string CurrentUserId)
    {
        var message = await _repository.GetByIdAsync(id);
        if (message == null)
            return ResponseResult.NotFound("Message not found.");

        if (_userService.IsCurrentUser(message.SenderId))
            return ResponseResult.NotFound("You are not allowed to delete this message.");
        
        var success = await _repository.DeleteAsync(id);
        return success 
            ? ResponseResult.Success() 
            : ResponseResult.NotFound("Message not found.");
    }
    private static MessageDTO _ConvertToDTO(TMessage message)
    {
        return new MessageDTO
        {
            Id = message.Id,
            ConversationId = message.ConversationId,
            SenderId = message.SenderId,
            Content = message.Content,
            SentAt = message.SentAt,
            IsRead = message.IsRead,
            ReadAt = message.ReadAt,
            Sender = message.Sender != null  && message.Sender.UserName != null ? new UserBasicDTO

            {
                Id = message.Sender.Id,
                UserName = message.Sender.UserName
            } : null
        };
    }
    public async Task<ResponseResult> MarkAsReadAsync(int messageId)
    {
        // هاذ بدها شوية شغل... خليها لحد ما أجهّز السيجنال آر بتوضح الأمور وقتها إن شاء الله
        var message = await _repository.GetByIdAsync(messageId);
        if (message == null)
            return ResponseResult.NotFound("Message not found.");

        var isAuthorized = await _conversationService.IsCurrentUserInConversation(message.ConversationId);
        if (!isAuthorized)
            return ResponseResult.Error("You are not authorized to mark messages as read in this conversation.", 403);

        var currentUserId = _userService.GetCurrentUserId();

        // تحديث الرسائل باستخدام استعلام SQL مباشر
        var success = await _repository.MarkMessagesAsReadAsync(message.ConversationId, message.SentAt, currentUserId);
        return success
            ? ResponseResult.Success()
            : ResponseResult.Error("Failed to mark messages as read.", 500);
    }

}
