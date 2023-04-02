using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace DAL.Servisi
{
    public class DbServis : IDbServis
    {

        private readonly IDbConnection _db;

        private string? connection;

        public DbServis(IConfiguration configuration)
        {
            _db = new NpgsqlConnection(configuration.GetConnectionString("Vdb"));
        }
        public DbServis(string conn)
        {
            _db = new NpgsqlConnection(conn);
        }
        public async Task<T> Dohvati<T>(string command, object parms)
        {
            T result;

            result = await _db.QuerySingleOrDefaultAsync<T>(command,  parms );

            return result;
        }

        public async Task<List<T>> DohvatiSve<T>(string command, object parms)
        {
            List<T> result = new List<T>();

            result = (await _db.QueryAsync<T>(command, parms)).ToList();

            return result;
        }

        public async Task<int> UrediPodatke(string command, object parms)
        {
            int result;

            result = await _db.ExecuteAsync(command, parms);

            return result;
        }
    }
}
