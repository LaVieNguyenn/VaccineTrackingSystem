using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.DAOs.NotificationDAO
{
    public interface INotificationDAO
    {
        Task<int> InsertNotificationAsync(Notification notification);
        Task<IEnumerable<Notification>> GetNotificationsByUserAsync(int userId);

    }
}
