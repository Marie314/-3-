using System;
using System.Collections.Generic;

namespace lab3.Models
{
    public partial class Feedback
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Text { get; set; } = null!;
        public int Mark { get; set; }
        public DateTime DateTime { get; set; }

        public virtual Order Order { get; set; } = null!;
    }
}
