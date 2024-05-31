using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using NETAPI.DTO.Request.User;
using NETAPI.DTO.Response;
using NETAPI.Helper;
using NETAPI.Models;

namespace NETAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(ILogger<AccountController> logger, DataContext context, IConfiguration configuration)
        : ControllerBase
    {
        [HttpPost]
        public async Task<BaseResponse<string>> Create(NewUserRequest newUser)
        {
            try
            {
                var account = new Account()
                {
                    Email = newUser.Email.ToLower(),
                    Password = UserHelper.HashPassword(newUser.Password)
                };
                await context.Accounts.AddAsync(account);
                await context.SaveChangesAsync();
                var secret = configuration["Jwt:Secret"];
                var token = SessionHelper.GenerateToken(account, secret);
                return new BaseResponse<string>()
                {
                    Data = token
                };
            }
            catch (Exception e) when (e.InnerException is MySqlException)
            {
                return new BaseResponse<string>()
                {
                    Data = "",
                    Message = e.InnerException.Message,
                    Status = HttpStatusCode.Conflict
                };
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return new BaseResponse<string>(e);
            }
        }
    }
}