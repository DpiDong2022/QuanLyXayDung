using Application.Interfaces;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence {
    public class UnitOfWork: IUnitOfWork {
        private readonly DbConnectionFactory _factory;
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        public IDbConnection Connection => _connection ??= _factory.CreateConnection();
        public IDbTransaction Transaction => _transaction;

        public UnitOfWork(DbConnectionFactory factory) {
            _factory = factory;
        }

        public Task CommitAsync() {
            this.Transaction?.Commit();
            this.Dispose();
            return Task.CompletedTask;
        }

        public void Dispose() {
            this.Connection?.Dispose();
            this.Transaction?.Dispose();
        }

        public Task RollbackAsync() {
            this.Transaction?.Rollback();
            this.Dispose();
            return Task.CompletedTask;
        }

        public async Task BeginTransactionAsync() {
            if(_connection == null) {
                _connection = _factory.CreateConnection();
            }
            if(_connection.State == ConnectionState.Closed) {
                await ((NpgsqlConnection)_connection).OpenAsync();
            }
            _transaction = _connection.BeginTransaction();
        }
    }
}
