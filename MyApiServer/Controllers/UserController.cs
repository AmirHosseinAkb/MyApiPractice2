using System.Net;
using Data.Contracts;
using Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFramework.Api;
using WebFramework.DTOs;
using WebFramework.Filters;
using WebFramework.ViewModels;

namespace MyApiServer.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    [ApiResultFilter]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("{id}")]
        public async Task<ApiResult<UserViewModel>> Get(int id,CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(cancellationToken,id);
            if (user == null)
                return NotFound();
            return new UserViewModel()
            {
                UserName = user.UserName,
                Age = user.Age,
                FullName = user.FullName,
                Gender = user.Gender,
                Password = user.PasswordHash
            };
        }
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAll(CancellationToken cancellationToken)
        {
            var users =await _userRepository.TableNoTracking.ToListAsync(cancellationToken);
            return Ok(users);
        }

        [HttpPost]
        public async Task<ApiResult<User>> Create(UserDto userDto, CancellationToken cancellationToken)
        {
            if (await _userRepository.IsExistAsync(u => u.UserName == userDto.UserName,cancellationToken))
                return BadRequest("نام کاربری تکراری است");
            var user = new User()
            {
                Age = userDto.Age,
                FullName = userDto.FullName,
                Gender = userDto.Gender,
                UserName = userDto.UserName
            };
            await _userRepository.AddUser(user, userDto.Password, cancellationToken);
            return Ok(user);
        } 
    }
}
