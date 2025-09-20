
namespace RegistryRecord.DTOs
{
    public class OfficerFormDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Unit { get; set; } = null!;
        public string RegistrationNumber { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public List<IFormFile> Files { get; set; } = new List<IFormFile>();
    }
}