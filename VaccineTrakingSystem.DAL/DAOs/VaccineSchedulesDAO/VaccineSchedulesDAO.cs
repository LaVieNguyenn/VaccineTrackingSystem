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
                var sql = @"
        SELECT 
    vs.ScheduleID AS ScheduleId, 
    vs.ChildID, 
    vs.VaccineID, 
    vs.ScheduledDate, 
    vs.Status, 
    vs.AppointmentID, 
    vs.CreatedAt,
    vs.UpdatedAt,

    -- Child Info
    c.ChildID, 
    c.ParentID, 
    c.FullName AS ChildFullName, 
    c.DateOfBirth, 
    c.Gender, 
    c.AdditionalInfo, 
    c.CreatedAt AS ChildCreatedAt, 
    c.UpdatedAt AS ChildUpdatedAt,

    -- Parent Info
    u.UserID AS ParentID,
    u.FullName AS ParentFullName,
    u.Address AS ParentAddress,
    u.PhoneNumber AS ParentPhoneNumber,

    -- Vaccine Info
    v.VaccineID, 
    v.VaccineName, 
    v.Description AS VaccineDescription, 
    v.NumberOfDoses, 
    v.Manufacturer, 
    v.VaccineType, 
    v.ExpirationPeriod, 
    v.ProductionDate, 
    v.CreatedAt AS VaccineCreatedAt, 
    v.UpdatedAt AS VaccineUpdatedAt,

    -- Appointment Info
    a.AppointmentID,
    a.ServiceID,
    a.AppointmentDate,
    a.BookingDate,
    a.Status AS AppointmentStatus,
    a.PaymentStatus,

    -- Service Info
    s.ServiceName,
s.Description AS ServiceDescription,
s.Price,

    -- Vaccination Record Info
    vr.RecordID,
    vr.VaccinationDate,
    vr.AdverseReaction,
    vr.StaffID,
-- Staff Info
    COALESCE(staff.FullName, 'Unknown') AS StaffFullName
