using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RegistryRecord.Data;
using RegistryRecord.DTOs;
using RegistryRecord.Entities;
using RegistryRecord.Services;

namespace RegistryRecord.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize]

public class OfficerController : ControllerBase
{
    private readonly OfficerService  _officerService;
    private readonly IWebHostEnvironment _env;
    public OfficerController(OfficerService officerService, IWebHostEnvironment env)
    {
        _officerService = officerService;
        _env = env;
    }

    [HttpGet]
    public async Task<Officer?> GetOfficers(string registrationNumber,string token)
    {
        if (string.IsNullOrEmpty(registrationNumber));
        if (string.IsNullOrEmpty(token));

        return await _officerService.GetOfficerById(registrationNumber,token);
        
    }
    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(50_000_000)]
    public async Task<IActionResult> CreateOfficer([FromForm] OfficerFormDto dto)
    {
        if (dto == null) return BadRequest("DTO boş olamaz.");

        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        var createdOfficer = await _officerService.CreateOfficerWithFilesAsync(
            dto,
            dto.Files,
            token
        );

        return Ok(createdOfficer);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOfficer(int id)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var success = await _officerService.DeleteOfficerAsync(id, token);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpDelete("file/{fileId}")]
    public async Task<IActionResult> DeleteFile(int fileId)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var success = await _officerService.DeleteFileAsync(fileId, token);
        if (!success) return NotFound();
        return NoContent();
    }
    
}