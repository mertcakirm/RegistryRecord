using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RegistryRecord.Data;
using RegistryRecord.Entities;
using RegistryRecord.Services;

namespace RegistryRecord.Controllers;


[ApiController]
[Route("api/[controller]")]
public class OfficerController
{
    private readonly OfficerService  _officerService;

    public OfficerController(OfficerService officerService)
    {
        _officerService = officerService;
    }

    [HttpGet]
    public async Task<Officer?> GetOfficers(string registrationNumber,string token)
    {
        if (string.IsNullOrEmpty(registrationNumber));
        if (string.IsNullOrEmpty(token));

        return await _officerService.GetOfficerById(registrationNumber,token);
        
    }
    
}