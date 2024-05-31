using System.ComponentModel.DataAnnotations;

namespace NETAPI.DTO.Request.Category;

public class CategoryRequest
{
    [Required]
    [MinLength(6)]
    public string Name { get; set; }
}