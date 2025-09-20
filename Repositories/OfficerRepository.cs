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

    public async Task<bool> IsOfficerExists(string registrationNumber)
    {
        return await _context.Officers.AnyAsync(o => o.RegistrationNumber == registrationNumber);
    }

    // Tek Officer çekme
    public async Task<Officer?> GetOfficer(string registrationNumber)
    {
        return await _context.Officers
            .Include(o => o.Files.Where(f => !f.IsDeleted))
            .FirstOrDefaultAsync(o => o.RegistrationNumber == registrationNumber && !o.IsDeleted);
    }

    // Yeni Officer ekleme
    public async Task<Officer> AddOfficerAsync(Officer officer)
    {
        _context.Officers.Add(officer);
        await _context.SaveChangesAsync();
        return officer;
    }
    
    // Officer soft delete
    public async Task<bool> SoftDeleteOfficerAsync(int officerId)
    {
        var officer = await _context.Officers
            .Include(o => o.Files)
            .FirstOrDefaultAsync(o => o.Id == officerId && !o.IsDeleted);

        if (officer == null) return false;

        officer.IsDeleted = true;
        foreach (var file in officer.Files)
            file.IsDeleted = true; // Officer silinince dosyalar da soft delete

        await _context.SaveChangesAsync();
        return true;
    }

    // Tek dosya soft delete
    public async Task<bool> SoftDeleteFileAsync(int fileId)
    {
        var file = await _context.OfficerFiles.FirstOrDefaultAsync(f => f.Id == fileId && !f.IsDeleted);
        if (file == null) return false;

        file.IsDeleted = true;
        await _context.SaveChangesAsync();
        return true;
    }
}