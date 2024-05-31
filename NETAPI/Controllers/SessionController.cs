using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NETAPI.DTO.Request.Session;
using NETAPI.DTO.Response;
using NETAPI.Helper;
using NETAPI.Models;
using static System.String;

namespace NETAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController(ILogger<SessionController> logger, DataContext context, IConfiguration configuration) : ControllerBase
    {
        [HttpPost]
        public async Task<BaseResponse<string>> New(NewSessionRequest sessionRequest)
        {
            try
            {
                logger.LogInformation("New session");
                var hashedPassword = UserHelper.HashPassword(sessionRequest.Password);
                var account = await context.Accounts.FirstOrDefaultAsync(p =>
                    p.Email == sessionRequest.Email && p.Password == hashedPassword);
                if (account is null)
                {
                    return new BaseResponse<string>()
                    {
                        Data = "",
                        Message = "Unauthorized",
                        Status = HttpStatusCode.Unauthorized,
                    };
                }
                var secret = configuration[AppConstant.JWTKEY] ?? string.Empty;
                var token = SessionHelper.GenerateToken(account, secret);
                return new BaseResponse<string>()
                {
                    Data = token
                };
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return new BaseResponse<string>()
                {
                    Data = "",
                    Message = e.Message,
                    Status = HttpStatusCode.InternalServerError,
                };
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<BaseResponse<Account>> Get()
        {
            try
            {
                logger.LogInformation("Get");
                var claims = SessionHelper.GetUserId(HttpContext,out var id);
                if (!claims)
                {
                    return new BaseResponse<Account>()
                    {
                        Message = "Unauthorized",
                        Status = HttpStatusCode.Unauthorized
                    };
                }

                var account = context.Accounts.FirstOrDefault(p => p.Id == id);
                return new BaseResponse<Account>()
                {
                    Data = account
                };
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return new BaseResponse<Account>(e);
            }
        }
    }
}
