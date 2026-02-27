using Application.Interfaces;
using Application.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services {
    public class AppUserService {
        private readonly IAppUserRepository _appUserRepository;
        private readonly IUnitOfWork _uow;

        public AppUserService(IAppUserRepository appUserRepository, IUnitOfWork uow) {
            _appUserRepository = appUserRepository;
            _uow = uow;
        }

        public Task<AppUserDTO?> GetByEmailAsync(string email) {
            return _appUserRepository.GetByEmail(email);
        }

        public async Task<Guid> CreateUserAsync(AppUserDTO user) {
            var id = await _appUserRepository.Insert(user);
            return id;
        }
    }
}
