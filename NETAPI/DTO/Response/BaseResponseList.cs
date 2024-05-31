namespace NETAPI.DTO.Response;

public class BaseResponseList<T> : BaseResponse<T>
{
    public ICollection<T?> Data { get; set; }
    public int Count { get; set; } = 0;
    public int Total { get; set; } = 0;

    public BaseResponseList() : base() {}

    public BaseResponseList(Exception e) : base(e){}

    public BaseResponseList(ICollection<T>? Data, int Total)
    {
        this.Data = Data;
        this.Count = Data.Count;
        this.Total = Total;
    }

}