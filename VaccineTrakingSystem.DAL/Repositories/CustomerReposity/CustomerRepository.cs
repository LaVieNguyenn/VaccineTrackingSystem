using VaccineTrakingSystem.DAL.DAOs.ChildDAO;
using VaccineTrakingSystem.DAL.DAOs.UserDAO;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.Repositories.CustomerReposity
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IUserDAO _userDAO;
        private readonly IChildDAO _childDAO;
        public CustomerRepository(IUserDAO userDAO, IChildDAO childDAO)
        {
            _userDAO = userDAO;
            _childDAO = childDAO;
        }

        public async Task<int> RegisterCustomerAsync(User user, List<Child> children)
        {
            int userId = await _userDAO.CreateCustomerAsync(user);

            foreach (var child in children)
            {
                child.ParentId = userId;
                await _childDAO.InsertChildAsync(child);
            }
            return userId;
        }
    }
}
