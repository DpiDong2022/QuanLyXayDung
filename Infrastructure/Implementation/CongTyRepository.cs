using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Net.Http.Headers;

namespace Infrastructure.Implementation {
    public class CongTyRepository: ICongTyRepository {
        private readonly IUnitOfWork _uow;

        public CongTyRepository(IUnitOfWork uow) {
            _uow = uow;
        }

        public Task Delete(Guid id) {
            var sql = "DELETE FROM CongTy WHERE id = @id";
            var result = _uow.Connection.ExecuteAsync(sql, new {id}, transaction: _uow.Transaction);
            return result;
        }

        public async Task<CongTy> GetByID(Guid id) {
            var sql = "SELECT * FROM CongTy WHERE id = @id";
            var result = await _uow.Connection.QueryFirstOrDefaultAsync<CongTy>(sql, new {id});
            return result;
        }

        public async Task<CongTy> Get(CongTy value) {
            var sql = @"SELECT * FROM CongTy WHERE id = @ID OR ""MaSoThue"" = @MaSoThue";
            var result = await _uow.Connection.QueryFirstOrDefaultAsync<CongTy>(sql, new {value.ID, value.MaSoThue});
            return result;
        }

        public async Task<IEnumerable<CongTy>> GetList() {
            var sql = "SELECT * FROM CongTy ORDER BY NgayTao DESC";
            var result = await _uow.Connection.QueryAsync<CongTy>(sql);
            return result;
        }

        public Task<Guid> Insert(CongTy entity) {
            var sql = @"INSERT INTO CongTy(
	            Ten, DiaChi, SDT, Email, ""MaSoThue"", ""Plan"")
	            VALUES (@Ten, @DiaChi, @SDT, @Email, @MaSoThue, @Plan)
                RETURNING id;";
            var param = new {
                Ten = entity.Ten,
                SDT = entity.SDT,
                Email = entity.Email,
                DiaChi = entity.DiaChi,
                MaSoThue = entity.MaSoThue,
                Plan = entity.Plan
            };
            var result = _uow.Connection.ExecuteScalarAsync<Guid>(sql, param, transaction: _uow.Transaction);
            return result;
        }

        public Task Update(CongTy entity) {
            var sql = @"UPDATE CongTy SET
	            Ten = @Ten, 
                DiaChi = @DiaChi, 
                SDT = @SDT, 
                Email = @Email,
                ""MaSoThue"" = @MaSoThue,
                ""Plan"" = @Plan
                WHERE id = @Id";
            var param = new {
                Id = entity.ID,
                Ten = entity.Ten,
                SDT = entity.SDT,
                Email = entity.Email,
                DiaChi = entity.DiaChi,
                MaSoThue = entity.MaSoThue,
                Plan = entity.Plan
            };
            var result = _uow.Connection.ExecuteScalarAsync<Guid>(sql, param, transaction: _uow.Transaction);
            return result;
        }
    }
}
