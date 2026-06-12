using Microsoft.EntityFrameworkCore;
using SmartLeadAI.Data;
using SmartLeadAI.Models;

namespace SmartLeadAI.Services;

public class CompanyService
{
    private readonly SmartLeadContext _context;

    public CompanyService(SmartLeadContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Company company)
    {
        _context.Companies.Add(company);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Company>> GetAllAsync()
    {
        return await _context.Companies.ToListAsync();
    }
}