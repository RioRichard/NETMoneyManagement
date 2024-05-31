using System.ComponentModel.DataAnnotations;

namespace NETAPI.DTO.Request.Category;

public class CategoryQuery : BaseQuery
{
    [MinLength(2)] public string Name { get; set; }
}