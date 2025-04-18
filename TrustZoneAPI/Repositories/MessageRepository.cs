using TrustZoneAPI.Data;
using TrustZoneAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace TrustZoneAPI.Repositories;
public interface ITMessageRepository
{
    Task<IEnumerable<TMessage>> GetAllAsync();
    Task<TMessage?> GetByIdAsync(int id);
    Task<bool> AddAsync(TMessage entity);
    Task<bool> UpdateAsync(TMessage entity);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<TMessage>> GetMessagesByConversationAsync(int conversationId);
    Task<IEnumerable<TMessage>> GetMessagesByConversationAsync(int conversationId, int page = 1, int pageSize = 20);
    Task<bool> MarkAsReadAsync(int messageId);
    Task<bool> MarkMessagesAsReadAsync(int conversationId, DateTime sentAt, string currentUserId);
}
public class MessageRepository : ITMessageRepository
{
    private readonly AppDbContext _context;

    public MessageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TMessage>> GetAllAsync()
    {
        return await _context.TMessages.ToListAsync();
    }

    public async Task<TMessage?> GetByIdAsync(int id)
    {
        return await _context.TMessages.FindAsync(id);
    }

    public async Task<bool> AddAsync(TMessage entity)
    {
        entity.SentAt = DateTime.UtcNow;
        await _context.TMessages.AddAsync(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync(TMessage entity)
    {
        _context.TMessages.Update(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var message = await _context.TMessages.FindAsync(id);
        if (message == null)
            return false;

        _context.TMessages.Remove(message);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<TMessage>> GetMessagesByConversationAsync(int conversationId)
    {
        return await _context.TMessages
            .Where(m => m.ConversationId == conversationId)
            .Include(m => m.Sender)
            .OrderBy(m => m.SentAt)
            .ToListAsync();
    }
    public async Task<IEnumerable<TMessage>> GetMessagesByConversationAsync(int conversationId, int page = 1, int pageSize = 20)
    {
        return await _context.TMessages
            .Where(m => m.ConversationId == conversationId)
            .Include(m => m.Sender)
            .OrderByDescending(m => m.SentAt) // Most recent messages first
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
    public async Task<bool> MarkAsReadAsync(int messageId)
    {
        var message = await _context.TMessages.FindAsync(messageId);
        if (message == null)
            return false;

        message.IsRead = true;
        message.ReadAt = DateTime.UtcNow;
        return await _context.SaveChangesAsync() > 0;
    }
    public async Task<bool> MarkMessagesAsReadAsync(int conversationId, DateTime sentAt, string currentUserId)
    {
        var query = _context.TMessages
            .Where(m => m.ConversationId == conversationId && m.SentAt <= sentAt && m.SenderId != currentUserId);

        await query.ForEachAsync(m =>
        {
            m.IsRead = true;
            m.ReadAt = DateTime.UtcNow;
        });

        return await _context.SaveChangesAsync() > 0;
    }

}