using System.ComponentModel.DataAnnotations;

namespace RegistryRecord.Entities;

public class Officer:BaseEntity
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Unit { get; set; }
    public string RegistrationNumber { get; set; }
    [Required]
    [MaxLength(15)]
    [RegularExpression(@"^\+?[0-9]{10,15}$", ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
    public string PhoneNumber { get; set; }
    
    public ICollection<OfficerFile> Files { get; set; } = new List<OfficerFile>();
}