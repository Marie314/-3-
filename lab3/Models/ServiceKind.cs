using System;
using System.Collections.Generic;

namespace lab3.Models
{
    public partial class ServiceKind
    {
        public ServiceKind()
        {
            Services = new HashSet<Service>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public virtual ICollection<Service> Services { get; set; }
    }
}
