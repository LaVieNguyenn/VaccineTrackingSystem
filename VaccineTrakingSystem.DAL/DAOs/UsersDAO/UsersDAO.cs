using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;
namespace VaccineTrakingSystem.DAL.DAOs.UsersDAO
{
    public class UsersDAO : IGenericDAO<User>
    {
        private readonly string _connectionString;

        public UsersDAO(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }
        public async Task<IEnumerable<User>>GetAllAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"SELECT UserID AS UserId, 
                                   Username, 
                                   PasswordHash, 
                                   Email, 
                                   FullName, 
                                   PhoneNumber, 
                                   Address,
                                   RoleID
                            FROM Users 
                            ORDER BY CreatedAt ASC";
                return await connection.QueryAsync<User>(sql);
            }
        }
        public async Task<bool> DeleteAsync(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = "DELETE FROM Users WHERE UserID = @UserId";
                var affectedRows = await connection.ExecuteAsync(sql, new { UserId = userId });
                return affectedRows > 0;
            }
        }
        public async Task<User?> GetByIdAsync(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = "SELECT * FROM Users WHERE UserID = @UserId";
                return await connection.QueryFirstOrDefaultAsync<User>(sql, new { UserId = userId });
            }
        }
        public async Task<int> InsertAsync(User obj)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var checkSql = @"SELECT COUNT(*) FROM Users WHERE Email = @Email OR PhoneNumber = @PhoneNumber";
                int existingCount = await connection.ExecuteScalarAsync<int>(checkSql, new { obj.Email, obj.PhoneNumber });

                if (existingCount > 0)
                {
                    throw new ArgumentException("Email or phone number already exists.");
                }

                var sql = @"INSERT INTO Users (Username, PasswordHash, Email, FullName, PhoneNumber, Address, RoleID, CreatedAt, UpdatedAt) 
                    VALUES (@Username, @PasswordHash, @Email, @FullName, @PhoneNumber, @Address, @RoleID, @CreatedAt, @UpdatedAt);
                    SELECT CAST(SCOPE_IDENTITY() as int);";

                return await connection.ExecuteScalarAsync<int>(sql, new
                {
                    obj.Username,
                    obj.PasswordHash,
                    obj.Email,
                    obj.FullName,
                    obj.PhoneNumber,
                    obj.Address ,
                    obj.RoleId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
            }
        }




        public async Task<bool> UpdateAsync(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"
            UPDATE Users
            SET Username = @Username,
                Email = @Email,
                FullName = @FullName,
                PhoneNumber = @PhoneNumber,
                Address = @Address,
                RoleID = @RoleID,
                UpdatedAt = @UpdatedAt
            WHERE UserID = @UserId";

                var affectedRows = await connection.ExecuteAsync(sql, user);
                return affectedRows > 0;
            }
        }



    }
}
