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
    public class StudentFeedback:BaseModel
    {
        [Required]
        public string FullName { get; set; }
        public string Image { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Profession { get; set; }
        [NotMapped]
        public IFormFile file { get; set; } 
    }
}
