using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineTrakingSystem.DAL.Helper
{
    public class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
    {
        public override DateOnly Parse(object value)
        {
            if (value is DateTime dt)
                return DateOnly.FromDateTime(dt);
            throw new DataException($"Cannot convert {value.GetType()} to DateOnly");
        }

        public override void SetValue(IDbDataParameter parameter, DateOnly value)
        {
            // Chuyển DateOnly sang DateTime với giờ = 00:00:00
            parameter.Value = value.ToDateTime(new TimeOnly(0, 0));
        }
    }
}
