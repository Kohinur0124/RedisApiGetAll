using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using RedisApiGetAll.DataAccess;
using RedisApiGetAll.DTOs;
using RedisApiGetAll.Models;
using System.Text.Json;

namespace RedisApiGetAll.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private DBUser _db;
        private IDistributedCache _distributedCache;

        public UserController(IDistributedCache distributedCache, DBUser db)
        {
            _distributedCache = distributedCache;
            _db = db;
        }

        [HttpGet]
        public async ValueTask<List<User>> GetUsersAsync()
        {
            var users = await _db.Users.ToListAsync();

            
            var json = JsonSerializer.Serialize(users).ToString();
            await _distributedCache.SetStringAsync("Users", json);

            return users;
        }
          
        [HttpPost]
        public async ValueTask<string> AddUserAsync(UserDto user)
        {
            try
            {
                var us = new User
                {
                    UserName = user.UserName,
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password,
                };
                await _db.Users.AddAsync(us);
                await _db.SaveChangesAsync();
                return "Created";
            }
            catch (Exception ex)
            {
                return "Error";
            }
        }
    }
}
