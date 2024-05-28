using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolApp.Core.Entities
{
   public class Teacher:BaseModel
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Profession { get; set; }
        [Required]
        public string Image { get; set; }
        public int? CourseId { get; set; }
        public Course? Course { get; set; }
        public string? FaceBook1 { get; set; }
        public string? Instagram { get; set; }
        public string? Linkedin { get; set; }
        public bool ForHomePage { get; set; }
        [NotMapped]
        public IFormFile file { get; set; }
    }
}
