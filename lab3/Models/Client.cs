using System;
using System.Collections.Generic;

namespace lab3.Models
{
    public partial class Client
    {
        public Client()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string MiddleName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Telephone { get; set; } = null!;
        public string Description { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; }
    }
}
