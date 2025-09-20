using RegistryRecord.Entities;
using RegistryRecord.Helpers;
using RegistryRecord.Repositories;

namespace RegistryRecord.Services;

public class OfficerService
{
    private readonly OfficerRepository  _repository;
    private readonly CreteToken _token;

    public OfficerService(OfficerRepository repository, CreteToken token)
    {
        _repository = repository;
        _token = token;
    }

    public async Task<Officer?> GetOfficerById(string registrationNumber, string token)
    {
        var user = _token.GetUserIdFromToken(token);
        Console.WriteLine(token);

        return await _repository.GetOfficer(registrationNumber);
    }


}