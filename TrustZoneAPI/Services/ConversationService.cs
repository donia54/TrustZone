using TrustZoneAPI.DTOs.Chat;
using TrustZoneAPI.DTOs.Users;
using TrustZoneAPI.Models;
using TrustZoneAPI.Repositories;
using TrustZoneAPI.Services.Users;

namespace TrustZoneAPI.Services.Conversations;
public interface IConversationService
{
    Task<ResponseResult<ConversationDTO>> GetByIdAsync(int id);
    Task<ResponseResult<IEnumerable<ConversationDTO>>> GetConversationsByUserIdAsync(string userId, int page = 1, int pageSize = 20);
    Task<ResponseResult<ConversationDTO>> GetConversationBetweenUsersAsync(string user1Id, string user2Id);
    Task<ResponseResult> CreateAsync(CreateConversationDTO dto);
    Task<bool> UpdateLastMessageAtAsync(int id, DateTime LastMessageAt);
    Task<ResponseResult> DeleteAsync(int id);
}

public class ConversationService : IConversationService
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IUserService _userService;

    public ConversationService(IConversationRepository conversationRepository,IUserService userService)
    {
        _conversationRepository = conversationRepository;
        _userService = userService;
    }


    public async Task<ResponseResult<ConversationDTO>> GetByIdAsync(int id)
    {
        var conversation = await _conversationRepository.GetByIdAsync(id);
        return conversation == null
            ? ResponseResult<ConversationDTO>.NotFound("Conversation not found.")
            : ResponseResult<ConversationDTO>.Success(_ConvertToDTO(conversation));
    }

    public async Task<ResponseResult<IEnumerable<ConversationDTO>>> GetConversationsByUserIdAsync(string userId, int page = 1, int pageSize = 20)
    {
        if(!_userService.IsCurrentUser(userId))
            return ResponseResult<IEnumerable<ConversationDTO>>.NotFound("No conversations found here yet.");
       
        var conversations = await _conversationRepository.GetConversationsByUserIdAsync(userId, page, pageSize);
        if (!conversations.Any())
            return ResponseResult<IEnumerable<ConversationDTO>>.NotFound("No conversations found here yet.");

        var conversationDtos = conversations.Select(_ConvertToDTO);
        return ResponseResult<IEnumerable<ConversationDTO>>.Success(conversationDtos);
    }

    public async Task<ResponseResult<ConversationDTO>> GetConversationBetweenUsersAsync(string user1Id, string user2Id)
    {
        if(!(_userService.IsCurrentUser(user1Id) || _userService.IsCurrentUser(user2Id))) 
            return ResponseResult<ConversationDTO>.Error("You are not authorized to view this conversation if exists.", 403);

        var conversation = await _conversationRepository.GetConversationBetweenUsersAsync(user1Id, user2Id);
        return conversation != null
            ? ResponseResult<ConversationDTO>.Success(_ConvertToDTO(conversation))
            : ResponseResult<ConversationDTO>.NotFound("No conversation between users yet.");
    }

    public async Task<ResponseResult> CreateAsync(CreateConversationDTO dto)
    {
        var CurrentUserId = _userService.GetCurrentUser();
        bool isUser1Exists = await _userService.IsUserExists(CurrentUserId);
        bool isUser2Exists = await _userService.IsUserExists(dto.User2Id);

        if (!(isUser1Exists && isUser1Exists))
            return ResponseResult.Error("User not found.", 404);

        var conversation = new Conversation
        {
            User1Id = CurrentUserId,
            User2Id = dto.User2Id,
            CreatedAt = DateTime.UtcNow
        };

        var success = await _conversationRepository.AddAsync(conversation);
        return success
            ? ResponseResult.Created()
            : ResponseResult.Error("Failed to create conversation.", 500);
    }

    public async Task<bool> UpdateLastMessageAtAsync(int id,DateTime LastMessageAt)
    {
        var conversation = await _conversationRepository.GetByIdAsync(id);
        if (conversation == null)
            return false;

        if(!IsCurrentUserInConversation(conversation))
            return false;

        conversation.LastMessageAt = LastMessageAt;

        return await _conversationRepository.UpdateAsync(conversation);
    }

    public async Task<ResponseResult> DeleteAsync(int id)
    { 
        // This will delete the conversation for both users. I think this is not the best way to do it.

        var conversation = await _conversationRepository.GetByIdAsync(id);
        if (conversation == null)
            return ResponseResult.NotFound("Conversation not found.");
        
        if(!IsCurrentUserInConversation(conversation))
            return ResponseResult.Error("You are not authorized to delete this conversation.", 403); // This is not a good message.

        var success = await _conversationRepository.DeleteAsync(conversation);
        return success
            ? ResponseResult.Success()
            : ResponseResult.Error("Failed to delete conversation.", 500);
    }

    private bool IsCurrentUserInConversation(Conversation conversation)
    {
        // Check if the current user is either User1 or User2 in the conversation.
        return _userService.IsCurrentUser(conversation.User1Id) || _userService.IsCurrentUser(conversation.User2Id);
    }
    private static ConversationDTO _ConvertToDTO(Conversation conversation)
    {
        return new ConversationDTO
        {
            Id = conversation.Id,
            User1Id = conversation.User1Id,
            User2Id = conversation.User2Id,
            CreatedAt = conversation.CreatedAt,
            LastMessageAt = conversation.LastMessageAt,
            User1 = conversation.User1 != null && conversation.User1.UserName != null ? new UserBasicDTO
            {
                Id = conversation.User1.Id,
                UserName = conversation.User1.UserName
            } : null,
            User2 = conversation.User2 != null && conversation.User2.UserName != null ? new UserBasicDTO
            {
                Id = conversation.User2.Id,
                UserName = conversation.User2.UserName
            } : null
        };
    }
}