using Application.Models;
using Application.Models.Dto;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;

namespace QuanLyXayDungAPI.Controllers {
    [Route("api/auth")]
    [ApiController]
    public class AuthController: ControllerBase {
        private readonly JwtService _jwtService;
        private readonly AppUserService _userService;
        private readonly IPasswordHasher<AppUserDTO> _passwordHasher;

        public AuthController(JwtService jwtService, AppUserService userService, IPasswordHasher<AppUserDTO> passwordHasher) {
            _jwtService = jwtService;
            _userService = userService;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request) {

            // check input
            // find user ;
            // check password
            // if valid, create token and return
            if (string.IsNullOrEmpty(request.Email))
                return BadRequest(new LoginResponse { Message = "Email không được để trống!" });
            if (string.IsNullOrEmpty(request.Password))
                return BadRequest(new LoginResponse { Message = "Mật khẩu không được để trống!" }); 

            var appUser = await _userService.GetByEmailAsync(request.Email);
            if (appUser == null) {
                return BadRequest(new LoginResponse { Message = "Email hoặc mật khẩu không đúng!" });
            }

            var verifyResult = _passwordHasher.VerifyHashedPassword(appUser, appUser.PasswordHash, request.Password);
            if (verifyResult != PasswordVerificationResult.Success) {
                return BadRequest(new LoginResponse { Message = "Email hoặc mật khẩu không đúng!" });
            }

            var token = _jwtService.GenerateToken(appUser);
            return Ok(new LoginResponse {
                Token = token,
                HoTen = appUser.HoTen,
                Email = appUser.Email,
                Role = appUser.Role.ToString(),
                CongTyID = appUser.CongTyID,
                TenCongTy = appUser.TenCongTy
            });
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult Me() {
            //var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var congTyID = User.FindFirst("congty_id")?.Value;
            var tenCongTy = User.FindFirst("ten_congty")?.Value;
            var hoTen = User.FindFirst(ClaimTypes.Name)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new {
                CongTyID = congTyID,
                TenCongTy = tenCongTy,
                HoTen = hoTen,
                Email = email,
                Role = role
            });
        }
    }
}
