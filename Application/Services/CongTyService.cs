using Application.Interfaces;
using Domain.Entities;
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
            await _uow.BeginTransactionAsync();

            try {
                var existingCongTy = await _congTyRepository.GetByID(entity.ID);
                if (existingCongTy != null) {
                    throw new Exception("Công ty đã tồn tại");
                }
                var result = await _congTyRepository.Insert(entity);
                await _uow.CommitAsync();
                return result;
            } catch (Exception ex) {
                await _uow.RollbackAsync();
                throw;
            }
        }

        public async Task Update(CongTy entity) {
            await _uow.BeginTransactionAsync();
            try {
                await _congTyRepository.Update(entity);
                await _uow.CommitAsync();
            }
            catch (Exception ex) {
                await _uow.RollbackAsync();
                throw;
            }
        }

        public Task Delete(Guid id) {
            var result = _congTyRepository.Delete(id);
            return result;
        }
    }
}
