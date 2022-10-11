using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ServerSide.Domain.Models;
using ServerSide.Infra.Repository.Interface;

namespace ServerSide.Infra.Repository
{
    
    public class UserRepository : IUserRepository
    {
        private IConfiguration _configuration;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<User> GetUsers()
        {
            using (var dbContext = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                dbContext.Open();
                var sql = "SELECT * FROM dbo.Users";
                var resul = dbContext.Query<User>(sql);
                return resul;
            }
        }

        public User GetUsersByName(string name, string pwd)
        {
            using (var dbContext = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                dbContext.Open();
                var sql = "SELECT * FROM dbo.Users Where Name = @name and Pwd = @pwd";
                
                var parameters = new
                {
                    pwd = pwd,
                    name = name
                };

                var resul = dbContext.QuerySingleOrDefault<User>(sql, parameters);
                return resul;
            }
        }
    }
}
