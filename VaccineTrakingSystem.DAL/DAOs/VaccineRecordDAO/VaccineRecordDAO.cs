using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.DAOs.VaccineRecordDAO
{
    public class VaccineRecordDAO : IGenericDAO<VaccinationRecord>
    {
        private readonly string _connectionString;

        public VaccineRecordDAO(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }
        public async Task<bool> DeleteAsync(int recordId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = "DELETE FROM VaccinationRecords WHERE RecordID = @recordId";
                var affectedRows = await connection.ExecuteAsync(sql, new { RecordID = recordId });
                return affectedRows > 0;
            }
        }

        public async Task<IEnumerable<VaccinationRecord>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"SELECT RecordID AS recordID, 
                                   AppointmentID, 
                                    ChildID, 
                                   VaccineID, 
                                   VaccinationDate, 
                                   AdverseReaction, 
                                   StaffID,
                                    CreatedAt,
                                    UpdatedAt
                            FROM VaccinationRecords 
                            ORDER BY CreatedAt DESC";
                return await connection.QueryAsync<VaccinationRecord>(sql);
            }
        }

        public async Task<VaccinationRecord?> GetByIdAsync(int recordID)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"SELECT RecordID AS recordID, 
                                   AppointmentID, 
                                   ChildID, 
                                   VaccineID, 
                                   VaccinationDate, 
                                   AdverseReaction, 
                                   StaffID,
                                    CreatedAt,
                                    UpdatedAt
                            FROM VaccinationRecords 
                            WHERE ServiceID = @ServiceId";
                return await connection.QueryFirstOrDefaultAsync<VaccinationRecord>(sql, new { RecordID = recordID });
            }
        }

        public async Task<int> InsertAsync(VaccinationRecord record)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                record.CreatedAt = DateTime.UtcNow;
                record.UpdatedAt = DateTime.UtcNow;
                var sql = @"
                    INSERT INTO VaccinationRecords (AppointmentID, ChildID, VaccineID, VaccinationDate, AdverseReaction, StaffID,CreatedAt,UpdatedAt)
                    VALUES (@AppointmentID, @ChildID, @VaccineID, @VaccinationDate, @AdverseReaction, @StaffID,@CreatedAt,@UpdatedAt);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";
                return await connection.QuerySingleAsync<int>(sql, record);
            }
        }

        public async Task<bool> UpdateAsync(VaccinationRecord record)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                record.UpdatedAt = DateTime.UtcNow;
                var sql = @"
                    UPDATE VaccinationRecords
                    SET AppointmentID = @AppointmentID,
                        ChildID = @ChildID,
                        VaccineID = @VaccineID,
                        VaccinationDate = @VaccinationDate,
                        AdverseReaction = @AdverseReaction,
                        StaffID = @StaffID,
                        UpdatedAt = @UpdatedAt
                    WHERE RecordID = @recordID";
                var affectedRows = await connection.ExecuteAsync(sql, record);
                return affectedRows > 0;
            }
        }
    }
}
