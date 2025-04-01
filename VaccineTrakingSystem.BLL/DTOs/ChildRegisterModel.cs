using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineTrakingSystem.BLL.DTOs
{
    public class ChildRegisterModel
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        public byte Gender { get; set; }  // 0: Male, 1: Female, 2: Other

        public string? AdditionalInfo { get; set; }
    }
}
