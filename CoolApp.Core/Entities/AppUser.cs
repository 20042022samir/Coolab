using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolApp.Core.Entities
{
    public class AppUser:IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<Contact> Contacts { get; set; }
        public bool IsStudent { get; set; }
        public bool IsEmployee { get; set; }
        public bool IsTeacher { get; set; }
        public string? UserNamee { get; set; }
        public string? SchoolUni { get; set; }
        public List<CourseContact> CourseContacts { get; set; }
        public List<EventContact> EventContacts { get; set; }
        public List<Comment> Comments { get; set; } 
    }
}
