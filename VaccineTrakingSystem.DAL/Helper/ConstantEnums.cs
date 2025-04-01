using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineTrakingSystem.DAL.Helper
{
    public static class ConstantEnums
    {
        public enum PaymentMethod
        {
            Cash = 1,
            Momo = 2,
        }

        public enum PaymentStatus
        {
            Pending,
            Paid,
            Failed,
            Cancel,
            Refunded
        }

        public enum Gender
        {
            Male, Female, Other
        }

        public enum AppointmentStatus
        {
            Waiting,
            Process,
            Success,
            Failed,
            Cancelled
        }

    }
}
