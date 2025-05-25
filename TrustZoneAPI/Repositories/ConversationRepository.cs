using TrustZoneAPI.Data;
using TrustZoneAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace TrustZoneAPI.Repositories;
public interface IConversationRepository
{
    Task<Conversation?> GetByIdAsync(int id);
    Task<bool> AddAsync(Conversation entity);
    Task<bool> UpdateAsync(Conversation entity);
    Task<bool> DeleteAsync(Conversation conversation);
    Task<IEnumerable<Conversation>> GetConversationsByUserIdAsync(string userId);
    Task<IEnumerable<Conversation>> GetConversationsByUserIdAsync(string userId, int page = 1, int pageSize = 20);
    Task<Conversation?> GetConversationBetweenUsersAsync(string user1Id, string user2Id);
    Task<bool> IsConversationExists(int id);
}
public class ConversationRepository : IConversationRepository
{
    private readonly AppDbContext _context;

    public ConversationRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Conversation?> GetByIdAsync(int id)
    {
        return await _context.Conversations
            .Include(c => c.TMessages)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> AddAsync(Conversation entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.Conversations.AddAsync(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync(Conversation entity)
    {
        _context.Conversations.Update(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(Conversation conversation)
    {

        var messages = _context.TMessages.Where(m => m.ConversationId == conversation.Id);
        _context.TMessages.RemoveRange(messages);
        _context.Conversations.Remove(conversation);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<Conversation>> GetConversationsByUserIdAsync(string userId)
    {
        return await _context.Conversations
            .Where(c => c.User1Id == userId || c.User2Id == userId)
            .Include(c => c.User1)
            .Include(c => c.User2)
              .Include(c => c.TMessages)
            .OrderByDescending(c => c.LastMessageAt)
            .ToListAsync();

      


    }
    public async Task<IEnumerable<Conversation>> GetConversationsByUserIdAsync(string userId, int page = 1, int pageSize = 20)
    {
        if (page < 1 || pageSize < 1)
            throw new ArgumentException("Page and pageSize must be greater than 0.");

        return await _context.Conversations
            .Where(c => c.User1Id == userId || c.User2Id == userId)
            .Include(c => c.User1)
            .Include(c => c.User2)
            .OrderByDescending(c => c.LastMessageAt)
            .Include(c => c.TMessages)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
    public async Task<Conversation?> GetConversationBetweenUsersAsync(string user1Id, string user2Id)
    {
        return await _context.Conversations
            .Where(c => c.User1Id == user1Id && c.User2Id == user2Id ||
                        c.User1Id == user2Id && c.User2Id == user1Id)
            .Include(c => c.User1)
            .Include(c => c.User2)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> IsConversationExists(int id)
    {
        return await _context.Conversations.AnyAsync(c => c.Id == id);
    }
}