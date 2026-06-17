using Microsoft.EntityFrameworkCore;
using SmartLeadAI.Data;
using SmartLeadAI.Models;

namespace SmartLeadAI.Services;

public class CustomerService
{
    private readonly IDbContextFactory<SmartLeadContext> _contextFactory;

    public CustomerService(IDbContextFactory<SmartLeadContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    // READ: Only gets customers for a specific company
    public async Task<List<Customer>> GetAllForCompanyAsync(int companyId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Customers
            .AsNoTracking()
            .Where(c => c.CompanyId == companyId)
            .OrderBy(c => c.FullName)
            .ToListAsync();
    }

    // CREATE: Securely add a customer assigned to a specific company
    public async Task AddAsync(Customer customer)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        context.Customers.Add(customer);
        await context.SaveChangesAsync();
    }

    // READ: Get by ID (ensuring we don't return data from other companies)
    public async Task<Customer?> GetByIdAsync(int id, int companyId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id && c.CompanyId == companyId);
    }

    public async Task UpdateAsync(Customer customer)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        context.Customers.Update(customer);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var customer = await context.Customers.FindAsync(id);

        if (customer != null)
        {
            context.Customers.Remove(customer);
            await context.SaveChangesAsync();
        }
    }
}