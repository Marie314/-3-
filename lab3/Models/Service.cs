using System;
using System.Collections.Generic;

namespace lab3.Models
{
    public partial class Service
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ServiceKindId { get; set; }
        public int Code { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string Description { get; set; } = null!;

        public virtual Order Order { get; set; } = null!;
        public virtual ServiceKind ServiceKind { get; set; } = null!;
    }
}
