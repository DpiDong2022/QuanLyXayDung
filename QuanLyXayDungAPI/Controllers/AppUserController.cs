using Application.Models;
using Application.Models.Dto;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace QuanLyXayDungAPI.Controllers {
    [Route("api/app-user")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,Boss")]
    public class AppUserController: ControllerBase {
        private readonly IPasswordHasher<AppUserDTO> _passwordHasher;
        private readonly AppUserService _userService;

        public AppUserController(IPasswordHasher<AppUserDTO> passwordHasher, AppUserService userService) {
            _passwordHasher = passwordHasher;
            _userService = userService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request) {
            var user = new AppUserDTO{
                HoTen = request.HoTen,
                Email = request.Email,
                CongTyID = request.CongTyID,
                Role = request.Role
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
            var result = await _userService.CreateUserAsync(user);
            return Ok(new { code = 200, message = "Tạo người dùng thành công!" });
        }
    } 
}
