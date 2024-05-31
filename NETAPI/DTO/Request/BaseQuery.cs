namespace NETAPI.DTO.Request;

public class BaseQuery
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}