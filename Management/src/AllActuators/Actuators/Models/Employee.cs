using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Steeltoe.Actuators.Models
{
    public class Employee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Company { get; set; }

        public string Title { get; set; }
    }
}
