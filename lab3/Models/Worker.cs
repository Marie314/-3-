using System;
using System.Collections.Generic;

namespace lab3.Models
{
    public partial class Worker
    {
        public Worker()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string MiddleName { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; }
    }
}
