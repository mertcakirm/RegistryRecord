using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RegistryRecord.Entities;

public class OfficerFile : BaseEntity
{
    [ForeignKey(nameof(Officer.Id))]
    public int OfficerId { get; set; }
    [JsonIgnore]
    public Officer Officer { get; set; }
    public string FileName { get; set; }
    public string Path { get; set; }
    
}