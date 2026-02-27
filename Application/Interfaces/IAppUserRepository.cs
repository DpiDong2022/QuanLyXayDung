using Application.Models.Dto;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces {
    public interface IAppUserRepository {
        Task<AppUserDTO?> GetByEmail(string email);
        Task<IEnumerable<AppUserDTO>> GetListByRole(UserRole role);
        Task<Guid> Insert(AppUserDTO user);
    }
}
