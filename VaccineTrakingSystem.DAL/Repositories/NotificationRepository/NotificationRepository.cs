using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.DAOs;
using VaccineTrakingSystem.DAL.DAOs.NotificationDAO;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.Repositories.NotificationRepository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly INotificationDAO _notificationDAO;
        public NotificationRepository(INotificationDAO notificationDAO)
        {
            _notificationDAO = notificationDAO;
        }

        public Task<int> InsertNotificationAsync(Notification notification)
        {
            return _notificationDAO.InsertNotificationAsync(notification);
        }

        public Task<IEnumerable<Notification>> GetNotificationsByUserAsync(int userId)
        {
            return _notificationDAO.GetNotificationsByUserAsync(userId);
        }

    }
}
