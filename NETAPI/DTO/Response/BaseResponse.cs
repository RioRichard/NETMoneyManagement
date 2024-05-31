using System.Net;

namespace NETAPI.DTO.Response;

public class BaseResponse<T>
{
    public T? Data { get; set; }
    public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;
    public string Message { get; set; } = "Success";
    public DateTime RequestDate { get; set; } = DateTime.Now;

    public BaseResponse(){}

    public BaseResponse(Exception e)
    {
        this.Message = e.Message;
        this.Status = HttpStatusCode.InternalServerError;
    }
    public BaseResponse(T? Data)
    {
        this.Data = Data;
    }
}