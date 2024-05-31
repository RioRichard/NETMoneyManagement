using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NETAPI.DTO.Request;
using NETAPI.DTO.Request.Category;
using NETAPI.DTO.Response;
using NETAPI.Helper;
using NETAPI.Models;

namespace NETAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController(ILogger<CategoriesController> logger, DataContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<BaseResponseList<Category>> Index([FromQuery] CategoryQuery query)
        {
            logger.LogInformation("Index");
            try
            {
                var check = SessionHelper.GetUserId(HttpContext, out var id);
                if (check)
                {
                    var queryData = context.Categories.Where(p => p.AccountId == id);
                    if (!string.IsNullOrEmpty(query.Name))
                    {
                        queryData = queryData.Where(p => p.Name.ToLower().Contains(query.Name));
                    }
                    var countQuery = await queryData.CountAsync();
                    var pagination = await queryData.Skip(query.Page - 1)
                        .Take(query.PageSize).ToListAsync();

                    return new BaseResponseList<Category>(pagination, countQuery);
                }

                return new BaseResponseList<Category>()
                {
                    Message = "Unauthorized",
                    Status = HttpStatusCode.Unauthorized
                };
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return new BaseResponseList<Category>(e);
            }
        }

        [HttpGet("{id}")]
        public async Task<BaseResponse<Category>> Detail(int id)
        {
            logger.LogInformation("Index");
            try
            {
                var queryData = await context.Categories.Where(p => p.Id == id).FirstOrDefaultAsync();
                return new BaseResponse<Category>(queryData);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return new BaseResponse<Category>(e);
            }
        }

        [HttpPost]
        public async Task<BaseResponse<Category>> Create(CategoryRequest categoryRequest)
        {
            try
            {
                var validUser = SessionHelper.GetUserId(HttpContext, out var id);
                if (!validUser)
                {
                    return new BaseResponse<Category>()
                    {
                        Message = "Unauthorized",
                        Status = HttpStatusCode.Unauthorized
                    };
                }

                var category = new Category()
                {
                    AccountId = id,
                    Name = categoryRequest.Name
                };

                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();

                return new BaseResponse<Category>(category);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                logger.LogError(e.Message);
                return new BaseResponse<Category>(e);
            }
        }

        [HttpPut("{id}")]
        public async Task<BaseResponse<Category>> Edit(int id, CategoryRequest categoryRequest)
        {
            try
            {
                var validUser = SessionHelper.GetUserId(HttpContext, out var userId);
                if (!validUser)
                {
                    return new BaseResponse<Category>()
                    {
                        Message = "Unauthorized",
                        Status = HttpStatusCode.Unauthorized
                    };
                }

                var category = await context.Categories.Where(p => p.Id == id && p.AccountId == userId)
                    .FirstOrDefaultAsync();
                if (category is null)
                {
                    return new BaseResponse<Category>()
                    {
                        Message = "Not found",
                        Status = HttpStatusCode.NotFound
                    };
                }

                category.Name = categoryRequest.Name;

                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();

                return new BaseResponse<Category>(category);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                logger.LogError(e.Message);
                return new BaseResponse<Category>(e);
            }
        }

        [HttpDelete("{id}")]
        public async Task<BaseResponse<Category>> Delete(int id)
        {
            try
            {
                var category = await context.Categories.Where(p => p.Id == id)
                    .FirstOrDefaultAsync();
                if (category is null)
                {
                    return new BaseResponse<Category>()
                    {
                        Message = "Not found",
                        Status = HttpStatusCode.NotFound
                    };
                }

                context.Categories.Remove(category);
                await context.SaveChangesAsync();
                return new BaseResponse<Category>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                logger.LogError(e.Message);
                return new BaseResponse<Category>(e);
            }
        }
    }
}