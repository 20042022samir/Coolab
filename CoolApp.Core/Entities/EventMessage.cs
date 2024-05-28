using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolApp.Core.Entities
{
    public class EventMessage:BaseModel
    {
        public string PhoneNumber { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
        public string? Message { get; set; }
        public string UserName { get; set; }
       public string Email { get; set; }
        public bool IsTeacher { get; set; }
        public bool IsStudent { get; set; }
        public string? SchoolUniName { get; set; }
    }
}
