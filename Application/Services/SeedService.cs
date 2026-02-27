using Application.Interfaces;
using Application.Models.Dto;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services {
    public class SeedService {
        private readonly IAppUserRepository _userRepository;
        private readonly IPasswordHasher<AppUserDTO> _passwordHasher;
        private readonly IConfiguration _configuration;

        public SeedService(IAppUserRepository userRepository, IPasswordHasher<AppUserDTO> passwordHasher, IConfiguration configuration) {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        public async Task EnsureSuperAdminCreated() {
            //var email = Environment.GetEnvironmentVariable("SUPER_ADMIN_EMAIL") ?? string.Empty;
            //var password = Environment.GetEnvironmentVariable("SUPER_ADMIN_PASSWORD") ?? string.Empty;
            var email = _configuration["SuperAdmin:Email"]?? string.Empty;
            var password = _configuration["SuperAdmin:Password"]?? string.Empty;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return;

            var appUsers = await _userRepository.GetListByRole(UserRole.SuperAdmin);
            if (appUsers.Any())
                return;

            var superAdmin = new AppUserDTO {
                HoTen = "Super Admin",
                Email = email,
                Role = UserRole.SuperAdmin
            };
            superAdmin.PasswordHash = _passwordHasher.HashPassword(superAdmin, password);

            var userId = await _userRepository.Insert(superAdmin);
        }
    }
}
