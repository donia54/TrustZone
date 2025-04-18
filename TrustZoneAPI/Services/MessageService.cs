using TrustZoneAPI.DTOs.Chat;
using TrustZoneAPI.DTOs.Users;
using TrustZoneAPI.Models;
using TrustZoneAPI.Repositories;

namespace TrustZoneAPI.Services.Messages;

public interface IMessageService
{
    Task<ResponseResult<MessageDTO>> GetByIdAsync(int id);
    Task<ResponseResult<IEnumerable<MessageDTO>>> GetMessagesByConversationAsync(int conversationId, int page = 1);
    Task<ResponseResult<IEnumerable<MessageDTO>>> GetMessagesBySenderAsync(string senderId);
    Task<ResponseResult> CreateAsync(CreateMessageDTO dto, string CurrentUserId);
    Task<ResponseResult> UpdateAsync(int id, string CurrentUserId, CreateMessageDTO dto);
    Task<ResponseResult> DeleteAsync(int id, string CurrentUserId);
    Task<ResponseResult> MarkAsReadAsync(int messageId);
}

public class MessageService : IMessageService
{
    private readonly ITMessageRepository _messageRepository;

    public MessageService(ITMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    public async Task<ResponseResult<MessageDTO>> GetByIdAsync(int id)
    {
        var message = await _messageRepository.GetByIdAsync(id);
        return message == null
            ? ResponseResult<MessageDTO>.NotFound("Message not found.")
            : ResponseResult<MessageDTO>.Success(_ConvertToDTO(message));
    }

    public async Task<ResponseResult<IEnumerable<MessageDTO>>> GetMessagesByConversationAsync(int conversationId, int page = 1)
    {
        var messages = await _messageRepository.GetMessagesByConversationAsync(conversationId, page);
        if (!messages.Any())
            return ResponseResult<IEnumerable<MessageDTO>>.NotFound("No messages found for this conversation");

        var messageDtos = messages.Select(_ConvertToDTO);
        return ResponseResult<IEnumerable<MessageDTO>>.Success(messageDtos);
    }

    public async Task<ResponseResult<IEnumerable<MessageDTO>>> GetMessagesBySenderAsync(string senderId)
    {
        var messages = await _messageRepository.GetMessagesBySenderAsync(senderId);
        if (!messages.Any())
            return ResponseResult<IEnumerable<MessageDTO>>.NotFound("No messages found for this sender");

        var messageDtos = messages.Select(_ConvertToDTO);
        return ResponseResult<IEnumerable<MessageDTO>>.Success(messageDtos);
    }

    public async Task<ResponseResult> CreateAsync(CreateMessageDTO dto,string CurrentUserId)
    {
        var message = new TMessage
        {
            ConversationId = dto.ConversationId,
            SenderId = CurrentUserId,
            Content = dto.Content,
            IsRead = false
        };

        var success = await _messageRepository.AddAsync(message);
        return success 
            ? ResponseResult.Created() 
            : ResponseResult.Error("Failed to create message.", 500);
    }

    public async Task<ResponseResult> UpdateAsync(int id, string CurrentUserId, CreateMessageDTO dto)
    {
        var message = await _messageRepository.GetByIdAsync(id);
        if (message == null) 
            return ResponseResult.NotFound("Message not found.");

        if (!IsSender(message.SenderId, CurrentUserId))
            return ResponseResult.NotFound("Message not found.");

        message.Content = dto.Content;
        var success = await _messageRepository.UpdateAsync(message);
        return success 
            ? ResponseResult.Success() 
            : ResponseResult.Error("Failed to update message.", 500);
    }

    public async Task<ResponseResult> DeleteAsync(int id,string CurrentUserId)
    {
        var message = await _messageRepository.GetByIdAsync(id);
       
        if(message == null)
            return ResponseResult.NotFound("Message not found.");
        
        if (!IsSender(message.SenderId, CurrentUserId))
            return ResponseResult.NotFound("Message not found.");

        var success = await _messageRepository.DeleteAsync(id);
        return success 
            ? ResponseResult.Success() 
            : ResponseResult.NotFound("Message not found.");
    }

    public async Task<ResponseResult> MarkAsReadAsync(int messageId)
    {
        var success = await _messageRepository.MarkAsReadAsync(messageId);
        return success 
            ? ResponseResult.Success() 
            : ResponseResult.NotFound("Message not found.");
    }


    private bool IsSender(string senderId, string CurrentUserId)
    {
        return senderId == CurrentUserId;
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
            Sender = ((message.Sender != null)  && (message.Sender.UserName != null)) ? new UserBasicDTO

            {
                Id = message.Sender.Id,
                UserName = message.Sender.UserName
            } : null
        };
    }
}
