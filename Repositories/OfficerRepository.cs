using Microsoft.EntityFrameworkCore;
using RegistryRecord.Data;
using RegistryRecord.Entities;

namespace RegistryRecord.Repositories;

public class OfficerRepository
{
    private readonly AppDbContext _context;
    
    public OfficerRepository(AppDbContext context)
        {
        _context = context;
        }

    public async Task<Officer?> GetOfficer(string registrationNumber)
    {
        var data = await _context.Officers.Include(e => e.Files)
            .FirstOrDefaultAsync(o => o.RegistrationNumber == registrationNumber);
        return data;
    }
    
}