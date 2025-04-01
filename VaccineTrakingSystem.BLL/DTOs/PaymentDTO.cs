using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineTrakingSystem.BLL.DTOs
{
    public class PaymentDTO
    {
        [Required]
        public int AppointmentId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public byte PaymentMethod { get; set; }

        [Required]
        public byte PaymentStatus { get; set; }
    }
}
