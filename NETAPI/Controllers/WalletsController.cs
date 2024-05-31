using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NETAPI.DTO.Request;
using NETAPI.DTO.Request.Category;
using NETAPI.DTO.Request.Transaction;
using NETAPI.DTO.Request.Wallet;
using NETAPI.DTO.Response;
using NETAPI.Helper;
using NETAPI.Models;

namespace NETAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WalletsController(ILogger<WalletsController> logger, DataContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<BaseResponseList<Wallet>> Index([FromQuery] WalletQuery query)
        {
            logger.LogInformation("Index");
            try
            {
                var check = SessionHelper.GetUserId(HttpContext, out var id);
                if (check)
                {
                    var queryData = context.Wallets.Where(p => p.AccountId == id);
                    if (!string.IsNullOrEmpty(query.Name))
                    {
                        queryData = queryData.Where(p => p.Name.ToLower().Contains(query.Name));
                    }
                    var pagination = await queryData.Skip(query.Page - 1).Take(query.PageSize).ToListAsync();
                    var countQuery = await queryData.CountAsync();

                    return new BaseResponseList<Wallet>(pagination, countQuery);
                }

                return new BaseResponseList<Wallet>()
                {
                    Message = "Unauthorized",
                    Status = HttpStatusCode.Unauthorized
                };
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return new BaseResponseList<Wallet>(e);
            }
        }

        [HttpGet("{id}")]
        public async Task<BaseResponse<Wallet>> Detail(int id)
        {
            logger.LogInformation("Index");
            try
            {
                var queryData = await context.Wallets.Where(p => p.Id == id).FirstOrDefaultAsync();
                return new BaseResponse<Wallet>(queryData);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return new BaseResponse<Wallet>(e);
            }
        }

        [HttpPost]
        public async Task<BaseResponse<Wallet>> Create(WalletRequest walletRequest)
        {
            try
            {
                var validUser = SessionHelper.GetUserId(HttpContext, out var id);
                if (!validUser)
                {
                    return new BaseResponse<Wallet>()
                    {
                        Message = "Unauthorized",
                        Status = HttpStatusCode.Unauthorized
                    };
                }

                var category = new Wallet()
                {
                    AccountId = id,
                    Name = walletRequest.Name
                };

                await context.Wallets.AddAsync(category);
                await context.SaveChangesAsync();

                return new BaseResponse<Wallet>(category);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                logger.LogError(e.Message);
                return new BaseResponse<Wallet>(e);
            }
        }

        [HttpPut("{id}")]
        public async Task<BaseResponse<Wallet>> Edit(int id, WalletRequest walletRequest)
        {
            try
            {
                var validUser = SessionHelper.GetUserId(HttpContext, out var userId);
                if (!validUser)
                {
                    return new BaseResponse<Wallet>()
                    {
                        Message = "Unauthorized",
                        Status = HttpStatusCode.Unauthorized
                    };
                }

                var wallet = await context.Wallets.Where(p => p.Id == id && p.AccountId == userId)
                    .FirstOrDefaultAsync();
                if (wallet is null)
                {
                    return new BaseResponse<Wallet>()
                    {
                        Message = "Not found",
                        Status = HttpStatusCode.NotFound
                    };
                }

                wallet.Name = walletRequest.Name;

                await context.Wallets.AddAsync(wallet);
                await context.SaveChangesAsync();

                return new BaseResponse<Wallet>(wallet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                logger.LogError(e.Message);
                return new BaseResponse<Wallet>(e);
            }
        }

        [HttpDelete("{id}")]
        public async Task<BaseResponse<Wallet>> Delete(int id)
        {
            try
            {
                var wallet = await context.Wallets.Where(p => p.Id == id)
                    .FirstOrDefaultAsync();
                if (wallet is null)
                {
                    return new BaseResponse<Wallet>()
                    {
                        Message = "Not found",
                        Status = HttpStatusCode.NotFound
                    };
                }

                context.Wallets.Remove(wallet);
                await context.SaveChangesAsync();
                return new BaseResponse<Wallet>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                logger.LogError(e.Message);
                return new BaseResponse<Wallet>(e);
            }
        }

    }
}
