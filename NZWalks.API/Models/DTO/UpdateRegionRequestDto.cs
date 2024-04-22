using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO;

public class UpdateRegionRequestDto
{
    [Required]
    [MaxLength(100, ErrorMessage = "Name cannot be more than 100 characters.")]
    public string Name { get; set; }
    public string? RegionImageUrl { get; set; }
}