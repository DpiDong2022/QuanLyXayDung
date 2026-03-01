using Application.Interfaces;
using Domain.Entities;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services {
    public class CongTyService {
        private readonly IUnitOfWork _uow;
        private readonly ICongTyRepository _congTyRepository;

        public CongTyService(IUnitOfWork uow, ICongTyRepository congTyRepository) {
            _uow = uow;
            _congTyRepository = congTyRepository;
        }

        public async Task<List<CongTy>> GetList() {
            return (await _congTyRepository.GetList()).ToList();
        }

        public Task<CongTy> GetByID(Guid id) {
            var congTy = _congTyRepository.GetByID(id);
            return congTy;
        }

        public async Task<Guid> Insert(CongTy entity) {
            try {
                var result = await _congTyRepository.Insert(entity);
                return result;
            }
            catch (PostgresException ex) when (ex.SqlState == "23505") {
                throw new Exception("Mã số thuế đã tồn tại");
            }
        }

        public async Task Update(CongTy entity) {
            try {
                await _congTyRepository.Update(entity);
            } catch (PostgresException ex) when (ex.SqlState == "23505") {
                throw new Exception("Mã số thuế đã tồn tại");
            }
        }

        public Task Delete(Guid id) {
            var result = _congTyRepository.Delete(id);
            return result;
        }
    }
}