FROM VaccineSchedules vs
JOIN Children c ON vs.ChildID = c.ChildID
JOIN Users u ON c.ParentID = u.UserID
JOIN Vaccines v ON vs.VaccineID = v.VaccineID
LEFT JOIN Appointments a ON vs.AppointmentID = a.AppointmentID
LEFT JOIN Services s ON a.ServiceID = s.ServiceID
LEFT JOIN VaccinationRecords vr ON a.AppointmentID = vr.AppointmentID
LEFT JOIN Users staff ON vr.StaffID = staff.UserID 
WHERE vs.ScheduleID = @ScheduleId";

                var rawData = await connection.QueryAsync<dynamic>(sql, new { ScheduleId = scheduleId });

                var row = rawData.FirstOrDefault();

                if (row == null) return null;
                var firstRow = rawData.First();
                var rowDict = (IDictionary<string, object>)row;
                // Khởi tạo danh sách VaccinationRecords trong Appointment
                var vaccinationRecords = rawData
    .Where(row => row.RecordID != null) // Bỏ qua dòng không có record
    .Select(row => new VaccinationRecord
    {
        RecordId = (int)row.RecordID,
        VaccinationDate = row.VaccinationDate != null ? DateOnly.FromDateTime((DateTime)row.VaccinationDate) : DateOnly.MinValue,
        AdverseReaction = row.AdverseReaction,
        StaffId = row.StaffID != null ? (int)row.StaffID : 0,
        Staff = new User
        {
            UserId = row.StaffID != null ? (int)row.StaffID : 0,
            FullName = row.StaffFullName ?? "Unknown"
        }
    }).ToList();
                return new VaccineSchedule
                {
                    ScheduleId = rowDict.ContainsKey("ScheduleId") ? (int)rowDict["ScheduleId"] : 0,
                    ChildId = rowDict.ContainsKey("ChildID") ? (int)rowDict["ChildID"] : 0,
                    VaccineId = rowDict.ContainsKey("VaccineID") ? (int)rowDict["VaccineID"] : 0,
                    ScheduledDate = rowDict.ContainsKey("ScheduledDate") && rowDict["ScheduledDate"] != DBNull.Value
                        ? DateOnly.FromDateTime((DateTime)rowDict["ScheduledDate"])
                        : DateOnly.MinValue,
                    Status = rowDict.ContainsKey("Status") ? (byte)rowDict["Status"] : (byte)0,
                    AppointmentId = rowDict.ContainsKey("AppointmentID") && rowDict["AppointmentID"] != DBNull.Value
                        ? (int?)rowDict["AppointmentID"]
                        : null,
                    CreatedAt = rowDict.ContainsKey("CreatedAt") ? (DateTime)rowDict["CreatedAt"] : DateTime.MinValue,
                    UpdatedAt = rowDict.ContainsKey("UpdatedAt") ? (DateTime)rowDict["UpdatedAt"] : DateTime.MinValue,

                    Child = new Child
                    {
                        ChildId = rowDict.ContainsKey("ChildID") ? (int)rowDict["ChildID"] : 0,
                        ParentId = rowDict.ContainsKey("ParentID") ? (int)rowDict["ParentID"] : 0,
                        FullName = rowDict.ContainsKey("ChildFullName") ? (string)rowDict["ChildFullName"] : string.Empty,
                        DateOfBirth = rowDict.ContainsKey("DateOfBirth") && rowDict["DateOfBirth"] != DBNull.Value
                            ? DateOnly.FromDateTime((DateTime)rowDict["DateOfBirth"])
                            : DateOnly.MinValue,
                        Gender = rowDict.ContainsKey("Gender") ? (byte)rowDict["Gender"] : (byte)0,
                        AdditionalInfo = rowDict.ContainsKey("AdditionalInfo") && rowDict["AdditionalInfo"] != DBNull.Value
                            ? (string?)rowDict["AdditionalInfo"]
                            : null,
                        CreatedAt = rowDict.ContainsKey("ChildCreatedAt") ? (DateTime)rowDict["ChildCreatedAt"] : DateTime.MinValue,
                        UpdatedAt = rowDict.ContainsKey("ChildUpdatedAt") ? (DateTime)rowDict["ChildUpdatedAt"] : DateTime.MinValue,

                        Parent = new User
                        {
                            UserId = rowDict.ContainsKey("ParentID") ? (int)rowDict["ParentID"] : 0,
                            FullName = rowDict.ContainsKey("ParentFullName") ? (string)rowDict["ParentFullName"] : string.Empty,
                            Address = rowDict.ContainsKey("ParentAddress") ? (string)rowDict["ParentAddress"] : string.Empty,
                            PhoneNumber = rowDict.ContainsKey("ParentPhoneNumber") ? (string)rowDict["ParentPhoneNumber"] : string.Empty
                        }
                    },

                    Vaccine = new Vaccine
                    {
                        VaccineId = rowDict.ContainsKey("VaccineID") ? (int)rowDict["VaccineID"] : 0,
                        VaccineName = rowDict.ContainsKey("VaccineName") ? (string)rowDict["VaccineName"] : string.Empty,
                        Description = rowDict.ContainsKey("Descriptions") ? (string?)rowDict["Descriptions"] : null,
                        NumberOfDoses = rowDict.ContainsKey("NumberOfDoses") ? (int)rowDict["NumberOfDoses"] : 0,
                        Manufacturer = rowDict.ContainsKey("Manufacturer") ? (string)rowDict["Manufacturer"] : string.Empty,
                        VaccineType = rowDict.ContainsKey("VaccineType") ? (string)rowDict["VaccineType"] : string.Empty,
                        ExpirationPeriod = rowDict.ContainsKey("ExpirationPeriod") ? (int)rowDict["ExpirationPeriod"] : 0,
                        ProductionDate = rowDict.ContainsKey("ProductionDate") && rowDict["ProductionDate"] != DBNull.Value
                            ? DateOnly.FromDateTime((DateTime)rowDict["ProductionDate"])
                            : DateOnly.MinValue,
                        CreatedAt = rowDict.ContainsKey("VaccineCreatedAt") ? (DateTime)rowDict["VaccineCreatedAt"] : DateTime.MinValue,
                        UpdatedAt = rowDict.ContainsKey("VaccineUpdatedAt") ? (DateTime)rowDict["VaccineUpdatedAt"] : DateTime.MinValue
                    },

                    Appointment = rowDict.ContainsKey("AppointmentID") && rowDict["AppointmentID"] != DBNull.Value
                        ? new Appointment
                        {
                            AppointmentId = (int)rowDict["AppointmentID"],
                            ServiceId = rowDict.ContainsKey("ServiceID") ? (int)rowDict["ServiceID"] : 0,
                            Service = new Service
                            {
                                ServiceName = rowDict.ContainsKey("ServiceName") ? (string)rowDict["ServiceName"] : string.Empty,
                                Price = rowDict.ContainsKey("Price") ? (decimal)rowDict["Price"] : 0,
                                Description = rowDict.ContainsKey("ServiceDescription") ? (string)rowDict["ServiceDescription"] : string.Empty,

                            },
                            VaccinationRecords = vaccinationRecords,
                            AppointmentDate = rowDict.ContainsKey("AppointmentDate") && rowDict["AppointmentDate"] != DBNull.Value
                                ? DateTime.SpecifyKind((DateTime)rowDict["AppointmentDate"], DateTimeKind.Utc)
                                : DateTime.MinValue,
                            BookingDate = rowDict.ContainsKey("BookingDate") && rowDict["BookingDate"] != DBNull.Value
                                ? DateTime.SpecifyKind((DateTime)rowDict["BookingDate"], DateTimeKind.Utc)
                                : DateTime.MinValue,
                            Status = rowDict.ContainsKey("AppointmentStatus") ? (byte)rowDict["AppointmentStatus"] : (byte)0,
                            PaymentStatus = rowDict.ContainsKey("PaymentStatus") ? (byte)rowDict["PaymentStatus"] : (byte)0,
                            CreatedAt = rowDict.ContainsKey("AppointmentCreatedAt") ? (DateTime)rowDict["AppointmentCreatedAt"] : DateTime.MinValue,
                            UpdatedAt = rowDict.ContainsKey("AppointmentUpdatedAt") ? (DateTime)rowDict["AppointmentUpdatedAt"] : DateTime.MinValue
                        }
                        : null

                };
            }
        }



        public async Task<int> InsertAsync(VaccineSchedule vaccineSchedule)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Gán giá trị ngày giờ hiện tại
                vaccineSchedule.CreatedAt = DateTime.UtcNow;
                vaccineSchedule.UpdatedAt = DateTime.UtcNow;

                var sql = @"
            INSERT INTO VaccineSchedules (ChildID, VaccineID, VaccinationDate, Status, AppointmentID, CreatedAt, UpdatedAt)
            VALUES (@ChildID, @VaccineID, @ScheduledDate, @Status, @AppointmentID, @CreatedAt, @UpdatedAt);
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
