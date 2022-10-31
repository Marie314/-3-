using System;
using System.Collections.Generic;

namespace lab3.Models
{
    public partial class Order
    {
        public Order()
        {
            Feedbacks = new HashSet<Feedback>();
            Services = new HashSet<Service>();
        }

        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public int WorkerId { get; set; }
        public int ClientId { get; set; }

        public virtual Client Client { get; set; } = null!;
        public virtual Worker Worker { get; set; } = null!;
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Service> Services { get; set; }
    }
}
