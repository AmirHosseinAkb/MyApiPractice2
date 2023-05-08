using System.Net;
using Common.Exceptions;
using Data.Contracts;
using Entities.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Services;
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
        private readonly IJwtService _jwtService;
        public UserController(IUserRepository userRepository,IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
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

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<string> Token(string userName, string password, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByUserNameAndPass(userName, password, cancellationToken);
            if (user == null)
                throw new NotFoundException("User Not Found");
            return _jwtService.Generate(user);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiResult<User>> Create(UserDto userDto, CancellationToken cancellationToken)
        {
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
