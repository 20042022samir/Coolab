using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolApp.Core.Entities
{
    public class Comment:BaseModel
    {
        public string? FullName { get; set; }
        public string? Description { get; set; } 
        public int CourseId { get; set; }
        public Course Course { get; set;}
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public bool ForHomeScreen { get; set; }
    }
}
