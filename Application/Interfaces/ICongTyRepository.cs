using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces {
    public interface ICongTyRepository : IBaseRepository {
        Task<IEnumerable<CongTy>> GetList();
        Task<CongTy> GetByID(Guid id);
        Task<CongTy> Get(CongTy congTy);
        Task<Guid> Insert(CongTy entity);
        Task Update(CongTy entity);
        Task Delete(Guid id);
    }
}
