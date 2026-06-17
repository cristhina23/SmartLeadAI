using Microsoft.EntityFrameworkCore;
using SmartLeadAI.Data;
using SmartLeadAI.Models;

namespace SmartLeadAI.Services;

public class InteractionService
{
    private readonly SmartLeadContext _context;

    public InteractionService(SmartLeadContext context)
    {
        _context = context;
    }

    // CREATE: Adds a new interaction to the database
    public async Task<Interaction> CreateInteractionAsync(Interaction interaction)
    {
        _context.Interactions.Add(interaction);
        await _context.SaveChangesAsync();
        return interaction;
    }

    // READ: Gets all interactions
    public async Task<List<Interaction>> GetAllInteractionsAsync()
    {
        return await _context.Interactions
            .AsNoTracking()
            .Include(i => i.Customer) // Include related Customer data
            .Include(i => i.User)     // Include related User data
            .OrderByDescending(i => i.InteractionDate)
            .ToListAsync();
    }

    // READ: Gets a specific interaction by its ID
    public async Task<Interaction?> GetInteractionByIdAsync(int id)
    {
        return await _context.Interactions
            .AsNoTracking()
            .Include(i => i.Customer)
            .Include(i => i.User)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    // READ: Gets all interactions for a specific customer
    public async Task<List<Interaction>> GetInteractionsByCustomerIdAsync(int customerId)
    {
        return await _context.Interactions
            .AsNoTracking()
            .Include(i => i.User)
            .Where(i => i.CustomerId == customerId)
            .OrderByDescending(i => i.InteractionDate)
            .ToListAsync();
    }

    // UPDATE: Updates an existing interaction
    public async Task<bool> UpdateInteractionAsync(Interaction updatedInteraction)
    {
        var existingInteraction = await _context.Interactions.FindAsync(updatedInteraction.Id);
        
        if (existingInteraction == null)
            return false;

        // Update properties
        existingInteraction.Type = updatedInteraction.Type;
        existingInteraction.Notes = updatedInteraction.Notes;
        existingInteraction.InteractionDate = updatedInteraction.InteractionDate;
        existingInteraction.NextFollowUp = updatedInteraction.NextFollowUp;
        
        // Save changes
        await _context.SaveChangesAsync();
        return true;
    }

    // DELETE: Removes an interaction from the database
    public async Task<bool> DeleteInteractionAsync(int id)
    {
        var interaction = await _context.Interactions.FindAsync(id);
        
        if (interaction == null)
            return false;

        _context.Interactions.Remove(interaction);
        await _context.SaveChangesAsync();
        return true;
    }

    // READ: Gets all pending interactions (follow-up date is in the future)
    public async Task<List<Interaction>> GetPendingFollowUpsAsync()
    {
        return await _context.Interactions
            .AsNoTracking()
            .Include(i => i.Customer)
            .Include(i => i.User)
            .Where(i => i.NextFollowUp.HasValue && i.NextFollowUp > DateTime.UtcNow)
            .OrderBy(i => i.NextFollowUp)
            .ToListAsync();
    }
}
