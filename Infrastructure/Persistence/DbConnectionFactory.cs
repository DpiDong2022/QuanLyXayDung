using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence {
    public class DbConnectionFactory {
        private readonly string _connectionString;

        public DbConnectionFactory(IConfiguration config) {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public DbConnectionFactory() {
            _connectionString = ""; 
        }

        public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
    }
}
