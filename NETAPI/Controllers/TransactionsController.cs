using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NETAPI.DTO.Request.Transaction;
using NETAPI.DTO.Response;
using NETAPI.Helper;
using NETAPI.Models;

namespace NETAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionsController(ILogger<TransactionsController> logger, DataContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<BaseResponseList<Transaction>> Index([FromQuery] TransactionQuery queryData)
        {
            try
            {
                var validUser = SessionHelper.GetUserId(HttpContext, out var userId);
                if (!validUser)
                {
                    return new BaseResponseList<Transaction>()
                    {
                        Message = "Unauthorized",
                        Status = HttpStatusCode.Unauthorized
                    };
                }

                var query = context.Transactions.Where(p => p.AccountId == userId);

                if (!string.IsNullOrEmpty(queryData.Name))
                {
                    query = query.Where(p => p.Name.ToLower().Contains(queryData.Name));
                }
                if (queryData.CategoryId != null)
                {
                    query = query.Where(p => p.CategoryId == queryData.CategoryId);
                }
                if (queryData.WalletId != null)
                {
                    query = query.Where(p => p.SourceWalletId == queryData.WalletId);
                }
                if (queryData.FromAmount != null)
                {
                    query = query.Where(p=> p.Amount >= queryData.FromAmount);
                }
                if (queryData.ToAmount != null)
                {
                    query = query.Where(p => p.Amount <= queryData.ToAmount);
                }
                if (queryData.FromProcessDate != null)
                {
                    query = query.Where(p => p.ProcessDate >= queryData.FromProcessDate);
                }
                if (queryData.ToProcessDate != null)
                {
                    query = query.Where(p => p.ProcessDate <= queryData.ToProcessDate);
                }
                
                var total = await query.CountAsync();
                var data = await query
                    .OrderByDescending(p=>p.ProcessDate)
                    .Skip(queryData.Page - 1)
                    .Take(queryData.PageSize)
                    .ToListAsync();

                return new BaseResponseList<Transaction>(data, total);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                logger.LogError(e.Message);
                return new BaseResponseList<Transaction>(e);
            }
        }

        [HttpGet("{id}")]
        public async Task<BaseResponse<Transaction>> Detail(int id)
        {
            logger.LogInformation("Detail");
            try
            {
                var validUser = SessionHelper.GetUserId(HttpContext, out var userId);
                if (!validUser)
                {
                    return new BaseResponseList<Transaction>()
                    {
                        Message = "Unauthorized",
                        Status = HttpStatusCode.Unauthorized
                    };
                }

                var queryData = await context.Transactions.Where(p => p.Id == id && p.AccountId == userId)
                    .FirstOrDefaultAsync();
                if (queryData is null)
                {
                    return new BaseResponse<Transaction>()
                    {
                        Message = "Not found",
                        Status = HttpStatusCode.NotFound
                    };
                }
                return new BaseResponse<Transaction>(queryData);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return new BaseResponse<Transaction>(e);
            }
        }

        [HttpDelete("{Id}")]
        public async Task<BaseResponse<object>> Delete(int id)
        {
            logger.LogInformation("Delete");
            try
            {
                var validUser = SessionHelper.GetUserId(HttpContext, out var userId);
                if (!validUser)
                {
                    return new BaseResponseList<object>()
                    {
                        Message = "Unauthorized",
                        Status = HttpStatusCode.Unauthorized
                    };
                }

                var queryData = await context.Transactions.Where(p => p.Id == id && p.AccountId == userId)
                    .FirstOrDefaultAsync();
                if (queryData is null)
                {
                    return new BaseResponse<object>()
                    {
                        Message = "Not found",
                        Status = HttpStatusCode.NotFound
                    };
                }

                context.Transactions.Remove(queryData);
                await context.SaveChangesAsync();
                return new BaseResponse<object>();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return new BaseResponse<object>(e);
            }
        }

        [HttpPost]
        public async Task<BaseResponse<Transaction>> Create(TransactionRequest transactionRequest)
        {
            logger.LogInformation("Create");
            try
            {
                var validUser = SessionHelper.GetUserId(HttpContext, out var userId);
                if (!validUser)
                {
                    return new BaseResponseList<Transaction>()
                    {
                        Message = "Unauthorized",
                        Status = HttpStatusCode.Unauthorized
                    };
                }

                var transaction = new Transaction()
                {
                    AccountId = userId,
                    Amount = transactionRequest.Amount,
                    CategoryId = transactionRequest.CategoryId,
                    SourceWalletId = transactionRequest.SourceWalletId,
                    DestWalletId = transactionRequest.DestWalletId,
                    Type = transactionRequest.Type
                };
                await context.Transactions.AddAsync(transaction);
                await context.SaveChangesAsync();

                return new BaseResponse<Transaction>(transaction);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return new BaseResponse<Transaction>(e);
            }
        }

        [HttpPut("{id}")]
        public async Task<BaseResponse<Transaction>> Update(int id, TransactionRequest transactionRequest)
        {
            logger.LogInformation("Create");
            try
            {
                var validUser = SessionHelper.GetUserId(HttpContext, out var userId);
                if (!validUser)
                {
                    return new BaseResponseList<Transaction>()
                    {
                        Message = "Unauthorized",
                        Status = HttpStatusCode.Unauthorized
                    };
                }

                var transaction =
                    await context.Transactions.FirstOrDefaultAsync(p => p.AccountId == userId && p.Id == id);
                if (transaction is null)
                {
                    return new BaseResponse<Transaction>()
                    {
                        Message = "Not found",
                        Status = HttpStatusCode.NotFound
                    };
                }

                transaction.Amount = transactionRequest.Amount;
                transaction.CategoryId = transactionRequest.CategoryId;
                transaction.SourceWalletId = transactionRequest.SourceWalletId;
                transaction.DestWalletId = transactionRequest.DestWalletId;
                transaction.Type = transactionRequest.Type;

                context.Transactions.Update(transaction);
                await context.SaveChangesAsync();

                return new BaseResponse<Transaction>(transaction);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return new BaseResponse<Transaction>(e);
            }
        }
    }
}
