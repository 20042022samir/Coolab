using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolApp.Core.Entities
{
    public class EventContact:BaseModel
    {
        public string? Name { get; set; }    
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public int AppUserId { get;set; }
        public AppUser? user { get; set; }
        public int EvenntId { get;set; }
        public Event Event { get; set; }

    }
}
