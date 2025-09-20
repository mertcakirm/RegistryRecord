using RegistryRecord.DTOs;
using RegistryRecord.Entities;
using RegistryRecord.Helpers;
using RegistryRecord.Repositories;

namespace RegistryRecord.Services;

public class OfficerService
{
    private readonly OfficerRepository _repository;
    private readonly CreteToken _token;

    public OfficerService(OfficerRepository repository, CreteToken token)
    {
        _repository = repository;
        _token = token;
    }


    public async Task<Officer?> GetOfficerById(string registrationNumber, string token)
    {
        var userId = _token.GetUserIdFromToken(token);

        return await _repository.GetOfficer(registrationNumber);
    }

    public async Task<Officer> CreateOfficerWithFilesAsync(
        OfficerFormDto dto, 
        List<IFormFile> files, 
        string token)
    {
        var userId = _token.GetUserIdFromToken(token);

        var isOfficer = await _repository.IsOfficerExists(dto.RegistrationNumber);
        if (isOfficer)
            throw new Exception("Officer already exists");
        
        var officer = new Officer
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Unit = dto.Unit,
            PhoneNumber = dto.PhoneNumber,
            RegistrationNumber = dto.RegistrationNumber
        };


        var folderPath = Path.Combine("wwwroot", "officerfiles", dto.RegistrationNumber);
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        foreach (var file in files)
        {
            if (file.Length <= 0) continue;

            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(folderPath, uniqueFileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            officer.Files.Add(new OfficerFile
            {
                FileName = file.FileName,
                Path = $"/officerfiles/{dto.RegistrationNumber}/{uniqueFileName}"
            });
        }

        return await _repository.AddOfficerAsync(officer);
    }

    

    public async Task<bool> DeleteOfficerAsync(int officerId, string token)
    {
        var userId = _token.GetUserIdFromToken(token);
        return await _repository.SoftDeleteOfficerAsync(officerId);
    }

    public async Task<bool> DeleteFileAsync(int fileId, string token)
    {
        var userId = _token.GetUserIdFromToken(token);
        return await _repository.SoftDeleteFileAsync(fileId);
    }
}