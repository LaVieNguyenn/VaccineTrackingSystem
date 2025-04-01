using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.DAOs.UserDAO
{
    public class UserDAO : IUserDAO
    {
        private readonly string _connectionString;

        public UserDAO(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }


        public async Task<User?> AuthenticateAsync(string phoneNumber, string password)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                // Lấy thông tin user theo username
                var sql = @"
                    SELECT UserID AS UserId, Username, PasswordHash, Email, FullName, PhoneNumber, Address, RoleID, CreatedAt, UpdatedAt
                    FROM Users
                    WHERE PhoneNumber = @PhoneNumber AND PasswordHash = @Password";
                var user = await connection.QueryFirstOrDefaultAsync<User>(sql, new { PhoneNumber = phoneNumber, Password = password});
                if (user != null)
                {
                        return user;
                }
                return null;
            }
        }

        public async Task<int> CreateCustomerAsync(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"
                    INSERT INTO Users (Username, PasswordHash, Email, FullName, PhoneNumber, Address, RoleID, CreatedAt, UpdatedAt)
                    VALUES (@Username, @PasswordHash, @Email, @FullName, @PhoneNumber, @Address, @RoleID, @CreatedAt, @UpdatedAt);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";
                return await connection.QuerySingleAsync<int>(sql, user);
            }
        }

        public Task<bool> DeleteAsync(int obj)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetByIdAsync(int obj)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertAsync(User obj)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(User obj)
        {
            throw new NotImplementedException();
        }
    }
}
