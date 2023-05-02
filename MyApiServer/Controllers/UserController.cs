using Data.Contracts;
using Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using WebFramework.Api;
using WebFramework.DTOs;
using WebFramework.ViewModels;

namespace MyApiServer.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
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

        [HttpPost]
        public async Task<ApiResult<UserViewModel>> Create(UserDto userDto, CancellationToken cancellationToken)
        {
            if (await _userRepository.IsExistAsync(u => u.UserName == userDto.UserName,cancellationToken))
                return BadRequest("نام کاربری تکراری است");
            await _userRepository.AddAsync()
        } 
    }
}
