using Application.Interfaces;
using Application.Models.Dto;
using Dapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementation {
    public class AppUserRepository: IAppUserRepository {
        private readonly IUnitOfWork _uow;

        public AppUserRepository(IUnitOfWork uow) {
            _uow = uow;
        }

        public Task<AppUserDTO?> GetByEmail(string email) {
            var sql = @"SELECT u.ID, u.CongTyID, c.Ten as TenCongTy, u.HoTen, u.Email, u.PasswordHash, u.Role, u.Status, u.CreatedAt
                        FROM app_users u
                        LEFT JOIN congty c ON u.CongTyID = c.id
                        WHERE u.Email = @Email";
            return _uow.Connection.QueryFirstOrDefaultAsync<AppUserDTO>(sql, new { Email = email }, transaction: _uow.Transaction);
        }

        public Task<IEnumerable<AppUserDTO>> GetListByRole(UserRole role) {
            var sql = @"SELECT u.ID, u.CongTyID, c.Ten as TenCongTy, u.HoTen, u.Email, u.PasswordHash, u.Role, u.Status, u.CreatedAt
                        FROM app_users u
                        LEFT JOIN congty c ON u.CongTyID = c.id
                        WHERE u.Role = @Role::user_role";
            return _uow.Connection.QueryAsync<AppUserDTO>(sql, new { Role = role.ToString().ToLower() }, transaction: _uow.Transaction);
        }

        public Task<Guid> Insert(AppUserDTO user) {
            var sql = @"INSERT INTO app_users(
                CongTyID, HoTen, Email, PasswordHash, Role)
                VALUES (@CongTyID, @HoTen, @Email, @PasswordHash, @Role::user_role)
                RETURNING ID;";
            var param = new {
                user.CongTyID,
                user.HoTen,
                user.Email,
                user.PasswordHash,
                Role = user.Role.ToString().ToLower()
            };

            return _uow.Connection.ExecuteScalarAsync<Guid>(sql, param, transaction: _uow.Transaction);
        }
    }
}
