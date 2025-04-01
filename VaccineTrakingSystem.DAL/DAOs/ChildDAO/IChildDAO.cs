using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.DAOs.ChildDAO
{
    public interface IChildDAO
    {
        Task<int> InsertChildAsync(Child child);
    }
}
