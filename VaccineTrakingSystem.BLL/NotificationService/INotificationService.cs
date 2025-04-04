using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.BLL.NotificationService
{
    public interface INotificationService
    {
        Task<int> CreateNotificationAsync(int userId, int? childId, string message);
        Task<IEnumerable<Notification>> GetNotificationsByUserAsync(int userId);
    }
}
