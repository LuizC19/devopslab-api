using System.ComponentModel.DataAnnotations;

namespace DevOpsLab.Dtos;

public class UpdateCategoryDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
}