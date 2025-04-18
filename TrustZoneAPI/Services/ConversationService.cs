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
    Task<ResponseResult> CreateAsync(string CurrentUser, CreateConversationDTO dto);
    Task<ResponseResult> UpdateAsync(int id, UpdateConversationDTO dto);
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
        var user = await _userService.GetByIdAsync(userId);
        if (user == null)
            return ResponseResult<IEnumerable<ConversationDTO>>.NotFound("User not found.");
        var conversations = await _conversationRepository.GetConversationsByUserIdAsync(userId, page, pageSize);
        if (!conversations.Any())
            return ResponseResult<IEnumerable<ConversationDTO>>.NotFound("No conversations found here yet.");

        var conversationDtos = conversations.Select(_ConvertToDTO);
        return ResponseResult<IEnumerable<ConversationDTO>>.Success(conversationDtos);
    }

    public async Task<ResponseResult<ConversationDTO>> GetConversationBetweenUsersAsync(string user1Id, string user2Id)
    {
        var conversation = await _conversationRepository.GetConversationBetweenUsersAsync(user1Id, user2Id);
        return conversation == null
            ? ResponseResult<ConversationDTO>.NotFound("Conversation between users not found.")
            : ResponseResult<ConversationDTO>.Success(_ConvertToDTO(conversation));
    }

    public async Task<ResponseResult> CreateAsync(string CurrentUserId, CreateConversationDTO dto)
    {
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

    public async Task<ResponseResult> UpdateAsync(int id, UpdateConversationDTO dto)
    {
        var conversation = await _conversationRepository.GetByIdAsync(id);
        if (conversation == null)
            return ResponseResult.NotFound("Conversation not found.");

        conversation.LastMessageAt = dto.LastMessageAt;

        var success = await _conversationRepository.UpdateAsync(conversation);
        return success
            ? ResponseResult.Success()
            : ResponseResult.Error("Failed to update conversation.", 500);
    }

    public async Task<ResponseResult> DeleteAsync(int id)
    {
        var success = await _conversationRepository.DeleteAsync(id);
        return success
            ? ResponseResult.Success()
            : ResponseResult.NotFound("Conversation not found.");
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
            User1 = conversation.User1 != null ? new UserBasicDTO
            {
                Id = conversation.User1.Id,
                UserName = conversation.User1.UserName
            } : null,
            User2 = conversation.User2 != null ? new UserBasicDTO
            {
                Id = conversation.User2.Id,
                UserName = conversation.User2.UserName
            } : null
        };
    }
}