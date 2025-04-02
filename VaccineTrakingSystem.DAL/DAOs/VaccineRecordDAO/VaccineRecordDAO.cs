using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
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
                var sql = @"
            SELECT vr.RecordID, 
                   vr.AppointmentID, 
                   vr.ChildID, 
                   vr.VaccineID, 
                   vr.VaccinationDate, 
                   vr.AdverseReaction, 
                   vr.StaffID, 
                   vr.CreatedAt, 
                   vr.UpdatedAt,
                   c.ChildID, c.FullName,  -- Đảm bảo lấy cột ChildID
                  c.ParentId,
                   v.VaccineID, v.VaccineName, -- Đảm bảo lấy cột VaccineID
                   s.UserID AS StaffID, s.FullName  -- Đảm bảo lấy cột StaffID
            FROM VaccinationRecords vr
            LEFT JOIN Children c ON vr.ChildID = c.ChildID
            LEFT JOIN Vaccines v ON vr.VaccineID = v.VaccineID
            LEFT JOIN Users s ON vr.StaffID = s.UserID
            ORDER BY vr.CreatedAt DESC";

                var records = await connection.QueryAsync<VaccinationRecord, Child, Vaccine, User, VaccinationRecord>(
                    sql,
                    (record, child, vaccine, staff) =>
                    {
                        record.Child = child;
                        record.Vaccine = vaccine;
                        record.Staff = staff;
                        return record;
                    },
                    splitOn: "ChildID, VaccineID, StaffID"  // ⚡ Đúng cột để Dapper phân tách
                );

                return records;
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
                            WHERE RecordID = @recordID";
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
            INSERT INTO VaccinationRecords (AppointmentID, ChildID, VaccineID, VaccinationDate, AdverseReaction, StaffID, CreatedAt, UpdatedAt)
            VALUES (@AppointmentID, @ChildID, @VaccineID, @VaccinationDate, @AdverseReaction, @StaffID, @CreatedAt, @UpdatedAt);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

                var param = new
                {
                    record.AppointmentId,
                    record.ChildId,
                    record.VaccineId,
                    VaccinationDate = record.VaccinationDate.ToDateTime(TimeOnly.MinValue), // 🔥 Chuyển DateOnly → DateTime
                    record.AdverseReaction,
                    record.StaffId,
                    record.CreatedAt,
                    record.UpdatedAt
                };

                return await connection.QuerySingleAsync<int>(sql, param);
            }
        }

        public Task<int> InsertAsyncc(VaccinationRecord obj)
        {
            throw new NotImplementedException();
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
