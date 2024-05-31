using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NETAPI.DTO.Response;

namespace NETAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionTypesController : ControllerBase
    {
        [HttpGet]
        public BaseResponseList<KeyValuePair<string, int>> Get()
        {
            var data = Enum.GetValues(typeof(Models.TransactionType))
                .Cast<int>()
                .Select(p => new KeyValuePair<string?, int>(key: Enum.GetName(typeof(Models.TransactionType), p), value: p))
                .ToList();
            var total = data.Count;

            return new BaseResponseList<KeyValuePair<string, int>>(data, total );
        }
    }
}
