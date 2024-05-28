using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolApp.Core.Entities
{
    public class CourseContact:BaseModel
    {
        public string? SchoolUniName { get; set; }
        public bool IsTeacher { get;set; }
        public bool IsStudent { get;set; }
        public string Email { get;set; }    
        public string FullName { get;set; }
        public string PhoneNumber { get;set; }
        public string? Message { get;set; }
        public int CourseId { get;set; }
        public Course Course { get;set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get;set; }
    }
}
