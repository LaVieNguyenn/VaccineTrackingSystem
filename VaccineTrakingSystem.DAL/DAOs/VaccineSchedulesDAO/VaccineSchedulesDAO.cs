using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using VaccineTrakingSystem.DAL.DAOs;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrackingSystem.Controllers
{
    public class VaccineSchedulesDAO : IGenericDAO<VaccineSchedule>
    {
        private readonly string _connectionString;
        public VaccineSchedulesDAO(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<bool> DeleteAsync(int ScheduleId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = "DELETE FROM VaccineSchedules WHERE ScheduleID = @ScheduleId";
                var affectedRows = await connection.ExecuteAsync(sql, new { ScheduleID = ScheduleId });
                return affectedRows > 0;
            }
        }

        public async Task<IEnumerable<VaccineSchedule>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"SELECT vs.ScheduleID AS ScheduleId, 
                   vs.ChildID, 
                           vs.VaccineID, 
                           vs.ScheduledDate, 
                           vs.Status, 
                           vs.AppointmentID, 
                           vs.CreatedAt,
                           vs.UpdatedAt,
                           c.ChildID, 
                           c.ParentID, 
                           c.FullName, 
                           c.DateOfBirth, 
                           c.Gender, 
                           c.AdditionalInfo, 
                           c.CreatedAt AS ChildCreatedAt, 
                           c.UpdatedAt AS ChildUpdatedAt
            FROM VaccineSchedules vs
JOIN Children c ON vs.ChildID = c.ChildID
            ORDER BY CreatedAt DESC";

                var rawData = await connection.QueryAsync<dynamic>(sql);

                var result = rawData.Select(rowObj =>
                {
                    var row = (IDictionary<string, object>)rowObj; // Ép kiểu để truy cập dữ liệu đúng

                    return new VaccineSchedule
                    {
                        ScheduleId = row.ContainsKey("ScheduleId") && row["ScheduleId"] != DBNull.Value ? (int)row["ScheduleId"] : 0,
                        ChildId = row.ContainsKey("ChildID") && row["ChildID"] != DBNull.Value ? (int)row["ChildID"] : 0,
                        VaccineId = row.ContainsKey("VaccineID") && row["VaccineID"] != DBNull.Value ? (int)row["VaccineID"] : 0,
                        ScheduledDate = row.ContainsKey("ScheduledDate") && row["ScheduledDate"] != DBNull.Value
                            ? DateOnly.FromDateTime((DateTime)row["ScheduledDate"])
                            : DateOnly.MinValue,
                        Status = row.ContainsKey("Status") && row["Status"] != DBNull.Value ? (byte)row["Status"] : (byte)0,
                        AppointmentId = row.ContainsKey("AppointmentID") && row["AppointmentID"] != DBNull.Value ? (int?)row["AppointmentID"] : null,
                        CreatedAt = row.ContainsKey("CreatedAt") && row["CreatedAt"] != DBNull.Value ? (DateTime)row["CreatedAt"] : DateTime.MinValue,
                        UpdatedAt = row.ContainsKey("UpdatedAt") && row["UpdatedAt"] != DBNull.Value ? (DateTime)row["UpdatedAt"] : DateTime.MinValue,

                        Child = new Child
                        {
                            ChildId = row.ContainsKey("ChildID") && row["ChildID"] != DBNull.Value ? (int)row["ChildID"] : 0,
                            ParentId = row.ContainsKey("ParentID") && row["ParentID"] != DBNull.Value ? (int)row["ParentID"] : 0,
                            FullName = row.ContainsKey("FullName") && row["FullName"] != DBNull.Value ? (string)row["FullName"] : string.Empty,
                            DateOfBirth = row.ContainsKey("DateOfBirth") && row["DateOfBirth"] != DBNull.Value
                        ? DateOnly.FromDateTime((DateTime)row["DateOfBirth"])
                        : DateOnly.MinValue,
                            Gender = row.ContainsKey("Gender") && row["Gender"] != DBNull.Value ? (byte)row["Gender"] : (byte)0,
                            AdditionalInfo = row.ContainsKey("AdditionalInfo") && row["AdditionalInfo"] != DBNull.Value ? (string?)row["AdditionalInfo"] : null,
                            CreatedAt = row.ContainsKey("ChildCreatedAt") && row["ChildCreatedAt"] != DBNull.Value ? (DateTime)row["ChildCreatedAt"] : DateTime.MinValue,
                            UpdatedAt = row.ContainsKey("ChildUpdatedAt") && row["ChildUpdatedAt"] != DBNull.Value ? (DateTime)row["ChildUpdatedAt"] : DateTime.MinValue
                        }
                    };
                }).ToList();
                return result;

            }
        }

        public async Task<VaccineSchedule?> GetByIdAsync(int scheduleId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"SELECT ScheduleID AS ScheduleId, 
                                   ChildID, 
                                   VaccineID, 
                                   ScheduledDate, 
                                   Status, 
                                   AppointmentID, 
                                   CreatedAt,
                                   UpdatedAt
                            FROM VaccineSchedules 
                            WHERE ScheduleID = @ScheduleId";
                return await connection.QueryFirstOrDefaultAsync<VaccineSchedule>(sql, new { ScheduleId = scheduleId });
            }
        }

        public async Task<int> InsertAsync(VaccineSchedule vaccineSchedule)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"
                    INSERT INTO VaccineSchedules (ChildID, VaccineID, ScheduledDate, Status, AppointmentID,CreatedAt, UpdatedAt)
                    VALUES (@ChildID, @VaccineID, @ScheduledDate, @Status, @AppointmentID, @CreatedAt,@UpdatedAt);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";
                return await connection.QuerySingleAsync<int>(sql, vaccineSchedule);
            }
        }

        public async Task<bool> UpdateAsync(VaccineSchedule vaccineSchedule)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"
                    UPDATE VaccineSchedules
                    SET ChildID = @ChildID,
                        VaccineID = @VaccineID,
                        ScheduledDate = @ScheduledDate,
                        Status = @Status,
                        AppointmentID = @AppointmentID,
                        UpdatedAt = @UpdatedAt,
                        CreatedAt = @CreatedAt
                    WHERE ScheduleID = @ScheduleId";
                var affectedRows = await connection.ExecuteAsync(sql, vaccineSchedule);
                return affectedRows > 0;
            }
        }
    }
}
