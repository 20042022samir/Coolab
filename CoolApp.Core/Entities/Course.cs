using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolApp.Core.Entities
{
    public class Course:BaseModel
    {
        public DateTime RealStartedDate { get; set; }
        public bool StartedDate { get;set; }
        public bool IsSpecial { get; set; }
        public double Price { get; set; }
        public bool ForHomeScreen { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ViewCount { get; set; }
        public int SubjectId { get; set; }  
        public List<CourseContact> CourseContacts { get; set; }
        public Subject Subject { get; set; }
        public List<Teacher> Teachers { get; set; }
        public string Image { get; set; }
        public List<Comment> comments { get;set; }
        [NotMapped]
        public IFormFile file { get;set; }
    }
}
