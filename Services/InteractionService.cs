using Microsoft.EntityFrameworkCore;
using SmartLeadAI.Data;
using SmartLeadAI.Models;

namespace SmartLeadAI.Services;

public class InteractionService
{
    private readonly IDbContextFactory<SmartLeadContext> _contextFactory;

    public InteractionService(IDbContextFactory<SmartLeadContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<Interaction> CreateInteractionAsync(Interaction interaction)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        context.Interactions.Add(interaction);
        await context.SaveChangesAsync();
        return interaction;
    }

    public async Task<List<Interaction>> GetAllInteractionsAsync(int companyId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Interactions
            .AsNoTracking()
            .Include(i => i.Customer)
            .Include(i => i.User)
            .Where(i => i.Customer.CompanyId == companyId)
            .OrderByDescending(i => i.InteractionDate)
            .ToListAsync();
    }

    public async Task<Interaction?> GetInteractionByIdAsync(int id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Interactions
            .AsNoTracking()
            .Include(i => i.Customer)
            .Include(i => i.User)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<List<Interaction>> GetInteractionsByCustomerIdAsync(int customerId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Interactions
            .AsNoTracking()
            .Include(i => i.User)
            .Where(i => i.CustomerId == customerId)
            .OrderByDescending(i => i.InteractionDate)
            .ToListAsync();
    }

    public async Task<bool> UpdateInteractionAsync(Interaction updatedInteraction)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var existing = await context.Interactions.FindAsync(updatedInteraction.Id);
        if (existing == null) return false;

        existing.Type = updatedInteraction.Type;
        existing.Notes = updatedInteraction.Notes;
        existing.InteractionDate = updatedInteraction.InteractionDate;
        existing.NextFollowUp = updatedInteraction.NextFollowUp;
        
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteInteractionAsync(int id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var interaction = await context.Interactions.FindAsync(id);
        if (interaction == null) return false;

        context.Interactions.Remove(interaction);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Interaction>> GetPendingFollowUpsAsync(int companyId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var today = DateTime.UtcNow.Date;
        return await context.Interactions
            .AsNoTracking()
            .Include(i => i.Customer)
            .Include(i => i.User)
            .Where(i => i.Customer.CompanyId == companyId && 
                        i.NextFollowUp.HasValue && 
                        i.NextFollowUp.Value.Date >= today)
            .OrderBy(i => i.NextFollowUp)
            .ToListAsync();
    }
}