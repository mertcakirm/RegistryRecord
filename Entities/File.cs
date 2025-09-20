using System.ComponentModel.DataAnnotations.Schema;

namespace RegistryRecord.Entities;

public class OfficerFile : BaseEntity
{
    [ForeignKey(nameof(Officer.Id))]
    public int OfficerId { get; set; }
    public Officer Officer { get; set; }
    public string FileName { get; set; }
    public string Path { get; set; }
    
}